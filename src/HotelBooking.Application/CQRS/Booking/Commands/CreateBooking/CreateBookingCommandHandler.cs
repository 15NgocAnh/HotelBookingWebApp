using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Application.CQRS.Booking.Commands.CreateBooking
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Result<int>>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookingCommandHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate room exists and is available
                var room = await _roomRepository.GetByIdAsync(request.RoomId);
                if (room == null)
                    return Result<int>.Failure("Room not found");

                if (room.Status != Domain.AggregateModels.RoomAggregate.RoomStatus.Available)
                    return Result<int>.Failure("Room is not available");

                // Validate dates
                if (request.CheckInDate >= request.CheckOutDate)
                    return Result<int>.Failure("Check-in date must be before check-out date");

                if (request.CheckInDate < DateTime.Today)
                    return Result<int>.Failure("Check-in date cannot be in the past");

                // Check room availability for the date range
                if (!await _bookingRepository.IsRoomAvailableForBookingAsync(request.RoomId, request.CheckInDate, request.CheckOutDate))
                    return Result<int>.Failure("Room is not available for the selected dates");

                // Process guests - check for existing guests and use their info if found
                var guests = new List<Guest>();
                foreach (var guestDto in request.Guests)
                {
                    var existingGuest = await _bookingRepository.FindExistingGuestAsync(guestDto.CitizenIdNumber, guestDto.PassportNumber);
                    if (existingGuest != null)
                    {
                        // Use existing guest info
                        guests.Add(existingGuest);
                    }
                    else
                    {
                        // Create new guest
                        guests.Add(new Guest(
                            guestDto.CitizenIdNumber,
                            guestDto.PassportNumber,
                            guestDto.FirstName,
                            guestDto.LastName,
                            guestDto.PhoneNumber
                        ));
                    }
                }

                // Create booking with the first guest
                var booking = new Domain.AggregateModels.BookingAggregate.Booking(request.RoomId);

                // Add remaining guests and update booking details
                booking.Update(
                    request.RoomId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    guests,
                    request.Notes
                );

                // Add booking to repository
                await _bookingRepository.AddAsync(booking);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<int>.Success(booking.Id);
            }
            catch (DomainException ex)
            {
                return Result<int>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error creating booking: {ex.Message}");
            }
        }
    }
}
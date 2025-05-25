using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;

namespace HotelBooking.Application.CQRS.Booking.Commands.UpdateBooking;

public class UpdateBookingCommandHandler : IRequestHandler<UpdateBookingCommand, Result>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IExtraItemRepository _extraItemRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookingCommandHandler(
        IBookingRepository bookingRepository,
        IRoomRepository roomRepository,
        IExtraItemRepository extraItemRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _extraItemRepository = extraItemRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Get existing booking
            var booking = await _bookingRepository.GetByIdAsync(request.Id);
            if (booking == null)
                return Result.Failure("Booking not found");

            // Validate room exists and is available
            if (request.RoomId != null && request.CheckInDate != null && request.CheckOutDate != null)
            {
                if (request.RoomId != null)
                {
                    var room = await _roomRepository.GetByIdAsync(request.RoomId ?? 0);
                    if (room == null)
                        return Result.Failure("Room not found");

                    if (room.Status != Domain.AggregateModels.RoomAggregate.RoomStatus.Available)
                        return Result.Failure("Room is not available");
                }
                // Validate dates
                if (request.CheckInDate != null && request.CheckOutDate != null)
                {
                    if (request.CheckInDate >= request.CheckOutDate)
                        return Result.Failure("Check-in date must be before check-out date");

                    if (request.CheckInDate < DateTime.Today)
                        return Result.Failure("Check-in date cannot be in the past");
                }

                // Check room availability for the date range (excluding current booking)
                if (!await _bookingRepository.IsRoomAvailableForBookingAsync(request.RoomId ?? 0, request.CheckInDate ?? DateTime.Now, request.CheckOutDate ?? DateTime.Now, request.Id))
                    return Result.Failure("Room is not available for the selected dates");

                // Convert guest DTOs to domain entities
                var guests = request.Guests.Select(g => new Guest(
                    g.CitizenIdNumber,
                    g.PassportNumber,
                    g.FirstName,
                    g.LastName,
                    g.PhoneNumber
                )).ToList();

                // Update booking details
                booking.Update(
                    request.RoomId ?? 0,
                    request.CheckInDate ?? DateTime.Now,
                    request.CheckOutDate ?? DateTime.Now,
                    guests,
                    request.Notes
                );
            }

            // Handle extra usages
            var existingExtraUsages = booking.ExtraUsages.ToList();
            booking.ClearExtraUsages();

            foreach (var extraUsage in request.ExtraUsages)
            {
                var extraItem = await _extraItemRepository.GetByIdAsync(extraUsage.ExtraItemId);
                if (extraItem == null)
                    return Result.Failure($"Extra item with ID {extraUsage.ExtraItemId} not found");

                booking.AddExtraUsage(extraItem.Id, extraItem.Name, extraUsage.Quantity, extraItem.Price);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (DomainException ex)
        {
            return Result.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error updating booking: {ex.Message}");
        }
    }
} 
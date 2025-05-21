using AutoMapper;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

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

                // Convert guest DTOs to domain entities
                var guests = request.Guests.Select(g => new Guest(
                    g.CitizenIdNumber,
                    g.PassportNumber,
                    g.FirstName,
                    g.LastName,
                    g.PhoneNumber
                )).ToList();

                // Create booking with the first guest
                var booking = new Domain.AggregateModels.BookingAggregate.Booking(guests.First());

                // Add remaining guests and update booking details
                booking.Update(
                    request.RoomId,
                    request.CheckInDate,
                    request.CheckOutDate,
                    guests.Skip(1),
                    request.SpecialRequests
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
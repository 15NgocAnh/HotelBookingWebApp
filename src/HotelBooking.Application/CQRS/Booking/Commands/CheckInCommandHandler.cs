using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands
{
    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public CheckInCommandHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Result> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(request.Id);
                if (booking == null)
                    return Result.Failure("Booking not found");

                if (booking.Status != BookingStatus.Confirmed)
                    return Result.Failure("Only confirmed bookings can be checked in");

                var room = await _roomRepository.GetByIdAsync(booking.RoomId);
                if (room == null)
                    return Result.Failure("Room not found");

                if (room.Status != RoomStatus.Available)
                    return Result.Failure("Room is not available for check-in");

                booking.CheckIn();
                room.UpdateStatus(RoomStatus.Booked);

                await _bookingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Error during check-in: {ex.Message}");
            }
        }
    }
} 
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;

        public CheckOutCommandHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Result> Handle(CheckOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(request.Id);
                if (booking == null)
                    return Result.Failure("Booking not found");

                if (booking.Status != Domain.AggregateModels.BookingAggregate.BookingStatus.CheckedIn)
                    return Result.Failure("Only checked-in bookings can be checked out");

                var room = await _roomRepository.GetByIdAsync(booking.RoomId);
                if (room == null)
                    return Result.Failure("Room not found");

                booking.CheckOut();
                room.UpdateStatus(RoomStatus.CleaningUp);

                await _bookingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (System.Exception ex)
            {
                return Result.Failure($"Error during check-out: {ex.Message}");
            }
        }
    }
} 
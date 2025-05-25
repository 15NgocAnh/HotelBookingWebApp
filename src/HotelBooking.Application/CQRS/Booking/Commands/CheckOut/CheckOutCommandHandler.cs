using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.Booking.Commands.CheckOut
{
    public class CheckOutCommandHandler : IRequestHandler<CheckOutCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckOutCommandHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
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

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error during check-out: {ex.Message}");
            }
        }
    }
}
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Exceptions;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands.CheckIn
{
    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckInCommandHandler(
            IBookingRepository bookingRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
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

                // Perform check-in
                booking.CheckIn();
                if (!string.IsNullOrWhiteSpace(request.Notes))
                {
                    booking.Update(
                        booking.RoomId,
                        booking.CheckInTime,
                        booking.CheckOutTime,
                        booking.Guests,
                        booking.SpecialRequests,
                        request.Notes
                    );
                }

                // Update room status
                room.UpdateStatus(RoomStatus.Booked);

                // Save changes
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DomainException ex)
            {
                return Result.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error during check-in: {ex.Message}");
            }
        }
    }
}
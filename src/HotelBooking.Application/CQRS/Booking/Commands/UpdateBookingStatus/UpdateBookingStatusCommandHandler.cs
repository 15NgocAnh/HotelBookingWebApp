using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;

        public UpdateBookingStatusCommandHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<Result> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(request.Id);
                if (booking == null)
                    return Result.Failure("Booking not found");

                booking.UpdateStatus(request.Status);
                await _bookingRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating booking status: {ex.Message}");
            }
        }
    }
}
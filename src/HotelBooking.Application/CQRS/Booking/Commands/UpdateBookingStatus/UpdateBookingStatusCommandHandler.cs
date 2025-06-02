using HotelBooking.Domain.Common;

namespace HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler : IRequestHandler<UpdateBookingStatusCommand, Result>
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookingStatusCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
        {
            _bookingRepository = bookingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(request.Id);
                if (booking == null)
                    return Result.Failure("Booking not found");

                booking.UpdateStatus(request.Status);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error updating booking status: {ex.Message}");
            }
        }
    }
}
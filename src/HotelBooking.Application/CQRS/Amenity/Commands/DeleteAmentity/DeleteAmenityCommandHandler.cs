namespace HotelBooking.Application.CQRS.Amenity.Commands.DeleteAmentity
{
    public class DeleteAmenityCommandHandler : IRequestHandler<DeleteAmenityCommand, Result>
    {
        private readonly IAmenityRepository _amenityRepository;

        public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository));
        }

        public async Task<Result> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing amenity to check if it exists
                var amenity = await _amenityRepository.GetByIdAsync(request.Id);

                if (amenity == null)
                {
                    return Result.Failure($"Amenity with ID {request.Id} not found");
                }

                // Delete amenity
                await _amenityRepository.DeleteAsync(request.Id);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete amenity: {ex.Message}");
            }
        }
    }
}
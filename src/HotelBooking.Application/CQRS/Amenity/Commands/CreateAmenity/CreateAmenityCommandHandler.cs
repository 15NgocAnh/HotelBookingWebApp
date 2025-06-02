namespace HotelBooking.Application.CQRS.Amenity.Commands.CreateAmenity
{
    public class CreateAmenityCommandHandler : IRequestHandler<CreateAmenityCommand, Result<int>>
    {
        private readonly IAmenityRepository _amenityRepository;

        public CreateAmenityCommandHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository));
        }

        public async Task<Result<int>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Create new amenity domain entity
                var amenity = new Domain.AggregateModels.AmenityAggregate.Amenity(request.Name);

                // Add amenity to repository
                await _amenityRepository.AddAsync(amenity);

                // Return the newly created amenity ID
                return Result<int>.Success(amenity.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create amenity: {ex.Message}");
            }
        }
    }
}
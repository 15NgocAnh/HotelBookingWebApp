namespace HotelBooking.Application.CQRS.Building.Commands
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, Result<int>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IHotelRepository _hotelRepository;

        public CreateBuildingCommandHandler(
            IBuildingRepository buildingRepository,
            IHotelRepository hotelRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
            _hotelRepository = hotelRepository;
        }

        public async Task<Result<int>> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Kiểm tra quyền truy cập hotel
                if (request.HotelIds != null && request.HotelIds.Any() && !request.HotelIds.Contains(request.HotelId))
                {
                    return Result<int>.Failure("Access denied: You don't have permission to create building for this hotel.");
                }

                // Check if building name is unique in the hotel
                var isNameUnique = await _buildingRepository.IsNameUniqueInHotelAsync(request.HotelId, request.Name);
                if (!isNameUnique)
                {
                    return Result<int>.Failure($"A building with name '{request.Name}' already exists in this hotel");
                }

                // Create new building domain entity
                var building = new Domain.AggregateModels.BuildingAggregate.Building(request.HotelId, request.Name, request.TotalFloors);
                
                // Add building to repository
                await _buildingRepository.AddAsync(building);
                
                // Return the newly created building ID
                return Result<int>.Success(building.Id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Failed to create building: {ex.Message}");
            }
        }
    }
} 
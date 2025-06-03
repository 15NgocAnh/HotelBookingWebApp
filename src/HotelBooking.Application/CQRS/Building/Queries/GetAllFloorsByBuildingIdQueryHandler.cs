using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public class GetAllFloorsByBuildingIdQueryHandler : IRequestHandler<GetAllFloorsByBuildingIdQuery, Result<List<FloorDto>>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IMapper _mapper;

        public GetAllFloorsByBuildingIdQueryHandler(
            IBuildingRepository buildingRepository, 
            IMapper mapper)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
            _mapper = mapper;
        }

        public async Task<Result<List<FloorDto>>> Handle(GetAllFloorsByBuildingIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Kiểm tra quyền truy cập building
                var building = await _buildingRepository.GetByIdAsync(request.Id);
                if (building == null)
                {
                    return Result<List<FloorDto>>.Failure($"Building with ID {request.Id} not found");
                }

                // Kiểm tra quyền truy cập hotel
                if (request.HotelIds != null && request.HotelIds.Any() && !request.HotelIds.Contains(building.HotelId))
                {
                    return Result<List<FloorDto>>.Failure("Access denied: Building does not belong to your hotels.");
                }

                var floors = await _buildingRepository.GetAllFloorsByBuildingIdAsync(request.Id);
                
                return Result<List<FloorDto>>.Success(_mapper.Map<List<FloorDto>>(floors));
            }
            catch (Exception ex)
            {
                return Result<List<FloorDto>>.Failure($"Failed to get all floors: {ex.Message}");
            }
        }
    }
} 
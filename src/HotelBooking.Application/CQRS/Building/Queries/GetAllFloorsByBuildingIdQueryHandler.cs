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
                var floors = await _buildingRepository.GetAllFloorsByBuildingIdAsync(request.Id);
                
                return Result<List<FloorDto>>.Success(_mapper.Map<List<FloorDto>>(floors));
            }
            catch (Exception ex)
            {
                return Result<List<FloorDto>>.Failure($"Failed to get all buildings: {ex.Message}");
            }
        }
    }
} 
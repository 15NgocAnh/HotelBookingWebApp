using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public class GetAllBuildingsQueryHandler : IRequestHandler<GetAllBuildingsQuery, Result<List<BuildingDto>>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public GetAllBuildingsQueryHandler(
            IBuildingRepository buildingRepository, 
            IHotelRepository hotelRepository,
            IMapper mapper)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<Result<List<BuildingDto>>> Handle(GetAllBuildingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var buildings = await _buildingRepository.GetAllAsync();
                var buildingDtos = _mapper.Map<List<BuildingDto>>(buildings);
                
                foreach (var buildingDto in buildingDtos)
                {
                    var hotel = await _hotelRepository.GetByIdAsync(buildingDto.HotelId);
                    buildingDto.HotelName = hotel?.Name;
                    buildingDto.TotalFloors = buildingDto.Floors.Count;
                }

                return Result<List<BuildingDto>>.Success(buildingDtos);
            }
            catch (Exception ex)
            {
                return Result<List<BuildingDto>>.Failure($"Failed to get all buildings: {ex.Message}");
            }
        }
    }
} 
using HotelBooking.Application.CQRS.Building.DTOs;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, Result<BuildingDto>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IHotelRepository _hotelRepository;

        public GetBuildingByIdQueryHandler(
            IBuildingRepository buildingRepository,
            IHotelRepository hotelRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
            _hotelRepository = hotelRepository;
        }

        public async Task<Result<BuildingDto>> Handle(GetBuildingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var building = await _buildingRepository.GetByIdAsync(request.Id);
                
                if (building == null)
                {
                    return Result<BuildingDto>.Failure($"Building with ID {request.Id} not found");
                }

                // Kiểm tra quyền truy cập hotel
                if (request.HotelIds != null && request.HotelIds.Any() && !request.HotelIds.Contains(building.HotelId))
                {
                    return Result<BuildingDto>.Failure("Access denied: Building does not belong to your hotels.");
                }
                
                var buildingDto = new BuildingDto
                {
                    Id = building.Id,
                    HotelId = building.HotelId,
                    Name = building.Name,
                    TotalFloors = building.Floors.Count,
                    Floors = building.Floors.Select(f => new FloorDto
                    {
                        Number = f.Number,
                        Name = f.Name
                    }).ToList()
                };

                var hotel = await _hotelRepository.GetByIdAsync(building.HotelId);
                buildingDto.HotelName = hotel?.Name;
                
                return Result<BuildingDto>.Success(buildingDto);
            }
            catch (Exception ex)
            {
                return Result<BuildingDto>.Failure($"Failed to get building: {ex.Message}");
            }
        }
    }
} 
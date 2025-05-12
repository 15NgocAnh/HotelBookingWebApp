using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public class GetBuildingByIdQueryHandler : IRequestHandler<GetBuildingByIdQuery, Result<BuildingDto>>
    {
        private readonly IBuildingRepository _buildingRepository;

        public GetBuildingByIdQueryHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
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
                
                return Result<BuildingDto>.Success(buildingDto);
            }
            catch (Exception ex)
            {
                return Result<BuildingDto>.Failure($"Failed to get building: {ex.Message}");
            }
        }
    }
} 
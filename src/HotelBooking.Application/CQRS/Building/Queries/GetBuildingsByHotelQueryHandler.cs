using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Queries
{
    public class GetBuildingsByHotelQueryHandler : IRequestHandler<GetBuildingsByHotelQuery, Result<List<BuildingDto>>>
    {
        private readonly IBuildingRepository _buildingRepository;

        public GetBuildingsByHotelQueryHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<Result<List<BuildingDto>>> Handle(GetBuildingsByHotelQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var buildings = await _buildingRepository.GetBuildingsByHotelAsync(request.HotelId);
                
                var buildingDtos = buildings.Select(b => new BuildingDto
                {
                    Id = b.Id,
                    HotelId = b.HotelId,
                    Name = b.Name,
                    TotalFloors = b.Floors.Count,
                    Floors = b.Floors.Select(f => new FloorDto
                    {
                        Number = f.Number,
                        Name = f.Name
                    }).ToList()
                }).ToList();
                
                return Result<List<BuildingDto>>.Success(buildingDtos);
            }
            catch (Exception ex)
            {
                return Result<List<BuildingDto>>.Failure($"Failed to get buildings: {ex.Message}");
            }
        }
    }
} 
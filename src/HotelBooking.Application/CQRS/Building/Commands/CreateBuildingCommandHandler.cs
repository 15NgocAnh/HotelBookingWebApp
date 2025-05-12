using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public class CreateBuildingCommandHandler : IRequestHandler<CreateBuildingCommand, Result<int>>
    {
        private readonly IBuildingRepository _buildingRepository;

        public CreateBuildingCommandHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<Result<int>> Handle(CreateBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
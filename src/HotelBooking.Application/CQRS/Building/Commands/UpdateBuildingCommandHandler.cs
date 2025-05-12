using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public class UpdateBuildingCommandHandler : IRequestHandler<UpdateBuildingCommand, Result>
    {
        private readonly IBuildingRepository _buildingRepository;

        public UpdateBuildingCommandHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<Result> Handle(UpdateBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing building
                var building = await _buildingRepository.GetByIdAsync(request.Id);
                
                if (building == null)
                {
                    return Result.Failure($"Building with ID {request.Id} not found");
                }

                // Check if the hotel ID matches
                if (building.HotelId != request.HotelId)
                {
                    return Result.Failure($"Building with ID {request.Id} does not belong to hotel with ID {request.HotelId}");
                }
                
                // Check if building name is unique in the hotel (if name is being changed)
                if (building.Name != request.Name)
                {
                    var isNameUnique = await _buildingRepository.IsNameUniqueInHotelAsync(request.HotelId, request.Name);
                    if (!isNameUnique)
                    {
                        return Result.Failure($"A building with name '{request.Name}' already exists in this hotel");
                    }
                }
                
                // Update building properties
                building.Update(request.Name, request.TotalFloors);
                
                // Save changes
                await _buildingRepository.UpdateAsync(building);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update building: {ex.Message}");
            }
        }
    }
} 
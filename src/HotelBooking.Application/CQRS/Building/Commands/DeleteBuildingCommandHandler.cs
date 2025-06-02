using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public class DeleteBuildingCommandHandler : IRequestHandler<DeleteBuildingCommand, Result>
    {
        private readonly IBuildingRepository _buildingRepository;

        public DeleteBuildingCommandHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository ?? throw new ArgumentNullException(nameof(buildingRepository));
        }

        public async Task<Result> Handle(DeleteBuildingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing building to check if it exists
                var building = await _buildingRepository.GetByIdAsync(request.Id);
                
                if (building == null)
                {
                    return Result.Failure($"Building with ID {request.Id} not found");
                }
                
                // Delete building
                await _buildingRepository.DeleteAsync(request.Id);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to delete building: {ex.Message}");
            }
        }
    }
} 
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Commands
{
    public class UpdateAmenityCommandHandler : IRequestHandler<UpdateAmenityCommand, Result>
    {
        private readonly IAmenityRepository _amenityRepository;

        public UpdateAmenityCommandHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository));
        }

        public async Task<Result> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get existing amenity
                var amenity = await _amenityRepository.GetByIdAsync(request.Id);
                
                if (amenity == null)
                {
                    return Result.Failure($"Amenity with ID {request.Id} not found");
                }
                
                // Update amenity properties
                amenity.Update(request.Name);
                
                // Save changes
                await _amenityRepository.UpdateAsync(amenity);
                
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure($"Failed to update amenity: {ex.Message}");
            }
        }
    }
} 
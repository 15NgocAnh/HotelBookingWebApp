using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Queries
{
    public class GetAmenityByIdQueryHandler : IRequestHandler<GetAmenityByIdQuery, Result<AmenityDto>>
    {
        private readonly IAmenityRepository _amenityRepository;

        public GetAmenityByIdQueryHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository));
        }

        public async Task<Result<AmenityDto>> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(request.Id);
                
                if (amenity == null)
                {
                    return Result<AmenityDto>.Failure($"Amenity with ID {request.Id} not found");
                }
                
                var amenityDto = new AmenityDto
                {
                    Id = amenity.Id,
                    Name = amenity.Name
                };
                
                return Result<AmenityDto>.Success(amenityDto);
            }
            catch (Exception ex)
            {
                return Result<AmenityDto>.Failure($"Failed to get amenity: {ex.Message}");
            }
        }
    }
} 
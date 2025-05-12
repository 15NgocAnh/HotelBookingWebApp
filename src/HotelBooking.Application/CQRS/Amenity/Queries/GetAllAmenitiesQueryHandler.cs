using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Queries
{
    public class GetAllAmenitiesQueryHandler : IRequestHandler<GetAllAmenitiesQuery, Result<List<AmenityDto>>>
    {
        private readonly IAmenityRepository _amenityRepository;

        public GetAllAmenitiesQueryHandler(IAmenityRepository amenityRepository)
        {
            _amenityRepository = amenityRepository ?? throw new ArgumentNullException(nameof(amenityRepository));
        }

        public async Task<Result<List<AmenityDto>>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var amenities = await _amenityRepository.GetAllAsync();
                
                var amenityDtos = amenities.Select(a => new AmenityDto
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList();
                
                return Result<List<AmenityDto>>.Success(amenityDtos);
            }
            catch (Exception ex)
            {
                return Result<List<AmenityDto>>.Failure($"Failed to get amenities: {ex.Message}");
            }
        }
    }
} 
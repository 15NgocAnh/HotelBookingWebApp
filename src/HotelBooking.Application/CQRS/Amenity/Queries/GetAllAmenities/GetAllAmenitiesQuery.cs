using HotelBooking.Application.CQRS.Amenity.DTOs;

namespace HotelBooking.Application.CQRS.Amenity.Queries.GetAllAmenities
{
    public record GetAllAmenitiesQuery : IQuery<Result<List<AmenityDto>>>;
}
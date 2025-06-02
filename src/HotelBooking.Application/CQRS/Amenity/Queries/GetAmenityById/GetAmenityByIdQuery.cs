using HotelBooking.Application.CQRS.Amenity.DTOs;

namespace HotelBooking.Application.CQRS.Amenity.Queries.GetAmenityById
{
    public record GetAmenityByIdQuery(int Id) : IQuery<Result<AmenityDto>>;
}
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;

namespace HotelBooking.Application.CQRS.Amenity.Queries.GetAmenityById
{
    public record GetAmenityByIdQuery(int Id) : IQuery<Result<AmenityDto>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
}
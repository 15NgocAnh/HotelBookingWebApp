using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Queries
{
    public record GetAllAmenitiesQuery : IRequest<Result<List<AmenityDto>>>;
} 
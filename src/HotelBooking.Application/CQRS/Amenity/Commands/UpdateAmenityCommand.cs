using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Commands
{
    public record UpdateAmenityCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
} 
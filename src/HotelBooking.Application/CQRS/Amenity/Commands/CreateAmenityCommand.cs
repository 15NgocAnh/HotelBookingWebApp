using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Amenity.Commands
{
    public record CreateAmenityCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
    }
} 
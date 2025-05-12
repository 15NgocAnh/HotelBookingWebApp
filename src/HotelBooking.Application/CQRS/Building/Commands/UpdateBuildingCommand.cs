using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record UpdateBuildingCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public int HotelId { get; init; }
        public string Name { get; init; }
        public int TotalFloors { get; init; }
    }
} 
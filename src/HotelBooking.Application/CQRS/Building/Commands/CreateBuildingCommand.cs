using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record CreateBuildingCommand : IRequest<Result<int>>
    {
        public int HotelId { get; init; }
        public string Name { get; init; }
        public int TotalFloors { get; init; }
    }
} 
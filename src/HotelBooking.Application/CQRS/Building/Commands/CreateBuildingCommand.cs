using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record CreateBuildingCommand : ICommand<Result<int>>
    {
        public int HotelId { get; init; }
        public string Name { get; init; }
        public int TotalFloors { get; init; }
    }
} 
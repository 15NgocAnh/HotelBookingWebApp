using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record UpdateBuildingCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public int HotelId { get; init; }
        public string Name { get; init; }
        public int TotalFloors { get; init; }
        public List<int> HotelIds { get; set; } = new();
    }
} 
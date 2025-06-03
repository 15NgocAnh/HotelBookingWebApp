using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record DeleteBuildingCommand(int Id) : ICommand<Result>
    {
        public List<int> HotelIds { get; set; } = new();
    }
} 
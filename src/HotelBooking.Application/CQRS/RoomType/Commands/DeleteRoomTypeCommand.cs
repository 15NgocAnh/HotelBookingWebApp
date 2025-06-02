using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public record DeleteRoomTypeCommand(int Id) : ICommand<Result>;
} 
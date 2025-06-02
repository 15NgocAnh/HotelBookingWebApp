using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record DeleteRoomCommand(int Id) : ICommand<Result>;
} 
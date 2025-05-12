using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record CreateRoomCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
        public int FloorId { get; init; }
        public int RoomTypeId { get; init; }
    }
} 
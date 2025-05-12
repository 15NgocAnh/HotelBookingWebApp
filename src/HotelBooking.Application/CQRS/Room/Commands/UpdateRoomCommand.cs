using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record UpdateRoomCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public int FloorId { get; init; }
        public int RoomTypeId { get; init; }
        public string Status { get; init; }
    }
} 
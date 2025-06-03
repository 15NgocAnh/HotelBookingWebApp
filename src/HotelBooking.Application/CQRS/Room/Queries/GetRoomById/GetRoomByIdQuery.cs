using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetRoomById
{
    public record GetRoomByIdQuery(int Id) : IQuery<Result<RoomDto>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
}
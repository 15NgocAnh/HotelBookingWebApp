using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetAllRooms
{
    public record GetAllRoomsQuery() : IQuery<Result<List<RoomDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
}
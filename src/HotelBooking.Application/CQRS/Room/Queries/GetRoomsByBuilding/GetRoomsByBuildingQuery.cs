using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetRoomsByBuilding
{
    public record GetRoomsByBuildingQuery(int BuildingId) : IQuery<Result<List<RoomDto>>>
    {
        public List<int> HotelIds { get; set; } = new();
    }
}
using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries
{
    public record GetRoomsByBuildingQuery(int BuildingId) : IQuery<Result<List<RoomDto>>>;
}
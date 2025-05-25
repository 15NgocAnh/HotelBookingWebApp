using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries.GetAllRoomAvailable
{
    public record GetAllRoomsAvailableQuery : IQuery<Result<List<RoomDto>>>;
}
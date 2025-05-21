using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.DTOs;

namespace HotelBooking.Application.CQRS.Room.Queries
{
    public record GetAllRoomsQuery : IQuery<Result<List<RoomDto>>>;
} 
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public record GetRoomTypeByIdQuery(int Id) : IQuery<Result<RoomTypeDto>>;
} 
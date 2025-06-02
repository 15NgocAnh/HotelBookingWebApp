using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public record GetAllRoomTypesQuery : IQuery<Result<List<RoomTypeDto>>>;
} 
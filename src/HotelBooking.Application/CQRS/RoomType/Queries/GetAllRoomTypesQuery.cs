using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using MediatR;
using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public record GetAllRoomTypesQuery : IRequest<Result<List<RoomTypeDto>>>;
} 
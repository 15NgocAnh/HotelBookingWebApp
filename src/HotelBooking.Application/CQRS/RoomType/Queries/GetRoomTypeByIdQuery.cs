using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Queries
{
    public record GetRoomTypeByIdQuery(int Id) : IRequest<Result<RoomTypeDto>>;
} 
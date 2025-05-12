using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Queries
{
    public record GetAllRoomsQuery : IRequest<Result<List<RoomDto>>>;
} 
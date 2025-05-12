using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.RoomType.Commands
{
    public record DeleteRoomTypeCommand(int Id) : IRequest<Result>;
} 
using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands
{
    public record DeleteRoomCommand(int Id) : IRequest<Result>;
} 
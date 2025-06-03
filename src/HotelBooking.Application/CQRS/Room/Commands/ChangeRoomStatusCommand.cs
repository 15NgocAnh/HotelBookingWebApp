using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using MediatR;

namespace HotelBooking.Application.CQRS.Room.Commands;

public class ChangeRoomStatusCommand : IRequest<Result>
{
    public int Id { get; set; }
    public RoomStatus Status { get; set; }
} 
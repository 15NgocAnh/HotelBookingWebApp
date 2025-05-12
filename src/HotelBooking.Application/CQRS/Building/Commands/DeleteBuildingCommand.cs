using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Building.Commands
{
    public record DeleteBuildingCommand(int Id) : IRequest<Result>;
} 
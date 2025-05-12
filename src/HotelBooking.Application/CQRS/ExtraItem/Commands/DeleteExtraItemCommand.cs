using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record DeleteExtraItemCommand(int Id) : IRequest<Result>;
} 
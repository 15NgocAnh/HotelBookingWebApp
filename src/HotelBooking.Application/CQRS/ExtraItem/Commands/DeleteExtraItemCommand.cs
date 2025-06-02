using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record DeleteExtraItemCommand(int Id) : ICommand<Result>;
} 
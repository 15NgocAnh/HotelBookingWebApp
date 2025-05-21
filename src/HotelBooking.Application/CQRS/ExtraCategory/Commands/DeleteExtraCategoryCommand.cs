using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record DeleteExtraCategoryCommand(int Id) : ICommand<Result>;
} 
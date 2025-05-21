using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record UpdateExtraCategoryCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
} 
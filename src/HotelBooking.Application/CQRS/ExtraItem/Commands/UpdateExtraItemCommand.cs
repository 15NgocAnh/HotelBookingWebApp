using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record UpdateExtraItemCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public int ExtraCategoryId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
} 
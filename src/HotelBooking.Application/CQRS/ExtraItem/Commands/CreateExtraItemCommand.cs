using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record CreateExtraItemCommand : ICommand<Result<int>>
    {
        public int ExtraCategoryId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
} 
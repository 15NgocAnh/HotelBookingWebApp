using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record CreateExtraItemCommand : IRequest<Result<int>>
    {
        public int ExtraCategoryId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
} 
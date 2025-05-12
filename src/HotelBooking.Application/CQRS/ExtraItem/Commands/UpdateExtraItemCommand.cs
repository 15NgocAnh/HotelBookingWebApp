using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraItem.Commands
{
    public record UpdateExtraItemCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public int ExtraCategoryId { get; init; }
        public string Name { get; init; }
        public decimal Price { get; init; }
    }
} 
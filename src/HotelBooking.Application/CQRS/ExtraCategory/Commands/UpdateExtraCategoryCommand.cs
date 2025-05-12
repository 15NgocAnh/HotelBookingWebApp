using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record UpdateExtraCategoryCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
} 
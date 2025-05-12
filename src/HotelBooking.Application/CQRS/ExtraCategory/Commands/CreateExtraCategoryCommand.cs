using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.ExtraCategory.Commands
{
    public record CreateExtraCategoryCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
    }
} 
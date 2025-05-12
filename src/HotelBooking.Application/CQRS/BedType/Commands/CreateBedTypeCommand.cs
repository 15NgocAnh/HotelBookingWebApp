using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands
{
    public record CreateBedTypeCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
    }
} 
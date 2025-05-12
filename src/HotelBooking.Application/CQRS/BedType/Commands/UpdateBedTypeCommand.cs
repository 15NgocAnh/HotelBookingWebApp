using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.BedType.Commands
{
    public record UpdateBedTypeCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
} 
using HotelBooking.Application.Common.Models;
using MediatR;

namespace HotelBooking.Application.CQRS.Hotel.Commands
{
    public record CreateHotelCommand : IRequest<Result<int>>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public string Address { get; init; }
        public string Phone { get; init; }
        public string Email { get; init; }
        public string Website { get; init; }
    }
} 
using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Hotel.Commands.CreateHotel
{
    public record CreateHotelCommand : ICommand<Result<int>>
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? Address { get; init; }
        public string Phone { get; init; }
        public string? Email { get; init; }
        public string? Website { get; init; }
    }
}
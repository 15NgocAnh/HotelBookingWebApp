using HotelBooking.Application.Common.Base;
using HotelBooking.Application.Common.Models;

namespace HotelBooking.Application.CQRS.Hotel.Commands.UpdateHotel
{
    public record UpdateHotelCommand : ICommand<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public string? Address { get; init; }
        public string Phone { get; init; }
        public string? Email { get; init; }
        public string? Website { get; init; }
    }
}
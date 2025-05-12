using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Commands
{
    public record CreateBookingCommand : IRequest<Result<int>>
    {
        public int RoomId { get; init; }
        public int CustomerId { get; init; }
        public DateTime CheckInDate { get; init; }
        public DateTime CheckOutDate { get; init; }
        public string SpecialRequests { get; init; }
        public List<GuestDto> Guests { get; init; }
    }
} 
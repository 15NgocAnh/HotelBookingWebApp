using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetPendingCheckins
{
    public record GetPendingCheckinsQuery : IRequest<Result<List<BookingDto>>>
    {
        public DateTime Date { get; init; }
        public List<int> HotelIds { get; set; } = new();
    }
}
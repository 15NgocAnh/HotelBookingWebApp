using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries
{
    public record GetPendingCheckinsQuery : IRequest<Result<List<BookingDto>>>
    {
        public DateTime Date { get; init; }
    }
} 
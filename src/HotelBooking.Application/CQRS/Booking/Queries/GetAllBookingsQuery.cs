using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.DTOs;
using MediatR;

namespace HotelBooking.Application.CQRS.Booking.Queries
{
    public record GetAllBookingsQuery : IRequest<Result<List<BookingDto>>>
    {
        public bool IncludeInactive { get; init; }
    }
} 
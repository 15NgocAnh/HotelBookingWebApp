using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetBookingById
{
    public record GetBookingByIdQuery : IRequest<Result<BookingDto>>
    {
        public int Id { get; init; }
    }
}
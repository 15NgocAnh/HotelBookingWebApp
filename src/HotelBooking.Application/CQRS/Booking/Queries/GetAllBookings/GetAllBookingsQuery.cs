using HotelBooking.Application.CQRS.Booking.DTOs;

namespace HotelBooking.Application.CQRS.Booking.Queries.GetAllBookings
{
    public record GetAllBookingsQuery : IQuery<Result<List<BookingDto>>>
    {
        public bool IncludeInactive { get; init; }
        public List<int> HotelIds { get; set; } = new();
    }
}
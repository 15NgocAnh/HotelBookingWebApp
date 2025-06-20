using HotelBooking.Domain.AggregateModels.BookingAggregate;

namespace HotelBooking.Application.CQRS.Booking.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal RoomPrice { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<GuestDto> Guests { get; set; } = new();
        public List<ExtraUsageDto> ExtraUsages { get; set; } = new();
    }
}
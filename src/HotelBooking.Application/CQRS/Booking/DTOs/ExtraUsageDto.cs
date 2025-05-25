namespace HotelBooking.Application.CQRS.Booking.DTOs
{
    public class ExtraUsageDto
    {
        public int? Id { get; set; }

        public int ExtraItemId { get; set; }

        public int Quantity { get; set; }
    }
}

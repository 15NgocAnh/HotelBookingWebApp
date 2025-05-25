namespace HotelBooking.Application.CQRS.Booking.DTOs
{
    public class GuestDto
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CitizenIdNumber { get; set; }
        public string? PassportNumber { get; set; }
    }
} 
namespace HotelBooking.Application.CQRS.Hotel.DTOs
{
    public class HotelDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Website { get; set; }
        public int TotalBuildings { get; set; }
    }
} 
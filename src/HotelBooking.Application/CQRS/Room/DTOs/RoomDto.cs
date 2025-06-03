namespace HotelBooking.Application.CQRS.Room.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int FloorId { get; set; }
        public string FloorName { get; set; } = string.Empty;
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; } = string.Empty;
        public decimal RoomTypePrice { get; set; }
        public int HotelId { get; set; }
    }
} 
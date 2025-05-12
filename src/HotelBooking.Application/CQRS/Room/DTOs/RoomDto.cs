namespace HotelBooking.Application.CQRS.Room.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int FloorId { get; set; }
        public string FloorName { get; set; }
        public int RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public decimal RoomTypePrice { get; set; }
    }
} 
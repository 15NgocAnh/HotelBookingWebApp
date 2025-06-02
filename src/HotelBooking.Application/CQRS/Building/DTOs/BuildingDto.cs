namespace HotelBooking.Application.CQRS.Building.DTOs
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string? HotelName { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TotalFloors { get; set; }
        public int TotalRooms { get; set; }
        public List<FloorDto> Floors { get; set; } = new();
    }
} 
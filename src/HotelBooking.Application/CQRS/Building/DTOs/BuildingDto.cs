namespace HotelBooking.Application.CQRS.Building.DTOs
{
    public class BuildingDto
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public string Name { get; set; }
        public int TotalFloors { get; set; }
        public List<FloorDto> Floors { get; set; } = new();
    }
} 
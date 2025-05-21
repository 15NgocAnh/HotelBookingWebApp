namespace HotelBooking.Application.CQRS.RoomType.DTOs
{
    public class RoomTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int TotalRooms { get; set; }
        public List<BedTypeSetupDetailDto> BedTypeSetupDetails { get; set; } = new();
        public List<AmenitySetupDetailDto> AmenitySetupDetails { get; set; } = new();
    }

    public class BedTypeSetupDetailDto
    {
        public int Id { get; set; }
        public int BedTypeId { get; set; }
        public string BedTypeName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public bool IsSelected { get; set; }
    }

    public class AmenitySetupDetailDto
    {
        public int Id { get; set; }
        public int AmenityId { get; set; }
        public string AmenityName { get; set; } = string.Empty;
        public int Quantity { get; set; } = 0;
        public bool IsSelected { get; set; }
    }
} 
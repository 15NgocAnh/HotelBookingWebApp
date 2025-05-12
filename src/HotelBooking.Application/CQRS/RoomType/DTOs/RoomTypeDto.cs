using System.Collections.Generic;

namespace HotelBooking.Application.CQRS.RoomType.DTOs
{
    public class RoomTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int TotalRooms { get; set; }
        public List<BedTypeSetupDetailDto> BedTypeSetupDetails { get; set; }
        public List<AmenitySetupDetailDto> AmenitySetupDetails { get; set; }
    }

    public class BedTypeSetupDetailDto
    {
        public int Id { get; set; }
        public int BedTypeId { get; set; }
        public string BedTypeName { get; set; }
        public int Quantity { get; set; }
    }

    public class AmenitySetupDetailDto
    {
        public int Id { get; set; }
        public int AmenityId { get; set; }
        public string AmenityName { get; set; }
        public int Quantity { get; set; }
    }
} 
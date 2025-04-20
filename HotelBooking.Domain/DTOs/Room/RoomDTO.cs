using HotelBooking.Data.Models;

namespace HotelBooking.Domain.DTOs.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string RoomNo { get; set; } // Người dùng nhập thủ công, không tự sinh

        public string RoomType { get; set; } // Loại phòng

        public string Status { get; set; } // Available, Booked, Maintenance

        public decimal PricePerNight { get; set; }

        public int Area { get; set; } // Diện tích (m²)

        public string? ImageUrl { get; set; } // Room image

        public int MaxOccupancy { get; set; }

        public int BedCount { get; set; }

        public List<Facility> Facilities { get; set; } = new();

        public string? Description { get; set; }
    }
}

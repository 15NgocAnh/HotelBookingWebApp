using HotelBooking.Data.Models;

namespace HotelBooking.Domain.DTOs.Room
{
    public class RoomDetailsDTO
    {
        public int Id { get; set; }

        public string RoomNo { get; set; } // Người dùng nhập thủ công, không tự sinh

        public int RoomType { get; set; } // Loại phòng

        public int BedCount { get; set; } // Số giường

        public int MaxOccupancy { get; set; } // Số người tối đa

        public decimal PricePerNight { get; set; } // Giá mỗi đêm

        public int Area { get; set; } // Diện tích (m²)

        public string? Description { get; set; } // Mô tả phòng

        public List<Facility>? Facilities { get; set; } // Danh sách tiện nghi (VD: "WiFi, TV, Điều hòa, Bồn tắm")

        public string? ImageUrl { get; set; } // Hình ảnh của phòng

        public string Status { get; set; }
    }
}

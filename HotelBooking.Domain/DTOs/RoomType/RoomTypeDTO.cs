using System.ComponentModel;

namespace HotelBooking.Domain.DTOs.RoomType
{
    public class RoomTypeDTO
    {
        public int Id { get; set; }
        [DisplayName("Loại phòng")]
        public string Name { get; set; }

        [DisplayName("Ghi chú")]
        public string Description { get; set; }
        [DisplayName("Người lớn")]
        public int NumberOfAdults { get; set; }

        [DisplayName("Trẻ em")]
        public int NumberOfChildrent { get; set; }
        [DisplayName("Ký hiệu loại phòng")]
        public string? RoomTypeSymbol { get; set; }
    }

    public class CreateRoomTypeDTO
    {
        [DisplayName("Loại phòng")]
        public string Name { get; set; }

        [DisplayName("Ký hiệu loại phòng")]
        public string? RoomTypeSymbol { get; set; }

        [DisplayName("Người lớn")]
        public int NumberOfAdults { get; set; }

        [DisplayName("Trẻ em")]
        public int NumberOfChildrent { get; set; }

        [DisplayName("Ghi chú")]
        public string? Description { get; set; }
    }

    public class UpdateRoomTypeDTO
    {
        public int Id { get; set; }
        [DisplayName("Loại phòng")]
        public string Name { get; set; }
        [DisplayName("Ký hiệu loại phòng")]
        public string? RoomTypeSymbol { get; set; }

        [DisplayName("Người lớn")]
        public int NumberOfAdults { get; set; }
        [DisplayName("Trẻ em")]
        public int NumberOfChildrent { get; set; }

        [DisplayName("Ghi chú")]
        public string? Description { get; set; }
    }
}
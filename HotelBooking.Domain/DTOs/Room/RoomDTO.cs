namespace HotelBooking.Domain.DTOs.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public string? RoomNumber { get; set; }
        public int FloorId { get; set; }
        public string? FloorName { get; set; }
        public int RoomTypeId { get; set; }
        public string? RoomTypeName { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildrent { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateRoomDTO
    {
        public string RoomNumber { get; set; }
        public int FloorId { get; set; }
        public int RoomTypeId { get; set; }
        public bool IsActive { get; set; }
        public string? Description { get; set; }
    }

    public class UpdateRoomDTO
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int FloorId { get; set; }
        public int RoomTypeId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
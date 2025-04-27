namespace HotelBooking.Domain.DTOs.Floor
{
    public class FloorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderFloor { get; set; }
        public int RoomCount { get; set; }
        public string RoomSymbol { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }

    public class CreateFloorDTO
    {
        public string Name { get; set; }
        public int OrderFloor { get; set; }
        public int RoomCount { get; set; }
        public int DefauldRoomRypeId { get; set; }
        public string? Description { get; set; }
        public string? RoomSymbol { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
    }

    public class UpdateFloorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OrderFloor { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
    }
}
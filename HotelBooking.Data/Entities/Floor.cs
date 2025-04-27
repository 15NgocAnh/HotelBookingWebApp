using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Entities
{
    public class Floor : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OrderFloor { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int RoomCount { get; set; }
        public string? RoomSymbol { get; set; }
        public int DefauldRoomRypeId { get; set; }
        public int BranchId { get; set; }
        public virtual RoomType DefauldRoomRype { get; set; }
        public virtual BranchModel Branch { get; set; }
        // Navigation properties
        public virtual ICollection<Room>? Rooms { get; set; }
    }
} 
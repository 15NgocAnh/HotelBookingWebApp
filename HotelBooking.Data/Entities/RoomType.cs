using HotelBooking.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.Entities
{
    public class RoomType : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildrent { get; set; }
        public string? RoomTypeSymbol { get; set; }
        public decimal? BasePrice { get; set; }
        
        // Navigation properties
        public virtual ICollection<Room>? Rooms { get; set; }
    }
} 
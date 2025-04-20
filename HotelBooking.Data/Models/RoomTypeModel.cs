using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a room type in the hotel.
    /// </summary>
    public class RoomTypeModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room type.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the room type.
        /// </summary>
        [Required(ErrorMessage = "Room type name is required.")]
        [StringLength(100, ErrorMessage = "Room type name cannot exceed 100 characters.")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the current price for this room type.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Room price must be a positive value.")]
        public decimal RoomPrice { get; set; }

        /// <summary>
        /// Gets or sets the default price for this room type.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Default room price must be a positive value.")]
        public decimal DefaultRoomPrice { get; set; }

        /// <summary>
        /// Gets or sets the URL of the room type's image.
        /// </summary>
        [Url(ErrorMessage = "Invalid URL format for room image.")]
        public string? RoomImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the description of the room type.
        /// </summary>
        [StringLength(500, ErrorMessage = "Room description cannot exceed 500 characters.")]
        public string? RoomDescription { get; set; }

        /// <summary>
        /// Gets or sets the collection of rooms of this type.
        /// </summary>
        public ICollection<RoomModel>? Rooms { get; set; } = new List<RoomModel>();
    }
}

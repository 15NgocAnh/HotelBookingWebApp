using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents the status of a room.
    /// </summary>
    public enum RoomStatus
    {
        /// <summary>
        /// Room is available for booking
        /// </summary>
        Available,

        /// <summary>
        /// Room is currently booked
        /// </summary>
        Booked,

        /// <summary>
        /// Room is under maintenance
        /// </summary>
        Maintenance
    }

    /// <summary>
    /// Represents the available facilities for a room.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Facility
    {
        /// <summary>
        /// WiFi internet access
        /// </summary>
        WiFi,

        /// <summary>
        /// Television
        /// </summary>
        TV,

        /// <summary>
        /// Air conditioning
        /// </summary>
        AirConditioning,

        /// <summary>
        /// Bathtub
        /// </summary>
        Bathtub,

        /// <summary>
        /// Mini bar
        /// </summary>
        MiniBar,

        /// <summary>
        /// Safe deposit box
        /// </summary>
        Safe,

        /// <summary>
        /// Breakfast included
        /// </summary>
        BreakfastIncluded
    }

    /// <summary>
    /// Represents a room in the hotel.
    /// </summary>
    public class RoomModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the room.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the room number.
        /// </summary>
        [Required(ErrorMessage = "Room number cannot be empty.")]
        [StringLength(10, ErrorMessage = "Room number cannot exceed 10 characters.")]
        public required string RoomNumber { get; set; }

        /// <summary>
        /// Gets or sets the ID of the room type.
        /// </summary>
        [Required]
        [ForeignKey("RoomType")]
        public int RoomTypeId { get; set; }

        /// <summary>
        /// Gets or sets the number of beds in the room.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Bed count must be greater than 0")]
        public int BedCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of occupants allowed in the room.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Maximum occupancy must be greater than 0")]
        public int MaxOccupancy { get; set; }

        /// <summary>
        /// Gets or sets the price per night for the room.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Gets or sets the current status of the room.
        /// </summary>
        [Required]
        public RoomStatus Status { get; set; } = RoomStatus.Available;

        /// <summary>
        /// Gets or sets the floor number where the room is located.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Floor number must be greater than 0")]
        public int FloorNumber { get; set; }

        [Required]
        [Range(10, 500, ErrorMessage = "Area must be valid (minimum 10m²)")]
        public int Area { get; set; } // Area (m²)

        /// <summary>
        /// Gets or sets the description of the room.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the list of facilities available in the room.
        /// </summary>
        [Required(ErrorMessage = "Facilities cannot be empty.")]
        public List<Facility> Facilities { get; set; } = new();

        /// <summary>
        /// Gets or sets the URL of the room's image.
        /// </summary>
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the room type associated with this room.
        /// </summary>
        public virtual RoomTypeModel RoomType { get; set; }

        /// <summary>
        /// Gets or sets the date when the room was last booked.
        /// </summary>
        public DateTime? LastBookedDate { get; set; }
    }
}

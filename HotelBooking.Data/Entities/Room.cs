using HotelBooking.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Domain.Entities
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
        Occupied,

        Reserved,

        OutOfOrder,

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

    public class Room : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int FloorId { get; set; }
        public int RoomTypeId { get; set; }
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        
        // Navigation properties
        public virtual Floor Floor { get; set; }
        public virtual RoomType RoomType { get; set; }

        [JsonIgnore]
        public virtual ICollection<BookingRoom> BookingRooms { get; set; } = [];
    }
} 
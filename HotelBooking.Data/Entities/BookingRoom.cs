using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents the relationship between users and roles in the system.
    /// </summary>
    public class BookingRoom : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the user associated with this role.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("BookingId")]
        public virtual BookingModel Booking { get; set; }

        /// <summary>
        /// Gets or sets the role associated with this user.
        /// </summary>
        [JsonIgnore]
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }
    }
}

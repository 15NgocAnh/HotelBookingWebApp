using HotelBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents the status of a booking.
    /// </summary>
    public enum BookingStatus
    {
        /// <summary>
        /// Booking is pending confirmation
        /// </summary>
        Pending,

        /// <summary>
        /// Booking has been confirmed
        /// </summary>
        Confirmed,

        /// <summary>
        /// Booking has been cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// Guest has checked in
        /// </summary>
        CheckedIn,

        /// <summary>
        /// Guest has checked out
        /// </summary>
        CheckedOut
    }

    public enum BookingType
    {
        DayAndNight,
        Night,
        Hour,
        Month
    }

    /// <summary>
    /// Represents a booking in the system.
    /// </summary>
    public class BookingModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the booking.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the guest who made the booking.
        /// </summary>
        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        /// <summary>
        /// Gets or sets the expected arrival date.
        /// </summary>
        public DateTime EstCheckinTime { get; set; }

        /// <summary>
        /// Gets or sets the expected departure date.
        /// </summary>
        public DateTime EstCheckoutTime { get; set; }

        /// <summary>
        /// Gets or sets the number of adults in the booking.
        /// </summary>
        public int NumberOfAdults { get; set; }

        /// <summary>
        /// Gets or sets the number of children in the booking.
        /// </summary>
        public int NumberOfChildren { get; set; }

        /// <summary>
        /// Gets or sets the booking type (Loại hình).
        /// </summary>
        public BookingType BookingType { get; set; }

        /// <summary>
        /// Gets or sets the note (Ghi chú).
        /// </summary>
        public string? Note { get; set; }

        /// <summary>
        /// Gets or sets the actual check-in time.
        /// </summary>
        public DateTime? ActualCheckInTime { get; set; }

        /// <summary>
        /// Gets or sets the actual check-out time.
        /// </summary>
        public DateTime? ActualCheckOutTime { get; set; }

        /// <summary>
        /// Gets or sets the guest who made the booking.
        /// </summary> 
        [JsonIgnore]
        public virtual GuestModel Guest { get; set; } = new();

        /// <summary>
        /// Gets or sets the room being booked.
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<BookingRoom> BookingRooms { get; set; } = [];

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }
}

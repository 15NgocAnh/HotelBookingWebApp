using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Gets or sets the ID of the room being booked.
        /// </summary>
        [ForeignKey("Room")]
        public int RoomId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the booking was made.
        /// </summary>
        public DateTime BookingDateTime { get; set; }

        /// <summary>
        /// Gets or sets the expected arrival date.
        /// </summary>
        public DateTime ArrivalDate { get; set; }

        /// <summary>
        /// Gets or sets the expected departure date.
        /// </summary>
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Gets or sets the estimated arrival time.
        /// </summary>
        public string? EstimatedArrivalTime { get; set; }

        /// <summary>
        /// Gets or sets the estimated departure time.
        /// </summary>
        public string? EstimatedDepartureTime { get; set; }

        /// <summary>
        /// Gets or sets the number of adults in the booking.
        /// </summary>
        public int NumberOfAdults { get; set; }

        /// <summary>
        /// Gets or sets the number of children in the booking.
        /// </summary>
        public int NumberOfChildren { get; set; }

        /// <summary>
        /// Gets or sets any special requirements for the booking.
        /// </summary>
        public string? SpecialRequirements { get; set; }

        /// <summary>
        /// Gets or sets the guest who made the booking.
        /// </summary>
        public virtual UserModel Guest { get; set; }

        /// <summary>
        /// Gets or sets the room being booked.
        /// </summary>
        public virtual RoomModel Room { get; set; }

        /// <summary>
        /// Gets or sets the current status of the booking.
        /// </summary>
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
    }
}

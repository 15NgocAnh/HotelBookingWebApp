using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a payment method for a bill.
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Cash payment
        /// </summary>
        Cash,

        /// <summary>
        /// Credit card payment
        /// </summary>
        CreditCard,

        /// <summary>
        /// Online payment
        /// </summary>
        OnlinePayment
    }

    /// <summary>
    /// Represents a bill in the system.
    /// </summary>
    public class BillModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bill.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated booking.
        /// </summary>
        [ForeignKey("Booking")]
        public int BookingId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the guest.
        /// </summary>
        [ForeignKey("Guest")]
        public int GuestId { get; set; }

        /// <summary>
        /// Gets or sets the room charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Room charge must be a positive value.")]
        public decimal RoomCharge { get; set; }

        /// <summary>
        /// Gets or sets the room service charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Room service charge must be a positive value.")]
        public decimal RoomServiceCharge { get; set; }

        /// <summary>
        /// Gets or sets the restaurant charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Restaurant charge must be a positive value.")]
        public decimal RestaurantCharge { get; set; }

        /// <summary>
        /// Gets or sets the bar charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Bar charge must be a positive value.")]
        public decimal BarCharge { get; set; }

        /// <summary>
        /// Gets or sets any miscellaneous charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Miscellaneous charge must be a positive value.")]
        public decimal MiscellaneousCharge { get; set; }

        /// <summary>
        /// Gets or sets the late checkout charges.
        /// </summary>
        [Range(0, double.MaxValue, ErrorMessage = "Late checkout charge must be a positive value.")]
        public decimal LateCheckoutCharge { get; set; }

        /// <summary>
        /// Gets or sets the payment date.
        /// </summary>
        [Required]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the associated booking.
        /// </summary>
        public virtual BookingModel Booking { get; set; }

        /// <summary>
        /// Gets or sets the guest who made the payment.
        /// </summary>
        public virtual UserModel Guest { get; set; }

        /// <summary>
        /// Gets the total amount of the bill.
        /// </summary>
        public decimal TotalAmount => RoomCharge + RoomServiceCharge + RestaurantCharge + 
                                    BarCharge + MiscellaneousCharge + LateCheckoutCharge;
    }
}

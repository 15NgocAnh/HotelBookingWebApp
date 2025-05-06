using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents a guest in the system.
    /// </summary>
    public class GuestModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the guest.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the guest's full name.
        /// </summary>
        [StringLength(100)]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the guest's identity number (CMT/Hộ chiếu).
        /// </summary>
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Gets or sets the guest's identity issue date.
        /// </summary>
        public DateTime? IdentityIssueDate { get; set; }

        /// <summary>
        /// Gets or sets the guest's identity issue place.
        /// </summary>
        [StringLength(100)]
        public string? IdentityIssuePlace { get; set; }

        /// <summary>
        /// Gets or sets the guest's address.
        /// </summary>
        [StringLength(200)]
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the guest's nationality.
        /// </summary>
        [StringLength(50)]
        public string? Nationality { get; set; }

        /// <summary>
        /// Gets or sets the guest's gender.
        /// </summary>
        public Gender? Gender { get; set; }

        /// <summary>
        /// Gets or sets the guest's birth date.
        /// </summary>
        public DateTime? BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the guest's province.
        /// </summary>
        [StringLength(50)]
        public string? Province { get; set; }

        /// <summary>
        /// Gets or sets the guest's phone number.
        /// </summary>
        [StringLength(20)]
        public string? Phone { get; set; }

        /// <summary>
        /// Gets or sets the guest's email.
        /// </summary>
        [StringLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings associated with this guest.
        /// </summary>
        public virtual ICollection<BookingModel>? Bookings { get; set; }
    }
} 
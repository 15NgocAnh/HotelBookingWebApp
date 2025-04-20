using HotelBooking.Data.Utils.Variable;
using HotelBooking.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    /// <summary>
    /// Represents the gender of a user.
    /// </summary>
    public enum Gender
    {
        /// <summary>
        /// Male gender
        /// </summary>
        Male,

        /// <summary>
        /// Female gender
        /// </summary>
        Female,

        /// <summary>
        /// Other gender
        /// </summary>
        Other
    }

    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class UserModel : BaseModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the user's last name.
        /// </summary>
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the user's date of birth.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [AgeRequirementAttribure(15)]
        [JsonPropertyName("dob")]
        public DateOnly DOB { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [RegularExpression(RegexUtils.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the hashed password for the user.
        /// </summary>
        [JsonIgnore]
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string PasswordHash { get; set; }

        [JsonIgnore]
        [Required]
        public bool IsVerified { get; set; } = false;

        /// <summary>
        /// Gets or sets the user's passport number.
        /// </summary>
        public string? PassportNo { get; set; }

        /// <summary>
        /// Gets or sets the user's gender.
        /// </summary>
        [Required]
        public Gender Gender { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [Required]
        [RegularExpression(RegexUtils.PHONE_NUMBER, ErrorMessage = "Phone number format is not Valid!")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's address.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the user's postcode.
        /// </summary>
        public string? Postcode { get; set; }

        /// <summary>
        /// Gets or sets the user's city.
        /// </summary>
        public string? City { get; set; }

        /// <summary>
        /// Gets or sets the user's country.
        /// </summary>
        public string? Country { get; set; }

        /// <summary>
        /// Gets or sets the user's profile image URL.
        /// </summary>
        [Required]
        public string ProfileImage { get; set; }

        /// <summary>
        /// Gets or sets the user's biography.
        /// </summary>
        [Column(TypeName = "text")]
        public string? Bio { get; set; }

        /// <summary>
        /// Gets or sets the collection of user roles.
        /// </summary>
        [JsonIgnore]
        public ICollection<UserRoleModel> UserRoles { get; set; }

        /// <summary>
        /// Gets or sets the collection of JWT tokens associated with the user.
        /// </summary>
        [JsonIgnore]
        public ICollection<JWTModel> Jwts { get; set; }

        /// <summary>
        /// Gets or sets the collection of posts created by the user.
        /// </summary>
        [JsonIgnore]
        public ICollection<PostModel> Posts { get; set; }

        /// <summary>
        /// Gets or sets the collection of notifications sent by the user.
        /// </summary>
        [JsonIgnore]
        public ICollection<NotificationModel> FromUserNotifications { get; set; }

        /// <summary>
        /// Gets or sets the collection of bookings made by the user.
        /// </summary>
        public ICollection<BookingModel> Bookings { get; set; }

        /// <summary>
        /// Gets or sets the collection of bills associated with the user.
        /// </summary>
        public ICollection<BillModel> Bills { get; set; }
    }
}

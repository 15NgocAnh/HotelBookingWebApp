using HotelBooking.Data.Utils.Variable;
using HotelBooking.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HotelBooking.Data.Models
{
    public enum Gender
    {
        male,
        female,
        other
    }
    public class UserModel : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [AgeRequirementAttribure(15)]
        public DateOnly DOB { get; set; }

        [Required]
        [RegularExpression(RegexUtils.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public string Email { get; set; }

        [JsonIgnore]
        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must contain atleast 8 character")]
        public string PasswordHash { get; set; }

        public string? PassportNo { get; set; }

        [JsonIgnore]
        [Required]
        public bool IsVerified { get; set; } = false;

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }

        [Required]
        public string Address { get; set; }

        public string? Postcode { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        [Required]
        [RegularExpression(RegexUtils.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        public string PhoneNo { get; set; }

        [Column(TypeName = "text")]
        public string? Avatar { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserRoleModel> user_roles { get; set; }
        [JsonIgnore]
        public virtual ICollection<JWTModel> jwts { get; set; }
        [JsonIgnore]
        public virtual ICollection<PostModel> posts { get; set; }
        [JsonIgnore]
        public virtual ICollection<NotificationModel> from_user_notifications { get; set; }
        public ICollection<BookingModel> Bookings { get; set; }
        public ICollection<BillModel> Bills { get; set; }
    }
}

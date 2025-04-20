using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Authentication;
using HotelBooking.Data;
using HotelBooking.Data.Utils.Variable;
using HotelBooking.Data.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.DTOs.User
{
    public class UserRegisterDTO : PasswordDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        public string first_name { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")]
        public string last_name { get; set; }

        [Required]
        [RegularExpression(RegexUtils.EMAIL, ErrorMessage = "Email format is not Valid!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "Email length must between 5 and 70 character")]
        public string email { get; set; }

        [Required]
        [RegularExpression(RegexUtils.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        public string phone_number { get; set; }
        [Required(ErrorMessage = "Gender is required")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [AgeRequirementAttribure(CJConstant.MIN_AGE)]
        public DateOnly dob { get; set; }
        [Required]
        public string address { get; set; }

        [Column(TypeName = "text")]
        public string? avatar { get; set; }
    }
}
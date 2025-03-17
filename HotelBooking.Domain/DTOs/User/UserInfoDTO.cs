using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Data.Utils.Variable;
using HotelBooking.Data.Validation;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.User
{
    public class UserInfoDTO
    {
        [StringLength(50, MinimumLength = 1, ErrorMessage = "First Name must contain at least 1 character and maximum to 50 character")]
        public string? first_name { get; set; }
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Last Name must contain at least 1 character and maximum to 10 character")] 
        public string? last_name { get; set; }
        [RegularExpression(RegexUtils.PHONE_NUMBER, ErrorMessage = "Phone number is not valid format!")]
        [StringLength(11, MinimumLength = 9)]
        public string? phone_number { get; set; }
        public Gender? gender { get; set; }
        [DataType(DataType.Date)]
        public DateOnly dob { get; set; }
        public string? address { get; set; }
        public string? avatar { get; set; }
    }
}
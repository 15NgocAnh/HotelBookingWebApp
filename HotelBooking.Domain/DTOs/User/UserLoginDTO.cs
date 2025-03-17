using HotelBooking.Domain.DTOs.Authentication;
using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.User
{
    public class UserLoginDTO : PasswordDTO
    {
        [Required]
        public string email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.CQRS.User.DTOs
{
    public class ChangePasswordDto
    {
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}
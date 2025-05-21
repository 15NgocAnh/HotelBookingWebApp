using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Application.CQRS.User.DTOs
{
    public class UpdateProfileDto
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
    }
}
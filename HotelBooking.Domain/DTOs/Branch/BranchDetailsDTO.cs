using System.ComponentModel.DataAnnotations;

namespace HotelBooking.Domain.DTOs.Branch
{
    public class BranchDetailsDTO
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Tên cơ sở là bắt buộc")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        public string? Website { get; set; }

        public string? Note { get; set; }
    }
} 
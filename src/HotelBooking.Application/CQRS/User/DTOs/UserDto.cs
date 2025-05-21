using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Role.DTOs;

namespace HotelBooking.Application.CQRS.User.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public RoleDto Role { get; set; } = new();
        public List<HotelDto> Hotels { get; set; } = [];
        public bool IsActive { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
} 
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.DTOs.Room;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.Domain.DTOs.User
{
    public class UserDetailsDTO
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public Gender gender { get; set; }
        [DataType(DataType.Date)]
        public DateOnly dob { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }

        [Column(TypeName = "text")]
        public string? avatar { get; set; }
        public ICollection<RoleDto> roles { get; set; }
        public ICollection<RoomDTO> posts { get; set; }
        public int followers { get; set; }
        public int following { get; set; }
    }
}

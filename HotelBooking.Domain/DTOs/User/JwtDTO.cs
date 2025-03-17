using HotelBooking.Data.Models;

namespace HotelBooking.Domain.DTOs.User
{
    public class JwtDTO
    {
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public UserModel user { get; set; }
    }
}

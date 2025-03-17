
using HotelBooking.Domain.DTOs.User;

namespace HotelBooking.Domain.DTOs.Authentication
{
    public class CredentialDTO : UserDTO
    {
        public string Token {  get; set; }
        public string RefreshToken { get; set; }
    }
}

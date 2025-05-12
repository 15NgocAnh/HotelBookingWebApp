using HotelBooking.Application.Common.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace HotelBooking.Application.Common.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return BC.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return BC.Verify(password, hash);
        }
    }
} 
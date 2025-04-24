using HotelBooking.Domain.DTOs.User;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IJwtServices
    {
        Task InsertJWTToken(JwtDTO jwt);
        public Task<bool> IsJWTValid(string token);
        public Task InvalidateToken(string token);
    }
}
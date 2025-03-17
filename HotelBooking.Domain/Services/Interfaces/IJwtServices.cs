using HotelBooking.Domain.DTOs.User;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IJwtServices
    {
        Task InsertJWTToken(JwtDTO jwt);
        public Task<bool> IsJWTValid(string token);
        public Task InvalidateToken(string token);
    }
}
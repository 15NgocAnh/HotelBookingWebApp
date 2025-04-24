using HotelBooking.Data;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IJwtRepository : IGenericRepository<JWTModel>
    {
        public Task<JWTModel?> FindByValue(string token);
        public Task<bool> InvalidToken(string token);
    }
}
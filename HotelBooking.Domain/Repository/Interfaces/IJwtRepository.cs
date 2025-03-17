using HotelBooking.Data;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IJwtRepository : IGenericRepository<JWTModel>
    {
        public Task<JWTModel?> FindByValue(string token);
        public Task<bool> InvalidToken(string token);
    }
}
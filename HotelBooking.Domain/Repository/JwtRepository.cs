using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data.Models;
namespace HotelBooking.Domain.Repository
{
    public class JwtRepository : GenericRepository<JWTModel>, IJwtRepository
    {
        public JwtRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<JWTModel?> FindByValue(string token)
        {
            return await _context.jwts.FirstOrDefaultAsync(x => x.token_hash_value == token && !x.is_deleted);
        }

        public async Task<bool> InvalidToken(string token)
        {
            try
            {
                var jwt = await _context.jwts.FirstOrDefaultAsync(x => x.token_hash_value == token && x.is_deleted == false);
                jwt.is_deleted = true;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
            return true;
        }
    }
}
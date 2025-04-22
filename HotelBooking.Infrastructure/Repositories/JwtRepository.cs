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
            return await _context.Jwts.FirstOrDefaultAsync(x => x.TokenHashValue == token && !x.IsDeleted);
        }

        public async Task<bool> InvalidToken(string token)
        {
            try
            {
                var jwt = await _context.Jwts.FirstOrDefaultAsync(x => x.TokenHashValue == token && x.IsDeleted == false);
                jwt.IsDeleted = true;
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
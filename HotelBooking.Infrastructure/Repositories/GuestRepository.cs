using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class GuestRepository : GenericRepository<GuestModel>, IGuestRepository
    {
        public GuestRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<GuestModel> GetByIdentityNumberAsync(string identityNumber)
        {
            return await _context.Guests
                .FirstOrDefaultAsync(g => g.IdentityNumber == identityNumber);
        }
    }
} 
using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Domain.Repository
{
    public class BookingRepository : GenericRepository<BookingModel>, IBookingRepository
    {
        public BookingRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<IEnumerable<BookingModel>> GetAllAsync()
        {
            return await _context.Set<BookingModel>()
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }

        public async Task<BookingModel> GetByIdAsync(int id)
        {
            return await _context.Set<BookingModel>()
                .Include(b => b.Room)
                .Include(b => b.Guest)
                .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
        }
    }
}

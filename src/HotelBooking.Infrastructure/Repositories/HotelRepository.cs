using AutoMapper;
using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class HotelRepository : GenericRepository<Hotel>, IHotelRepository
{
    public HotelRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) 
        : base(context, mapper, unitOfWork)
    {
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.Hotels
            .AnyAsync(h => h.Name.ToLower() == name.ToLower());
    }

    public async Task<bool> HasBuildingsAsync(int hotelId)
    {
        return await _context.Buildings
            .AnyAsync(b => b.HotelId == hotelId);
    }

    public async Task<int> CountBuildingsAsync(int hotelId)
    {
        return await _context.Buildings
            .CountAsync(b => b.HotelId == hotelId);
    }

    public async Task DeleteAsync(int id)
    {
        var hotel = await GetByIdAsync(id);
        if (hotel != null)
        {
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Hotel?> GetByNameAsync(string name)
    {
        return await _context.Set<Hotel>()
            .FirstOrDefaultAsync(h => h.Name == name && !h.IsDeleted);
    }
}

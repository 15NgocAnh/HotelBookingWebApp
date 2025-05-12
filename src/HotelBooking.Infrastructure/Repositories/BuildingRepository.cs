using AutoMapper;
using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class BuildingRepository : GenericRepository<Building>, IBuildingRepository
{
    private readonly AppDbContext _context;

    public BuildingRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) 
        : base(context, mapper, unitOfWork)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public override async Task<IEnumerable<Building>> GetAllAsync()
    {
        return await _context.Set<Building>()
            .Include(b => b.Floors)
            .Where(b => !b.IsDeleted)
            .AsNoTracking()
            .ToListAsync();
    }

    public override async Task<Building?> GetByIdAsync(int id)
    {
        return await _context.Set<Building>()
            .Include(b => b.Floors)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task<IEnumerable<Building>> GetBuildingsByHotelAsync(int hotelId)
    {
        return await _context.Set<Building>()
            .Include(b => b.Floors)
            .Where(b => b.HotelId == hotelId && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> IsNameUniqueInHotelAsync(int hotelId, string name)
    {
        return !await _context.Set<Building>()
            .AnyAsync(b => b.HotelId == hotelId && b.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase));
    }

    public async Task DeleteAsync(int id)
    {
        var building = await GetByIdAsync(id);
        if (building != null)
        {
            _context.Buildings.Remove(building);
            await _context.SaveChangesAsync();
        }
    }
}

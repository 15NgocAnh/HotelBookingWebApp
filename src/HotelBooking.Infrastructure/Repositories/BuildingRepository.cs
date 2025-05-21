using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Domain.AggregateModels.BuildingAggregate;

namespace HotelBooking.Infrastructure.Repositories;

public class BuildingRepository : GenericRepository<Building>, IBuildingRepository
{
    public BuildingRepository(AppDbContext context, IUnitOfWork unitOfWork) 
        : base(context, unitOfWork)
    {
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
            .AnyAsync(b => b.HotelId == hotelId && b.Name == name);
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

    public async Task<IEnumerable<Floor>> GetAllFloorsByBuildingIdAsync(int buildingId)
    {
        return await _context.Buildings
                .SelectMany(b => b.Floors.Select(f => new Floor(f.Number, f.Name)))
                .ToListAsync();
    }
}

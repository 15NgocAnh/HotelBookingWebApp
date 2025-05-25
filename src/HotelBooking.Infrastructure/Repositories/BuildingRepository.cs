using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using Microsoft.Data.SqlClient;

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
                .Where(b => b.Id == buildingId)
                .SelectMany(b => b.Floors
                    .Where(f => !f.IsDeleted)
                    .Select(f => new Floor(f.Id, f.Number, f.Name)))
                .ToListAsync();
    }

    public async Task<Floor> GetFloorByIdAsync(int floorId)
    {
        var building = await _context.Buildings
            .Include(b => b.Floors) 
            .FirstOrDefaultAsync(b => b.Floors.Any(f => f.Id == floorId));

        return building?.Floors.FirstOrDefault(f => f.Id == floorId);
    }
}

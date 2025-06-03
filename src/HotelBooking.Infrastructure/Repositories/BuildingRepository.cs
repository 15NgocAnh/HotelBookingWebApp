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
        return await _context.Buildings
            .Include(b => b.Floors)
            .FirstOrDefaultAsync(b => b.Id == id);
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

    public async Task<Floor?> GetFloorByIdAsync(int floorId)
    {
        var building = await _context.Buildings
        .Include(b => b.Floors)
        .FirstOrDefaultAsync(b => b.Floors.Any(f => f.Id == floorId));

        return building?.Floors.FirstOrDefault(f => f.Id == floorId);
    }

    public async Task<List<int>> GetBuildingIdsByHotelIdsAsync(List<int> hotelIds)
    {
        return await _context.Buildings
            .Where(b => hotelIds.Contains(b.HotelId))
            .Select(b => b.Id)
            .ToListAsync();
    }

    public async Task<List<int>> GetFloorIdsByBuildingIdsAsync(List<int> buildingIds)
    {
        return await _context.Buildings
            .Include(b => b.Floors)
            .Where(b => buildingIds.Contains(b.Id))
            .SelectMany(b => b.Floors) 
            .Select(f => f.Id)         
            .ToListAsync();           
    }

    public async Task<Building> GetBuildingByFloorIdAsync(int floorId)
    {
        return await _context.Buildings
        .Include(b => b.Floors)
        .FirstOrDefaultAsync(b => b.Floors.Any(f => f.Id == floorId));
    }

    public async Task<IEnumerable<Floor>> GetAllFloorsByBuildingIdAsync(int buildingId)
    {
        return await _context.Buildings
            .Include(b => b.Floors)
            .Where(b => b.Id == buildingId)
            .SelectMany(b => b.Floors)
            .ToListAsync();
    }
}

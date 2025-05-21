using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

namespace HotelBooking.Infrastructure.Repositories;

public class RoomTypeRepository : GenericRepository<RoomType>, IRoomTypeRepository
{
    public RoomTypeRepository(AppDbContext context, IUnitOfWork unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public override async Task<IEnumerable<RoomType>> GetAllAsync()
    {
        return await _context.RoomTypes
            .Include(rt => rt.BedTypeSetupDetails)
            .Include(rt => rt.AmenitySetupDetails)
            .Where(rt => !rt.IsDeleted)
            .ToListAsync();
    }

    public override async Task<RoomType?> GetByIdAsync(int id)
    {
        return await _context.RoomTypes
            .Include(rt => rt.BedTypeSetupDetails)
            .Include(rt => rt.AmenitySetupDetails)
            .FirstOrDefaultAsync(rt => rt.Id == id && !rt.IsDeleted);
    }

    
    public async Task<RoomType?> GetByNameAsync(string name)
    {
        return await _context.RoomTypes
            .Include(rt => rt.BedTypeSetupDetails)
            .Include(rt => rt.AmenitySetupDetails)
            .FirstOrDefaultAsync(rt => rt.Name == name && !rt.IsDeleted);
    }

    public async Task<bool> IsNameUniqueAsync(string name)
    {
        return !await _context.RoomTypes
            .AnyAsync(rt => rt.Name.ToLower() == name.ToLower());
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesByAmenityAsync(int amenityId)
    {
        return await _context.RoomTypes
            .Include(rt => rt.BedTypeSetupDetails)
            .Include(rt => rt.AmenitySetupDetails)
            .Where(rt => rt.AmenitySetupDetails.Any(a => a.AmenityId == amenityId) && !rt.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoomType>> GetRoomTypesByBedTypeAsync(int bedTypeId)
    {
        return await _context.RoomTypes
            .Include(rt => rt.BedTypeSetupDetails)
            .Include(rt => rt.AmenitySetupDetails)
            .Where(rt => rt.BedTypeSetupDetails.Any(b => b.BedTypeId == bedTypeId) && !rt.IsDeleted)
            .ToListAsync();
    }

    public async Task<bool> HasRoomsAsync(int roomTypeId)
    {
        return await _context.Rooms
            .AnyAsync(r => r.RoomTypeId == roomTypeId);
    }

    public async Task<int> CountRoomsAsync(int roomTypeId)
    {
        return await _context.Rooms
            .CountAsync(r => r.RoomTypeId == roomTypeId);
    }

    public async Task DeleteAsync(int id)
    {
        var roomType = await GetByIdAsync(id);
        if (roomType != null)
        {
            _context.Set<RoomType>().Remove(roomType);
            await _context.SaveChangesAsync();
        }
    }
}

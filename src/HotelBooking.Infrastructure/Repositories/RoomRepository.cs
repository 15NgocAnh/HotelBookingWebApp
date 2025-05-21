using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Infrastructure.Repositories;

public class RoomRepository : GenericRepository<Room>, IRoomRepository
{
    public RoomRepository(AppDbContext context, IUnitOfWork unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public async Task<Room?> GetByRoomNumberAsync(string roomNumber)
    {
        return await _context.Rooms.FirstOrDefaultAsync(x => x.Name  == roomNumber);
    }

    public async Task<bool> IsRoomNumberUniqueInBuildingAsync(int floorId, string roomNumber)
    {
        var building = await _context.Buildings.FirstOrDefaultAsync(x => x.Floors.Any(x => x.Id  == floorId));

        if (building == null) { return  false; }

        return !await _context.Rooms
            .AnyAsync(r => building.Floors.Any(f => f.Id == r.FloorId) && 
                          r.Name == roomNumber);
    }

    public async Task<bool> HasActiveBookingsAsync(int roomId)
    {
        return await _context.Bookings
            .AnyAsync(b => b.RoomId == roomId && 
                          b.Status != BookingStatus.Cancelled);
    }

    public async Task DeleteAsync(int id)
    {
        var room = await GetByIdAsync(id);
        if (room != null)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Room>> GetRoomsByFloorIdAsync(int floorId)
    {
        return await _context.Rooms.Where(r => r.FloorId == floorId).ToListAsync();
    }
}

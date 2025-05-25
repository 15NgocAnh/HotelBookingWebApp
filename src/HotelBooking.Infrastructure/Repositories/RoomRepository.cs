using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.AggregateModels.RoomAggregate;
using Microsoft.Data.SqlClient;
using static HotelBooking.Domain.Interfaces.Repositories.IRoomRepository;

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
        var building = await _context.Buildings
                    .Where(b => b.Floors.Any(f => f.Id == floorId))
                    .Select(b => new { b.Id })
                    .FirstOrDefaultAsync();

        if (building == null)
            return false;

        var floorIds = await _context.Buildings
            .Where(b => b.Id == building.Id)
            .SelectMany(b => b.Floors.Select(f => f.Id))
            .ToListAsync();

        return !await _context.Rooms
            .AnyAsync(r => floorIds.Contains(r.FloorId) && r.Name == roomNumber);
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

    public async Task<List<RoomWithTypeName>> GetRoomWithTypeNamesAsync(CancellationToken cancellationToken)
    {
        var query = @"
        SELECT 
            r.Id AS RoomId,
            r.RoomTypeId,
            rt.Name AS RoomTypeName,
            r.Status
        FROM 
            Room r
        INNER JOIN 
            RoomType rt ON r.RoomTypeId = rt.Id";

        return await _context.Database
            .SqlQueryRaw<RoomWithTypeName>(query)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Room>> GetRoomsByBuildingAsync(int buildingId)
    {
        var sql = @"
        SELECT r.*
        FROM [Room] AS r
        INNER JOIN [Floor] AS f ON r.[FloorId] = f.[Id]
        INNER JOIN [Building] AS b ON f.[BuildingId] = b.[Id]
        WHERE b.[Id] = @buildingId";

        return await _context.Rooms
            .FromSqlRaw(sql, new SqlParameter("@buildingId", buildingId))
            .ToListAsync();
    }

    public async Task<List<Room>> GetAllRoomsAvailableAsync()
    {
        return await _context.Rooms.Where(r => !r.IsDeleted && r.Status == RoomStatus.Available).ToListAsync();
    }

    public async Task<IEnumerable<RoomTypeStatistics>> GetRoomTypeStatisticsAsync()
    {
        var query = @"
            SELECT 
                rt.Name AS RoomType,
                COUNT(*) AS Total,
                SUM(CASE WHEN r.Status = 'Available' THEN 1 ELSE 0 END) AS Available,
                SUM(CASE WHEN r.Status = 'Booked' THEN 1 ELSE 0 END) AS Booked,
                SUM(CASE WHEN r.Status = 'CleaningUp' THEN 1 ELSE 0 END) AS CleaningUp,
                SUM(CASE WHEN r.Status = 'UnderMaintenance' THEN 1 ELSE 0 END) AS UnderMaintenance
            FROM 
                Room r
            INNER JOIN 
                RoomType rt ON r.RoomTypeId = rt.Id
            GROUP BY 
                rt.Name";

        return await _context.Database
            .SqlQueryRaw<RoomTypeStatistics>(query)
            .ToListAsync();
    }

    public async Task<IEnumerable<RoomStatusStatistic>> GetRoomStatusStatisticsAsync()
    {
        return await _context.Rooms
            .GroupBy(r => r.Status)
            .Select(g => new RoomStatusStatistic(g.Key.ToString(), g.Count()))
            .ToListAsync();
    }
}

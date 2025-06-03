using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using static HotelBooking.Domain.Interfaces.Repositories.IBookingRepository;

namespace HotelBooking.Infrastructure.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    public BookingRepository(AppDbContext context, IUnitOfWork unitOfWork) 
        : base(context, unitOfWork)
    {
    }

    public override async Task<IEnumerable<Booking>> GetAllAsync()
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Include(b => b.ExtraUsages)
            .Where(b => !b.IsDeleted)
            .ToListAsync();
    }

    public override async Task<Booking?> GetByIdAsync(int id)
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Include(b => b.ExtraUsages)
            .Where(b => b.Id == id && !b.IsDeleted)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Booking>> GetBookingsByCustomerIdAsync(string citizenIdNumber)
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Where(b => b.Guests.Any(g => g.CitizenIdNumber == citizenIdNumber))
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId)
    {
        return await _context.Bookings
            .Where(b => b.RoomId == roomId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Where(b => b.CheckInTime >= startDate && b.CheckOutTime <= endDate)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetActiveBookingsAsync()
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Where(b => b.Status == BookingStatus.Confirmed || b.Status == BookingStatus.CheckedIn)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetPendingCheckoutsAsync(DateTime date)
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Where(b => b.Status == BookingStatus.CheckedIn && b.CheckOutTime == date)
            .OrderBy(b => b.CheckOutTime)
            .ToListAsync();
    }

    public async Task<List<Booking>> GetPendingCheckinsAsync(DateTime date)
    {
        return await _context.Bookings
            .Include(b => b.Guests)
            .Where(b => b.Status == BookingStatus.Confirmed && b.CheckInTime == date)
            .OrderBy(b => b.CheckInTime)
            .ToListAsync();
    }

    public async Task<bool> IsRoomAvailableForBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int excludeBookingId = 0)
    {
        return !await _context.Bookings
            .AnyAsync(b => b.RoomId == roomId &&
                (excludeBookingId == 0 || b.Id != excludeBookingId) &&
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Completed &&
                ((checkInDate >= b.CheckInTime && checkInDate < b.CheckOutTime) ||
                 (checkOutDate > b.CheckInTime && checkOutDate <= b.CheckOutTime) ||
                 (checkInDate <= b.CheckInTime && checkOutDate >= b.CheckOutTime)));
    }

    public async Task<int> GetTotalBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Bookings.AsQueryable();
        if (startDate.HasValue)
            query = query.Where(b => b.CheckInDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(b => b.CheckInDate <= endDate.Value);
        return await query.CountAsync();
    }

    public async Task<int> GetCompletedBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Bookings.Where(b => b.Status == BookingStatus.Completed);
        if (startDate.HasValue)
            query = query.Where(b => b.CheckInDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(b => b.CheckInDate <= endDate.Value);
        return await query.CountAsync();
    }

    public async Task<int> GetCancelledBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Bookings.Where(b => b.Status == BookingStatus.Cancelled);
        if (startDate.HasValue)
            query = query.Where(b => b.CheckInDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(b => b.CheckInDate <= endDate.Value);
        return await query.CountAsync();
    }

    public async Task<int> GetPendingBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Bookings.Where(b => b.Status == BookingStatus.Pending);
        if (startDate.HasValue)
            query = query.Where(b => b.CheckInDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(b => b.CheckInDate <= endDate.Value);
        return await query.CountAsync();
    }

    public async Task<IEnumerable<dynamic>> GetDailyBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.Bookings.AsQueryable();
        if (startDate.HasValue)
            query = query.Where(b => b.CheckInDate >= startDate.Value);
        if (endDate.HasValue)
            query = query.Where(b => b.CheckInDate <= endDate.Value);

        var result = await query
            .GroupBy(b => b.CheckInDate.Date)
            .Select(g => new DailyBookingDto
            {
                Date = g.Key,
                Count = g.Count(),
                CompletedCount = g.Count(b => b.Status == BookingStatus.Completed),
                PendingCount = g.Count(b => b.Status == BookingStatus.Pending),
                CancelledCount = g.Count(b => b.Status == BookingStatus.Cancelled)
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<RoomTypeBooking>> GetRoomTypeBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT 
                rt.Name AS RoomType,
                COUNT(b.Id) AS BookedCount,
                ISNULL(SUM(i.TotalAmount), 0) AS TotalRevenue
            FROM Booking b
                INNER JOIN Room r ON b.RoomId = r.Id
                INNER JOIN RoomType rt ON r.RoomTypeId = rt.Id
                LEFT JOIN Invoices i ON b.Id = i.BookingId
            WHERE (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            GROUP BY rt.Name";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<RoomTypeBooking>(query, parameters)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT SUM(i.TotalAmount) AS Value
            FROM Booking b
            INNER JOIN Invoices i ON b.Id = i.Id
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal?>(query, parameters.ToArray())
            .SingleOrDefaultAsync();

        return result ?? 0m;
    }

    public async Task<decimal> GetMonthlyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT SUM(i.TotalAmount) AS VALUE
            FROM Booking b
            INNER JOIN Invoices i ON b.Id = i.Id
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal?>(query, parameters.ToArray())
            .SingleOrDefaultAsync();

        return result ?? 0m;
    }

    public async Task<decimal> GetWeeklyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT SUM(i.TotalAmount) AS VALUE
            FROM Booking b
            INNER JOIN Invoices i ON b.Id = i.Id
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal?>(query, parameters.ToArray())
            .SingleOrDefaultAsync();

        return result ?? 0m;
    }

    public async Task<IEnumerable<DailyRevenue>> GetDailyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT 
                CAST(b.CheckInDate AS DATE) AS Date,
                SUM(i.TotalAmount) AS Revenue
            FROM Booking b
            INNER JOIN Invoices i ON b.Id = i.Id
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            GROUP BY CAST(b.CheckInDate AS DATE)
            ORDER BY CAST(b.CheckInDate AS DATE)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<DailyRevenue>(query, parameters.ToArray())
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueDataAsync(int months = 6)
    {
        var endDate = DateTime.Today;
        var startDate = endDate.AddMonths(-months);

        var query = @"
            SELECT 
                CONCAT(YEAR(b.CheckInDate), '-', FORMAT(MONTH(b.CheckInDate), '00')) AS Month,
                COUNT(b.Id) AS BookingCount,
                SUM(i.TotalAmount) AS Revenue
            FROM Booking b
            INNER JOIN Invoices i ON b.Id = i.Id
            WHERE b.Status = 'Completed'
            AND b.CheckInDate >= @StartDate
            AND b.CheckInDate <= @EndDate
            GROUP BY YEAR(b.CheckInDate), MONTH(b.CheckInDate)
            ORDER BY YEAR(b.CheckInDate), MONTH(b.CheckInDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", startDate),
            new SqlParameter("@EndDate", endDate)
        };

        return await _context.Database
            .SqlQueryRaw<MonthlyRevenue>(query, parameters.ToArray())
            .ToListAsync();
    }

    public async Task<List<Booking>> GetBookingsByHotelIdsAsync(IEnumerable<int> hotelIds)
    {
        if (hotelIds == null || !hotelIds.Any())
            return new List<Booking>();

        // Tạo danh sách tham số động: @p0, @p1, ...
        var parameters = hotelIds
            .Select((id, index) => new SqlParameter($"@p{index}", id))
            .ToArray();

        // Ghép chuỗi IN clause
        var inClause = string.Join(", ", parameters.Select(p => p.ParameterName));

        var sql = $@"
        SELECT b.*
        FROM Booking b
        JOIN Room r     ON b.RoomId = r.Id
        JOIN Floor f    ON r.FloorId = f.Id
        JOIN Building bd ON f.BuildingId = bd.Id
        WHERE bd.HotelId IN ({inClause})
    ";

        return await _context.Bookings
            .FromSqlRaw(sql, parameters)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Booking> GetBookingByIdWithHotelIdsAsync(IEnumerable<int> hotelIds, int bookingId)
    {
        var sql = @"
            SELECT 
                b.*
            FROM Booking b
            JOIN Room r ON b.RoomId = r.Id
            JOIN Floor f ON r.FloorId = f.Id
            JOIN Building bd ON f.BuildingId = bd.Id
            WHERE bd.HotelId IN @HotelIds
            AND b.Id = @BookingId";

        var parameters = new[]
        {
            new SqlParameter("@HotelIds", hotelIds),
            new SqlParameter("@BookingId", bookingId),
        };

        return await _context.Database
            .SqlQueryRaw<Booking>(sql, parameters.ToArray())
            .FirstOrDefaultAsync();
    }

}

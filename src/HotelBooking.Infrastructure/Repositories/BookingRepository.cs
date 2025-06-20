﻿using HotelBooking.Application.CQRS.Booking.DTOs;
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
        var query = @"SELECT COUNT(*) AS Value 
                    FROM Booking b 
                    WHERE (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                    AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<int>(query, parameters)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetCompletedBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"SELECT COUNT(*) AS Value 
                    FROM Booking b 
                    WHERE b.Status = 'Completed'
                    AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                    AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<int>(query, parameters)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetCancelledBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"SELECT COUNT(*) AS Value 
                    FROM Booking b 
                    WHERE b.Status = 'Cancelled'
                    AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                    AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<int>(query, parameters)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetPendingBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"SELECT COUNT(*) AS Value 
                    FROM Booking b 
                    WHERE b.Status = 'Pending'
                    AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                    AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<int>(query, parameters)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<dynamic>> GetDailyBookingsAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT 
                CAST(b.CheckInDate AS DATE) AS Date,
                COUNT(*) AS Count,
                SUM(CASE WHEN b.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedCount,
                SUM(CASE WHEN b.Status = 'Pending' THEN 1 ELSE 0 END) AS PendingCount,
                SUM(CASE WHEN b.Status = 'Cancelled' THEN 1 ELSE 0 END) AS CancelledCount
            FROM Booking b
            WHERE (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
                AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            GROUP BY CAST(b.CheckInDate AS DATE)
            ORDER BY Date";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<DailyBookingDto>(query, parameters)
            .ToListAsync();
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
                LEFT JOIN Invoice i ON b.Id = i.BookingId
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
            SELECT ISNULL(SUM(i.TotalAmount), 0) AS Value
            FROM Booking b
            INNER JOIN Invoice i ON b.Id = i.BookingId
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal>(query, parameters)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<decimal> GetMonthlyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT ISNULL(SUM(i.TotalAmount), 0) AS Value
            FROM Booking b
            INNER JOIN Invoice i ON b.Id = i.BookingId
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            AND b.CheckInDate >= DATEADD(MONTH, -1, GETDATE())";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal>(query, parameters)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<decimal> GetWeeklyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT ISNULL(SUM(i.TotalAmount), 0) AS Value
            FROM Booking b
            INNER JOIN Invoice i ON b.Id = i.BookingId
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            AND b.CheckInDate >= DATEADD(WEEK, -1, GETDATE())";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        var result = await _context.Database
            .SqlQueryRaw<decimal>(query, parameters)
            .SingleOrDefaultAsync();

        return result;
    }

    public async Task<IEnumerable<DailyRevenue>> GetDailyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = @"
            SELECT 
                CAST(b.CheckInDate AS DATE) AS Date,
                ISNULL(SUM(i.TotalAmount), 0) AS Revenue
            FROM Booking b
            INNER JOIN Invoice i ON b.Id = i.BookingId
            WHERE b.Status = 'Completed'
            AND (@StartDate IS NULL OR b.CheckInDate >= @StartDate)
            AND (@EndDate IS NULL OR b.CheckInDate <= @EndDate)
            GROUP BY CAST(b.CheckInDate AS DATE)
            ORDER BY Date";

        var parameters = new[]
        {
            new SqlParameter("@StartDate", (object?)startDate ?? DBNull.Value),
            new SqlParameter("@EndDate", (object?)endDate ?? DBNull.Value)
        };

        return await _context.Database
            .SqlQueryRaw<DailyRevenue>(query, parameters)
            .ToListAsync();
    }

    public async Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueDataAsync(int months = 6)
    {
        var query = @"
            SELECT 
                FORMAT(b.CheckInDate, 'yyyy-MM') AS Month,
                COUNT(b.Id) AS BookingCount,
                ISNULL(SUM(i.TotalAmount), 0) AS Revenue
            FROM Booking b
            INNER JOIN Invoice i ON b.Id = i.BookingId
            WHERE b.Status = 'Completed'
            AND b.CheckInDate >= DATEADD(MONTH, -@Months, GETDATE())
            GROUP BY FORMAT(b.CheckInDate, 'yyyy-MM')
            ORDER BY Month";

        var parameters = new[]
        {
            new SqlParameter("@Months", months)
        };

        return await _context.Database
            .SqlQueryRaw<MonthlyRevenue>(query, parameters)
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

    public async Task<Guest?> FindExistingGuestAsync(string? citizenIdNumber, string? passportNumber)
    {
        if (string.IsNullOrEmpty(citizenIdNumber) && string.IsNullOrEmpty(passportNumber))
            return null;

        var query = _context.Bookings
            .Include(b => b.Guests)
            .Where(b => !b.IsDeleted)
            .SelectMany(b => b.Guests);

        if (!string.IsNullOrEmpty(citizenIdNumber))
            query = query.Where(g => g.CitizenIdNumber == citizenIdNumber);
        else if (!string.IsNullOrEmpty(passportNumber))
            query = query.Where(g => g.PassportNumber == passportNumber);

        return await query.FirstOrDefaultAsync();
    }

}

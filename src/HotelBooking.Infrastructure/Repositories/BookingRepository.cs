using AutoMapper;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Domain.Common;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories;

public class BookingRepository : GenericRepository<Booking>, IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context, IMapper mapper, IUnitOfWork unitOfWork) 
        : base(context, mapper, unitOfWork)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
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

    public async Task<bool> IsRoomAvailableForBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate)
    {
        return !await _context.Bookings
            .AnyAsync(b => b.RoomId == roomId &&
                b.Status != BookingStatus.Cancelled &&
                b.Status != BookingStatus.Completed &&
                ((checkInDate >= b.CheckInTime && checkInDate < b.CheckOutTime) ||
                 (checkOutDate > b.CheckInTime && checkOutDate <= b.CheckOutTime) ||
                 (checkInDate <= b.CheckInTime && checkOutDate >= b.CheckOutTime)));
    }
}

using HotelBooking.Application.Common.Models;
using HotelBooking.Application.Services.DTOs;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Application.Services;

public interface IReportingService
{
    Task<Result<BookingStatisticsDto>> GetBookingStatisticsAsync(DateTime startDate, DateTime endDate);
    Task<Result<RevenueReportDto>> GetRevenueReportAsync(DateTime startDate, DateTime endDate);
    Task<Result<OccupancyReportDto>> GetOccupancyReportAsync(DateTime startDate, DateTime endDate);
    Task<Result<GuestReportDto>> GetGuestReportAsync(DateTime startDate, DateTime endDate);
}

public class ReportingService : IReportingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRoomRepository _roomRepository;

    public ReportingService(
        IBookingRepository bookingRepository,
        IInvoiceRepository invoiceRepository,
        IRoomRepository roomRepository)
    {
        _bookingRepository = bookingRepository;
        _invoiceRepository = invoiceRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result<BookingStatisticsDto>> GetBookingStatisticsAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var totalBookings = bookings.Count;
            var confirmedBookings = bookings.Count(b => b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.Confirmed);
            var checkedInBookings = bookings.Count(b => b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.CheckedIn);
            var checkedOutBookings = bookings.Count(b => b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.CheckedOut);
            var cancelledBookings = bookings.Count(b => b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.Cancelled);
            var noShowBookings = bookings.Count(b => b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.NoShow);

            var statistics = new BookingStatisticsDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalBookings = totalBookings,
                ConfirmedBookings = confirmedBookings,
                CheckedInBookings = checkedInBookings,
                CheckedOutBookings = checkedOutBookings,
                CancelledBookings = cancelledBookings,
                NoShowBookings = noShowBookings,
                ConfirmationRate = totalBookings > 0 ? (decimal)confirmedBookings / totalBookings : 0,
                CancellationRate = totalBookings > 0 ? (decimal)cancelledBookings / totalBookings : 0,
                NoShowRate = totalBookings > 0 ? (decimal)noShowBookings / totalBookings : 0
            };

            return Result<BookingStatisticsDto>.Success(statistics);
        }
        catch (Exception ex)
        {
            return Result<BookingStatisticsDto>.Failure($"Error generating booking statistics: {ex.Message}");
        }
    }

    public async Task<Result<RevenueReportDto>> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var invoices = await _invoiceRepository.GetPendingInvoicesAsync(startDate, endDate);

            var totalRevenue = invoices.Sum(i => i.TotalAmount);
            var paidRevenue = invoices.Sum(i => i.PaidAmount);
            var pendingRevenue = invoices.Sum(i => i.RemainingAmount);
            var latePaymentFees = invoices.Sum(i => i.LatePaymentFee);

            var report = new RevenueReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = totalRevenue,
                PaidRevenue = paidRevenue,
                PendingRevenue = pendingRevenue,
                LatePaymentFees = latePaymentFees,
                PaymentRate = totalRevenue > 0 ? paidRevenue / totalRevenue : 0,
                AverageInvoiceAmount = invoices.Any() ? invoices.Average(i => i.TotalAmount) : 0,
                TotalInvoices = invoices.Count(),
                PaidInvoices = invoices.Count(i => i.Status == Domain.AggregateModels.InvoiceAggregate.InvoiceStatus.Paid),
                PendingInvoices = invoices.Count(i => i.Status == Domain.AggregateModels.InvoiceAggregate.InvoiceStatus.Pending),
                OverdueInvoices = invoices.Count(i => i.Status == Domain.AggregateModels.InvoiceAggregate.InvoiceStatus.Overdue)
            };

            return Result<RevenueReportDto>.Success(report);
        }
        catch (Exception ex)
        {
            return Result<RevenueReportDto>.Failure($"Error generating revenue report: {ex.Message}");
        }
    }

    public async Task<Result<OccupancyReportDto>> GetOccupancyReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var rooms = await _roomRepository.GetAllAsync();
            var totalRooms = rooms.Count();

            var dailyOccupancy = new Dictionary<DateTime, int>();
            var currentDate = startDate;

            while (currentDate <= endDate)
            {
                var occupiedRooms = bookings.Count(b => 
                    b.Status == Domain.AggregateModels.BookingAggregate.BookingStatus.CheckedIn &&
                    b.CheckInTime <= currentDate &&
                    (!b.CheckOutTime.HasValue || b.CheckOutTime > currentDate));

                dailyOccupancy[currentDate] = occupiedRooms;
                currentDate = currentDate.AddDays(1);
            }

            var report = new OccupancyReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRooms = totalRooms,
                AverageOccupancyRate = dailyOccupancy.Values.Average(o => (decimal)o / totalRooms),
                PeakOccupancyRate = dailyOccupancy.Values.Max(o => (decimal)o / totalRooms),
                LowestOccupancyRate = dailyOccupancy.Values.Min(o => (decimal)o / totalRooms),
                DailyOccupancy = dailyOccupancy
            };

            return Result<OccupancyReportDto>.Success(report);
        }
        catch (Exception ex)
        {
            return Result<OccupancyReportDto>.Failure($"Error generating occupancy report: {ex.Message}");
        }
    }

    public async Task<Result<GuestReportDto>> GetGuestReportAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            var bookings = await _bookingRepository.GetBookingsByDateRangeAsync(startDate, endDate);
            var totalGuests = bookings.Sum(b => b.Guests.Count);
            var uniqueGuests = bookings.SelectMany(b => b.Guests).Distinct().Count();

            var report = new GuestReportDto
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalGuests = totalGuests,
                UniqueGuests = uniqueGuests,
                AverageGuestsPerBooking = bookings.Any() ? (decimal)totalGuests / bookings.Count : 0,
                RepeatGuests = uniqueGuests > 0 ? bookings.SelectMany(b => b.Guests).GroupBy(g => g.CitizenIdNumber).Count(g => g.Count() > 1) : 0,
                GuestNationalities = bookings.SelectMany(b => b.Guests)
                    .GroupBy(g => g.PassportNumber.Substring(0, 2))
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return Result<GuestReportDto>.Success(report);
        }
        catch (Exception ex)
        {
            return Result<GuestReportDto>.Failure($"Error generating guest report: {ex.Message}");
        }
    }
} 
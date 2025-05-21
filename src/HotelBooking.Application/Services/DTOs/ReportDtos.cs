namespace HotelBooking.Application.Services.DTOs;

public class BookingStatisticsDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalBookings { get; set; }
    public int ConfirmedBookings { get; set; }
    public int CheckedInBookings { get; set; }
    public int CheckedOutBookings { get; set; }
    public int CancelledBookings { get; set; }
    public int NoShowBookings { get; set; }
    public decimal ConfirmationRate { get; set; }
    public decimal CancellationRate { get; set; }
    public decimal NoShowRate { get; set; }
}

public class RevenueReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal PaidRevenue { get; set; }
    public decimal PendingRevenue { get; set; }
    public decimal LatePaymentFees { get; set; }
    public decimal PaymentRate { get; set; }
    public decimal AverageInvoiceAmount { get; set; }
    public int TotalInvoices { get; set; }
    public int PaidInvoices { get; set; }
    public int PendingInvoices { get; set; }
    public int OverdueInvoices { get; set; }
}

public class OccupancyReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalRooms { get; set; }
    public decimal AverageOccupancyRate { get; set; }
    public decimal PeakOccupancyRate { get; set; }
    public decimal LowestOccupancyRate { get; set; }
    public Dictionary<DateTime, int> DailyOccupancy { get; set; } = new();
}

public class GuestReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalGuests { get; set; }
    public int UniqueGuests { get; set; }
    public decimal AverageGuestsPerBooking { get; set; }
    public int RepeatGuests { get; set; }
    public Dictionary<string, int> GuestNationalities { get; set; } = new();
}
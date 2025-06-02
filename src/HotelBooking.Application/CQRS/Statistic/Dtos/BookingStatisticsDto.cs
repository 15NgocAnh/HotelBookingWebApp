namespace HotelBooking.Application.CQRS.Statistic.Dtos
{
    public class BookingStatisticsDto
    {
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public int PendingBookings { get; set; }
        public List<DailyBookingDto> DailyBookings { get; set; } = new();
        public List<RoomTypeBookingDto> RoomTypeBookings { get; set; } = new();
    }

    public class DailyBookingDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public int CompletedCount { get; set; }
        public int PendingCount { get; set; }
        public int CancelledCount { get; set; }
    }

    public class RoomTypeBookingDto
    {
        public string RoomType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
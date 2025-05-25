namespace HotelBooking.Application.CQRS.Dashboard.Dtos
{
    public class DashboardStatisticsDto
    {
        public int TotalUsers { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int TotalBookings { get; set; }
        public int AvailableRooms { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenueData { get; set; } = new();
        public List<RoomTypeStatisticsDto> RoomTypeStatistics { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
    }

    public class RoomTypeStatisticsDto
    {
        public string RoomType { get; set; } = string.Empty;
        public int BookedCount { get; set; }
        public int AvailableCount { get; set; }
    }
}
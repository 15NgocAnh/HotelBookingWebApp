namespace HotelBooking.Application.CQRS.Statistic.Dtos
{
    public class RevenueStatisticsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalMonthlyRevenue { get; set; }
        public decimal TotalWeeklyRevenue { get; set; }
        public List<DailyRevenueDto> DailyRevenue { get; set; } = new();
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    }

    public class DailyRevenueDto
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int BookingCount { get; set; }
    }
}
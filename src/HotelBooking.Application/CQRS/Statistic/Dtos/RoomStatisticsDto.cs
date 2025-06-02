namespace HotelBooking.Application.CQRS.Statistic.Dtos
{
    public class RoomStatisticsDto
    {
        public int TotalRooms { get; set; }
        public int AvailableRooms { get; set; }
        public int BookedRooms { get; set; }
        public int CleaningUpRooms { get; set; }
        public int MaintenanceRooms { get; set; }
        public List<RoomTypeStatisticsDto> RoomTypeStatistics { get; set; } = new();
        public List<RoomStatusStatisticsDto> RoomStatusStatistics { get; set; } = new();
    }

    public class RoomTypeStatisticsDto
    {
        public string RoomType { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Available { get; set; }
        public int Booked { get; set; }
        public int CleaningUp { get; set; }
        public int Maintenance { get; set; }
    }

    public class RoomStatusStatisticsDto
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
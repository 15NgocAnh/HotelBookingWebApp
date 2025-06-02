using HotelBooking.Domain.AggregateModels.BookingAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetBookingsByCustomerIdAsync(string citizenIdNumber);
        Task<List<Booking>> GetBookingsByRoomIdAsync(int roomId);
        Task<List<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<Booking>> GetActiveBookingsAsync();
        Task<List<Booking>> GetPendingCheckoutsAsync(DateTime date);
        Task<List<Booking>> GetPendingCheckinsAsync(DateTime date);
        Task<bool> IsRoomAvailableForBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate, int excludeBookingId = 0);
        Task<int> GetTotalBookingsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetCompletedBookingsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetCancelledBookingsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<int> GetPendingBookingsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<dynamic>> GetDailyBookingsAsync(DateTime? startDate = null, DateTime? endDate = null); 
        public record RoomTypeBooking(string RoomType, int BookedCount, decimal TotalRevenue);
        Task<IEnumerable<RoomTypeBooking>> GetRoomTypeBookingsAsync(DateTime? startDate = null, DateTime? endDate = null); 
        public record DailyRevenue(DateTime Date, decimal Revenue);
        public record MonthlyRevenue(string Month, int BookingCount, decimal Revenue);
        Task<decimal> GetTotalRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetMonthlyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetWeeklyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<DailyRevenue>> GetDailyRevenueAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<MonthlyRevenue>> GetMonthlyRevenueDataAsync(int months = 6);
    }
}

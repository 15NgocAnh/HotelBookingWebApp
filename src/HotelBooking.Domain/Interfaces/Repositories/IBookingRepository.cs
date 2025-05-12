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
        Task<bool> IsRoomAvailableForBookingAsync(int roomId, DateTime checkInDate, DateTime checkOutDate);
    }
}

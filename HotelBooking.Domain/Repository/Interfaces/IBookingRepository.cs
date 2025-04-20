using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IBookingRepository : IGenericRepository<BookingModel>
    {
        Task<IEnumerable<BookingModel>> GetAllAsync();
        Task<BookingModel> GetByIdAsync(int id);
    }
}

using HotelBooking.Domain.AggregateModels.HotelAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IHotelRepository : IGenericRepository<Hotel>
    {
        Task<List<Hotel>> GetAllByUserIdAsync(int userId);
        Task<Hotel?> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);
        Task<bool> HasBuildingsAsync(int hotelId);
        Task<int> CountBuildingsAsync(int hotelId);
        Task DeleteAsync(int id);
    }
}
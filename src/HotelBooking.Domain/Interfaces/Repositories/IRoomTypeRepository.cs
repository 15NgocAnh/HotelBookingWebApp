using HotelBooking.Domain.AggregateModels.RoomTypeAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoomTypeRepository : IGenericRepository<RoomType>
    {
        Task<RoomType?> GetByNameAsync(string name);
        Task<bool> IsNameUniqueAsync(string name);
        Task<bool> HasRoomsAsync(int roomTypeId);
        Task<int> CountRoomsAsync(int roomTypeId);
        Task DeleteAsync(int id);
    }
}
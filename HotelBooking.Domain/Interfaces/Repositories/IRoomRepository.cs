using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Room>> GetByFloorIdAsync(int floorId);
        Task<IEnumerable<Room>> GetByRoomTypeIdAsync(int roomTypeId);
    }
}
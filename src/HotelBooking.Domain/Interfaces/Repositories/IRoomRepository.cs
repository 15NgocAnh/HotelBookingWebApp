using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetRoomsByFloorIdAsync(int floorId);
        Task<Room?> GetByRoomNumberAsync(string roomNumber);
        Task<bool> IsRoomNumberUniqueInBuildingAsync(int buildingId, string roomNumber);
        Task<bool> HasActiveBookingsAsync(int roomId);
        Task DeleteAsync(int id);
    }
}
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetAllRoomsAvailableAsync();
        Task<List<Room>> GetRoomsByBuildingAsync(int buildingId);
        Task<Room?> GetByRoomNumberAsync(string roomNumber);
        Task<bool> IsRoomNumberUniqueInBuildingAsync(int buildingId, string roomNumber);
        Task<bool> HasActiveBookingsAsync(int roomId);
        Task DeleteAsync(int id);
        public record RoomWithTypeName(int RoomId, int RoomTypeId, string RoomTypeName, string Status);
        Task<List<RoomWithTypeName>> GetRoomWithTypeNamesAsync(CancellationToken cancellationToken); 
        public record RoomTypeStatistics(
            string RoomType,
            int Total,
            int Available,
            int Booked,
            int CleaningUp,
            int UnderMaintenance);
        Task<IEnumerable<RoomTypeStatistics>> GetRoomTypeStatisticsAsync();
        public record RoomStatusStatistic(string Status, int Count);
        Task<IEnumerable<RoomStatusStatistic>> GetRoomStatusStatisticsAsync();
    }
}
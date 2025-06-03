using HotelBooking.Domain.AggregateModels.BuildingAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IBuildingRepository : IGenericRepository<Building>
    {
        Task<Building?> GetByIdAsync(int id);
        Task<Floor?> GetFloorByIdAsync(int floorId);
        Task<List<int>> GetBuildingIdsByHotelIdsAsync(List<int> hotelIds);
        Task<List<int>> GetFloorIdsByBuildingIdsAsync(List<int> buildingIds);
        Task<Building> GetBuildingByFloorIdAsync(int floorId);
        Task<IEnumerable<Floor>> GetAllFloorsByBuildingIdAsync(int buildingId);
        Task<IEnumerable<Building>> GetBuildingsByHotelAsync(int hotelId);
        Task<bool> IsNameUniqueInHotelAsync(int hotelId, string name);
        Task DeleteAsync(int id);
    }
}
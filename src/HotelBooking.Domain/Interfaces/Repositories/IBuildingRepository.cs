using HotelBooking.Domain.AggregateModels.BuildingAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IBuildingRepository : IGenericRepository<Building>
    {
        Task<Floor> GetFloorByIdAsync(int floorId);
        Task<IEnumerable<Floor>> GetAllFloorsByBuildingIdAsync(int buildingId);
        Task<IEnumerable<Building>> GetBuildingsByHotelAsync(int hotelId);
        Task<bool> IsNameUniqueInHotelAsync(int hotelId, string name);
        Task DeleteAsync(int id);
    }
}
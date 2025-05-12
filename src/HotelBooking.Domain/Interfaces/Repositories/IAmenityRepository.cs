using HotelBooking.Domain.AggregateModels.AmenityAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IAmenityRepository : IGenericRepository<Amenity>
    {
        Task<IEnumerable<Amenity>> GetByIdsAsync(IEnumerable<int> ids);
        Task DeleteAsync(int id);
    }
} 
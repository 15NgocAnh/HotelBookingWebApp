using HotelBooking.Domain.AggregateModels.BedTypeAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IBedTypeRepository : IGenericRepository<BedType>
    {
        Task<IEnumerable<BedType>> GetByIdsAsync(IEnumerable<int> ids);
        Task DeleteAsync(int id);
    }
} 
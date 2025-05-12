using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IExtraCategoryRepository : IGenericRepository<ExtraCategory>
    {
        Task<bool> IsNameUniqueAsync(string name);
        Task<bool> HasItemsAsync(int categoryId);
        Task DeleteAsync(int id);
    }
} 
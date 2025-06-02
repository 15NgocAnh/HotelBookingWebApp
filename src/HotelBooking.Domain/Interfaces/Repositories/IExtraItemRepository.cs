using HotelBooking.Domain.AggregateModels.ExtraItemAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IExtraItemRepository : IGenericRepository<ExtraItem>
    {
        Task<bool> IsNameUniqueInCategoryAsync(int categoryId, string name);
        Task<bool> IsInUseAsync(int itemId); 
        Task DeleteAsync(int id);
    }
} 
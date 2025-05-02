using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IDynamicPricingRepository : IGenericRepository<DynamicPricingRule>
    {
        Task<IEnumerable<DynamicPricingRule>> GetByRoomTypeIdAsync(int roomTypeId);
        Task<DynamicPricingRule> GetByPricingIdAsync(int pricingId);
        Task<IEnumerable<DynamicPricingRule>> GetActiveRulesForDateAsync(DateTime date);
        Task<IEnumerable<DynamicPricingRule>> GetActiveRulesForDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 
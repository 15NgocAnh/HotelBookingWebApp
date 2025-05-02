using HotelBooking.Domain.Entities;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IExtraChargesRepository : IGenericRepository<ExtraCharge>
    {
        Task DeleteAllByDynamicPricingIdAsync(int id);
    }
}

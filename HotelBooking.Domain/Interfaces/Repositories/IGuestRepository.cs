using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IGuestRepository : IGenericRepository<GuestModel>
    {
        Task<GuestModel> GetByIdentityNumberAsync(string identityNumber);
    }
} 
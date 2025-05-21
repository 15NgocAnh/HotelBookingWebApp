using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<bool> IsNameUniqueAsync(string name);
        Task<Role> GetRolesByUserIdAsync(int userId);
    }
}

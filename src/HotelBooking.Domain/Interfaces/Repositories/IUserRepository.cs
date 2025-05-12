using HotelBooking.Domain.AggregateModels.UserAggregate;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdWithRolesAsync(int id);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string roleName);
        Task<bool> IsEmailUniqueAsync(string email);
    }
}

using HotelBooking.Data;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IUserRoleRepository : IGenericRepository<UserRoleModel>
    {
        Task<UserRoleModel> GetUserRoleAsync(UserModel user, RoleModel role);
    }
}

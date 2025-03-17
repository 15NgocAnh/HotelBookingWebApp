using HotelBooking.Data;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Repository.Interfaces
{
    public interface IUserRoleRepository : IGenericRepository<UserRoleModel>
    {
        Task<UserRoleModel> GetUserRoleAsync(UserModel user, RoleModel role);
    }
}

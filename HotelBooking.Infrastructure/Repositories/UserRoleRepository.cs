using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace HotelBooking.Domain.Repository
{
    public class UserRoleRepository : GenericRepository<UserRoleModel>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<UserRoleModel> GetUserRoleAsync(UserModel user, RoleModel role)
        {
            return await _context.UserRoles.Where(x => x.User == user).Where(x => x.Role == role).FirstOrDefaultAsync();
        }
    }
}

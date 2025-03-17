using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Repository.Interfaces;
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
            return await _context.user_roles.Where(x => x.user == user).Where(x => x.role == role).FirstOrDefaultAsync();
        }
    }
}

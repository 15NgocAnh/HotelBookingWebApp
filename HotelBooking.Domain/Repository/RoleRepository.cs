using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.Repository.Interfaces;
using HotelBooking.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Repository
{
    public class RoleRepository : GenericRepository<RoleModel>, IRoleRepository
    {
        public RoleRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
        public async Task<RoleModel> getRoleByLevel_NameAsync(int level, string roleName)
        {
            return await _context.roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<RoleModel> getRoleByNameAsync(string roleName)
        {
            return await _context.roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<RoleModel> getRoleExceptAdmin(string roleName)
        {
            return await _context.roles.Where(r => !r.RoleName.Equals(CJConstant.ADMIN, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync(r => r.RoleName == roleName);
        }

        public async Task<RoleModel> getRoleByName(string roleName)
        {
            return _context.roles.FirstOrDefault(r => r.RoleName == roleName);
        }
    }
}

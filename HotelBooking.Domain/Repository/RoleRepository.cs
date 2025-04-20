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
        public async Task<RoleModel> getRoleByLevel_NameAsync(int level, string Name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == Name);
        }

        public async Task<RoleModel> getRoleByNameAsync(string Name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == Name);
        }

        public async Task<RoleModel> getRoleExceptAdmin(string Name)
        {
            return await _context.Roles.Where(r => !r.Name.Equals(CJConstant.ADMIN, StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync(r => r.Name == Name);
        }

        public async Task<RoleModel> getRoleByName(string Name)
        {
            return _context.Roles.FirstOrDefault(r => r.Name == Name);
        }
    }
}

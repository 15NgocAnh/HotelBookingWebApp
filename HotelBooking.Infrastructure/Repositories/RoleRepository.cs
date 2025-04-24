using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Interfaces.Repositories;

namespace HotelBooking.Domain.Repository
{
    public class RoleRepository : GenericRepository<RoleModel>, IRoleRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoleRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> AssignRoleToUserAsync(int userId, int roleId)
        {
            try
            {
                var userRole = new UserRoleModel
                {
                    UserId = userId,
                    RoleId = roleId
                };
                await _context.UserRoles.AddAsync(userRole);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<RoleModel> CreateRoleAsync(RoleModel role)
        {
            await _context.Roles.AddAsync(role);

            if (role.RolePermissions?.Any() == true)
            {
                await _context.RolePermissions.AddRangeAsync(role.RolePermissions);
            }

            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await (from r in _context.Roles
                            where r.Id == id
                            select r).FirstOrDefaultAsync();

            if (role == null)
                return false;

            role.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PermissionModel>> GetAllPermissionsAsync()
        {
            return await (from p in _context.Permissions
                         select p).ToListAsync();
        }

        public async Task<List<RoleModel>> GetAllRolesActiveAsync()
        {
            return await (from r in _context.Roles
                         where r.IsActive
                         select r)
                         .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                         .ToListAsync();
        }

        public async Task<List<RoleModel>> GetAllRolesAsync()
        {
            return await (from r in _context.Roles
                         select r)
                         .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                         .ToListAsync();
        }

        public async Task<RoleModel> GetRoleByIdAsync(int id)
        {
            return await (from r in _context.Roles
                         where r.Id == id
                         select r)
                         .Include(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                         .FirstOrDefaultAsync();
        }

        public async Task<bool> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var userRole = await (from ur in _context.UserRoles
                                where ur.UserId == userId && ur.RoleId == roleId
                                select ur).FirstOrDefaultAsync();

            if (userRole == null)
                return false;

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<RoleModel> UpdateRoleAsync(int id, RoleModel updateRole)
        {
            var role = await (from r in _context.Roles
                            where r.Id == id
                            select r)
                            .Include(r => r.RolePermissions)
                            .FirstOrDefaultAsync();

            if (role == null)
                return null;

            role.Name = updateRole.Name;
            role.Description = updateRole.Description;
            role.IsActive = updateRole.IsActive;

            // Update permissions
            if (updateRole.RolePermissions != null)
            {
                _context.RolePermissions.RemoveRange(role.RolePermissions);
                role.RolePermissions = updateRole.RolePermissions;
            }

            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<RoleModel> GetRoleByNameAsync(string roleName)
        {
            return await (from r in _context.Roles
                          where r.Name == roleName
                          select r)
                         .FirstOrDefaultAsync();
        }
    }
}

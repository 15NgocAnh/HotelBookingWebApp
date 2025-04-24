using HotelBooking.Data.Models;

namespace HotelBooking.Domain.Interfaces.Repositories
{
    public interface IRoleRepository : IGenericRepository<RoleModel>
    {
        Task<List<RoleModel>> GetAllRolesAsync();
        Task<List<RoleModel>> GetAllRolesActiveAsync();
        Task<RoleModel> GetRoleByIdAsync(int id);
        Task<RoleModel> CreateRoleAsync(RoleModel createRoleDto);
        Task<RoleModel> UpdateRoleAsync(int id, RoleModel updateRoleDto);
        Task<bool> DeleteRoleAsync(int id);
        Task<List<PermissionModel>> GetAllPermissionsAsync();
        Task<bool> AssignRoleToUserAsync(int userId, int roleId);
        Task<bool> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<RoleModel> GetRoleByNameAsync(string roleName);
    }
}

using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Interfaces.Services
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<RoleDto>>> GetAllRolesAsync();
        Task<ServiceResponse<List<RoleDto>>> GetAllRolesActiveAsync();
        Task<ServiceResponse<RoleDto>> GetRoleByIdAsync(int id);
        Task<ServiceResponse<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<ServiceResponse<RoleDto>> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto);
        Task<ServiceResponse<string>> DeleteRoleAsync(int id);
        Task<ServiceResponse<List<PermissionDto>>> GetAllPermissionsAsync();
        Task<ServiceResponse<string>> AssignRoleToUserAsync(int userId, int roleId);
        Task<ServiceResponse<string>> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<ServiceResponse<RoleDto>> GetRoleByNameAsync(string roleName);
    }
}
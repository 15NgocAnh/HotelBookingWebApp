using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelBooking.Domain.DTOs;

namespace HotelBooking.Domain.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto);
        Task<RoleDto> UpdateRoleAsync(Guid id, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteRoleAsync(Guid id);
        Task<List<PermissionDto>> GetAllPermissionsAsync();
        Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task<bool> RemoveRoleFromUserAsync(Guid userId, Guid roleId);
    }
} 
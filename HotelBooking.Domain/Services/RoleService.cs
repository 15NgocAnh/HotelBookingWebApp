using AutoMapper;
using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.Interfaces.Repositories;
using HotelBooking.Domain.Interfaces.Services;
using HotelBooking.Domain.Response;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<RoleDto>>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();
                var rolesDto = _mapper.Map<List<RoleDto>>(roles);
                return new ServiceResponse<List<RoleDto>>
                {
                    Data = rolesDto,
                    Message = "Roles retrieved successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<RoleDto>>
                {
                    Message = $"Error retrieving roles: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<List<RoleDto>>> GetAllRolesActiveAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesActiveAsync();
                var rolesDto = _mapper.Map<List<RoleDto>>(roles);
                return new ServiceResponse<List<RoleDto>>
                {
                    Data = rolesDto,
                    Message = "Roles retrieved successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<RoleDto>>
                {
                    Message = $"Error retrieving roles: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<RoleDto>> GetRoleByIdAsync(int id)
        {
            try
            {
                var role = await _roleRepository.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return new ServiceResponse<RoleDto>
                    {
                        Message = $"Role with ID {id} not found",
                        ResponseType = EResponseType.NotFound
                    };
                }

                var roleDto = _mapper.Map<RoleDto>(role);
                return new ServiceResponse<RoleDto>
                {
                    Data = roleDto,
                    Message = "Role retrieved successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RoleDto>
                {
                    Message = $"Error retrieving role: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
        {
            try
            {
                var role = _mapper.Map<RoleModel>(createRoleDto);
                var createdRole = await _roleRepository.CreateRoleAsync(role);
                var roleDto = _mapper.Map<RoleDto>(createdRole);

                return new ServiceResponse<RoleDto>
                {
                    Data = roleDto,
                    Message = "Role created successfully",
                    ResponseType = EResponseType.Created
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RoleDto>
                {
                    Message = $"Error creating role: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<RoleDto>> UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var existingRole = await _roleRepository.GetRoleByIdAsync(id);
                if (existingRole == null)
                {
                    return new ServiceResponse<RoleDto>
                    {
                        Message = $"Role with ID {id} not found",
                        ResponseType = EResponseType.NotFound
                    };
                }

                var role = _mapper.Map<RoleModel>(updateRoleDto);
                var updatedRole = await _roleRepository.UpdateRoleAsync(id, role);
                var roleDto = _mapper.Map<RoleDto>(updatedRole);

                return new ServiceResponse<RoleDto>
                {
                    Data = roleDto,
                    Message = "Role updated successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RoleDto>
                {
                    Message = $"Error updating role: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<string>> DeleteRoleAsync(int id)
        {
            try
            {
                var result = await _roleRepository.DeleteRoleAsync(id);
                if (!result)
                {
                    return new ServiceResponse<string>
                    {
                        Message = $"Role with ID {id} not found",
                        ResponseType = EResponseType.NotFound
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Role deleted successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>
                {
                    Message = $"Error deleting role: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<List<PermissionDto>>> GetAllPermissionsAsync()
        {
            try
            {
                var permissions = await _roleRepository.GetAllPermissionsAsync();
                var permissionsDto = _mapper.Map<List<PermissionDto>>(permissions);

                return new ServiceResponse<List<PermissionDto>>
                {
                    Data = permissionsDto,
                    Message = "Permissions retrieved successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<PermissionDto>>
                {
                    Message = $"Error retrieving permissions: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<string>> AssignRoleToUserAsync(int userId, int roleId)
        {
            try
            {
                var result = await _roleRepository.AssignRoleToUserAsync(userId, roleId);
                if (!result)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Failed to assign role to user",
                        ResponseType = EResponseType.CannotUpdate
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Role assigned to user successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>
                {
                    Message = $"Error assigning role to user: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<string>> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            try
            {
                var result = await _roleRepository.RemoveRoleFromUserAsync(userId, roleId);
                if (!result)
                {
                    return new ServiceResponse<string>
                    {
                        Message = "Role-user association not found",
                        ResponseType = EResponseType.NotFound
                    };
                }

                return new ServiceResponse<string>
                {
                    Message = "Role removed from user successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<string>
                {
                    Message = $"Error removing role from user: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }

        public async Task<ServiceResponse<RoleDto>> GetRoleByNameAsync(string roleName)
        {
            try
            {
                var role = await _roleRepository.GetRoleByNameAsync(roleName);
                if (role == null)
                {
                    return new ServiceResponse<RoleDto>
                    {
                        Message = $"Role with name {roleName} not found",
                        ResponseType = EResponseType.NotFound
                    };
                }

                var roleDto = _mapper.Map<RoleDto>(role);
                return new ServiceResponse<RoleDto>
                {
                    Data = roleDto,
                    Message = "Role retrieved successfully",
                    ResponseType = EResponseType.Success
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<RoleDto>
                {
                    Message = $"Error retrieving role: {ex.Message}",
                    ResponseType = EResponseType.InternalError
                };
            }
        }
    }
}
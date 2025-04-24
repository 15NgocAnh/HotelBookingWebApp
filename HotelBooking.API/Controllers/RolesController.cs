using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Authorize(Policy = "emailverified")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRoles()
        {
            var serviceResponse = await _roleService.GetAllRolesAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("active")]
        public async Task<ActionResult> GetAllRolesActive()
        {
            var serviceResponse = await _roleService.GetAllRolesActiveAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<ActionResult> GetRoleById(int id)
        {
            var serviceResponse = await _roleService.GetRoleByIdAsync(id);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleDto createRoleDto)
        {
            var serviceResponse = await _roleService.CreateRoleAsync(createRoleDto);
            if (serviceResponse.ResponseType != EResponseType.Created)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRole(int id, UpdateRoleDto updateRoleDto)
        {
            var serviceResponse = await _roleService.UpdateRoleAsync(id, updateRoleDto);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            var serviceResponse = await _roleService.DeleteRoleAsync(id);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }

        [HttpGet("permissions")]
        public async Task<ActionResult> GetAllPermissions()
        {
            var serviceResponse = await _roleService.GetAllPermissionsAsync();
            return Ok(serviceResponse.getData());
        }

        [HttpPost("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult> AssignRoleToUser(int userId, int roleId)
        {
            var serviceResponse = await _roleService.AssignRoleToUserAsync(userId, roleId);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return BadRequest(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }

        [HttpDelete("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult> RemoveRoleFromUser(int userId, int roleId)
        {
            var serviceResponse = await _roleService.RemoveRoleFromUserAsync(userId, roleId);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getMessage());
        }

        [HttpGet("name/{roleName}")]
        public async Task<ActionResult> GetRoleByName(string roleName)
        {
            var serviceResponse = await _roleService.GetRoleByNameAsync(roleName);
            if (serviceResponse.ResponseType != EResponseType.Success)
                return NotFound(serviceResponse.getMessage());

            return Ok(serviceResponse.getData());
        }
    }
} 
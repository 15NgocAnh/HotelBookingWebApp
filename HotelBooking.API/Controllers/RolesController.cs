using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.Domain.DTOs;
using HotelBooking.Domain.Interfaces;

namespace HotelBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(Guid id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto createRoleDto)
        {
            var role = await _roleService.CreateRoleAsync(createRoleDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RoleDto>> UpdateRole(Guid id, UpdateRoleDto updateRoleDto)
        {
            var role = await _roleService.UpdateRoleAsync(id, updateRoleDto);
            if (role == null)
                return NotFound();

            return Ok(role);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(Guid id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("permissions")]
        public async Task<ActionResult<List<PermissionDto>>> GetAllPermissions()
        {
            var permissions = await _roleService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpPost("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult> AssignRoleToUser(Guid userId, Guid roleId)
        {
            var result = await _roleService.AssignRoleToUserAsync(userId, roleId);
            if (!result)
                return BadRequest();

            return NoContent();
        }

        [HttpDelete("users/{userId}/roles/{roleId}")]
        public async Task<ActionResult> RemoveRoleFromUser(Guid userId, Guid roleId)
        {
            var result = await _roleService.RemoveRoleFromUserAsync(userId, roleId);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
} 
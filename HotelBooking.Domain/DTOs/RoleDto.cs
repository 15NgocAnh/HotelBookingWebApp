using System;
using System.Collections.Generic;

namespace HotelBooking.Domain.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class CreateRoleDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<Guid> PermissionIds { get; set; }
    }

    public class PermissionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
    }
} 
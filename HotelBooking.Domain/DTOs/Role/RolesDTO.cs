namespace HotelBooking.Domain.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<PermissionDto> Permissions { get; set; }
    }

    public class UpdateRoleDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int>? PermissionIds { get; set; }
    }

    public class PermissionDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Module { get; set; }
    }
}

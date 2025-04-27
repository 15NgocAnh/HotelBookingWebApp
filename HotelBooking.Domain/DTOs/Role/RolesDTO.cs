using System.ComponentModel;

namespace HotelBooking.Domain.DTOs.Role
{
    public class RoleDto
    {
        public int Id { get; set; }

        [DisplayName("Mã nhóm quyền")]
        public string Code { get; set; }

        [DisplayName("Tên nhóm quyền")]
        public string Name { get; set; }

        [DisplayName("Ghi chú")]
        public string Description { get; set; }

        [DisplayName("Trạng thái")]
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

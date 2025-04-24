namespace HotelBooking.Domain.DTOs.Role
{
    public class CreateRoleDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int>? PermissionIds { get; set; }
    }
}

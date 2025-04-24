using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public RoleDto Role { get; set; }
        public Dictionary<string, List<PermissionDto>> GroupedPermissions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var roleResponse = await GetAsync<RoleDto>($"api/v1/roles/{id}");
            if (roleResponse == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhóm quyền.";
                return RedirectToPage("Index");
            }

            Role = roleResponse;
            if (Role.Permissions != null)
            {
                GroupedPermissions = Role.Permissions
                    .GroupBy(p => p.Module)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }

            return Page();
        }
    }
} 
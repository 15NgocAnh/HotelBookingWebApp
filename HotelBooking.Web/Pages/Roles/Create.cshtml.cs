using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public CreateRoleDto Role { get; set; }

        public Dictionary<string, List<PermissionDto>> GroupedPermissions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var permissionsResponse = await GetAsync<List<PermissionDto>>("api/v1/roles/permissions");
            if (permissionsResponse != null)
            {
                GroupedPermissions = permissionsResponse
                    .GroupBy(p => p.Module)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var permissionsResponse = await GetAsync<List<PermissionDto>>("api/v1/roles/permissions");
                if (permissionsResponse != null)
                {
                    GroupedPermissions = permissionsResponse
                        .GroupBy(p => p.Module)
                        .ToDictionary(g => g.Key, g => g.ToList());
                }
                return Page();
            }

            var response = await PostAsync<CreateRoleDto, RoleDto>("api/v1/roles", Role);
            if (response != null)
            {
                TempData["SuccessMessage"] = "Thêm nhóm quyền thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi thêm nhóm quyền.");
            return Page();
        }
    }
} 
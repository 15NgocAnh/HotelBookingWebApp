using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HotelBooking.Domain.Response.EServiceResponseTypes;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class EditModel : AbstractPageModel
    {
        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UpdateRoleDto Role { get; set; }

        public Dictionary<string, List<PermissionDto>> GroupedPermissions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var roleResponse = await GetAsync<RoleDto>($"api/v1/roles/{id}");
            if (roleResponse == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhóm quyền.";
                return RedirectToPage("Index");
            }

            var permissionsResponse = await GetAsync<List<PermissionDto>>("api/v1/roles/permissions");
            if (permissionsResponse != null)
            {
                GroupedPermissions = permissionsResponse
                    .GroupBy(p => p.Module)
                    .ToDictionary(g => g.Key, g => g.ToList());
            }

            Role = new UpdateRoleDto
            {
                Id = roleResponse.Id,
                Code = roleResponse.Code,
                Name = roleResponse.Name,
                Description = roleResponse.Description,
                IsActive = roleResponse.IsActive,
                PermissionIds = roleResponse.Permissions?.Select(p => p.Id).ToList() ?? new List<int>()
            };

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

            var response = await PutAsync<UpdateRoleDto, RoleDto>($"api/v1/roles/{Role.Id}", Role);
            if (response != null)
            {
                TempData["SuccessMessage"] = "Cập nhật nhóm quyền thành công!";
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi cập nhật nhóm quyền.");
            return Page();
        }
    }
} 
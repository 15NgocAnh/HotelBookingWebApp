using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Role;
using HotelBooking.Domain.Filtering;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoleDto> Roles { get; private set; } = new();

        public async Task OnGetAsync()
        {
            var apiResponse = await GetAsync<List<RoleDto>>($"api/v1/roles");
            Roles = apiResponse ?? new List<RoleDto>();
        }

        public async Task<IActionResult> OnPostDelete(string roleId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/roles/{roleId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa nhóm quyền thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Xóa nhóm quyền thất bại: {errorMessage}";
                }
                return Redirect("/Roles");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa nhóm quyền: {ex.Message}";
                return Redirect("/Roles");
            }
        }
    }
} 
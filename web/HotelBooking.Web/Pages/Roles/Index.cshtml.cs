using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = "SuperAdmin")]
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<RoleDto> Roles { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<List<RoleDto>>("api/role");
                if (result == null)
                {
                    ErrorMessage = "Failed to fetch roles.";
                    return Page();
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Roles = result.Data;
                }
                else
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch roles.";
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roles");
                ErrorMessage = "An error occurred while fetching roles. Please try again later.";
                return Page();
            }
        }

        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/role/{id}");
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Role deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete role.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting role {RoleId}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the role. Please try again later.";
            }

            return RedirectToPage();
        }
    }
} 
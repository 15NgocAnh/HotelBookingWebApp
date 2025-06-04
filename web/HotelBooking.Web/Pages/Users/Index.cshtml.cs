using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Users
{
    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<UserDto> Users { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<List<UserDto>>("api/user");
                if (result == null)
                {
                    ErrorMessage = "Failed to fetch users.";
                    return Page();
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Users = result.Data;
                }
                else
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch users.";
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                ErrorMessage = "An error occurred while fetching users. Please try again later.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/user/{id}");
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "User deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete user.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                TempData["ErrorMessage"] = "An error occurred while deleting the user.";
            }
            return RedirectToPage();
        }
    }
} 
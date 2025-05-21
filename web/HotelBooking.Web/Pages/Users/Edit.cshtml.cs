using HotelBooking.Application.CQRS.User.Commands.UpdateUser;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Role.DTOs;

namespace HotelBooking.Web.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class EditModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<EditModel> _logger;

        public EditModel(IApiService apiService, ILogger<EditModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public UpdateUserCommand UserInput { get; set; } = new();

        [BindProperty]
        public List<RoleDto> Roles { get; set; } = new();

        [BindProperty]
        public List<HotelDto> Hotels { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
                var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");

                if (hotelsResult == null || rolesResult == null)
                {
                    TempData["ErrorMessage"] = "Failed to fetch data for user update.";
                    return RedirectToPage("./Index");
                }

                if (hotelsResult.IsSuccess && hotelsResult.Data != null &&
                    rolesResult.IsSuccess && rolesResult.Data != null)
                {
                    Hotels = hotelsResult.Data;
                    Roles = rolesResult.Data;
                }

                var result = await _apiService.GetAsync<UserDto>($"api/user/{id}");
                if (result == null)
                {
                    return NotFound();
                }

                if (!result.IsSuccess || result.Data == null)
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch user.";
                    return Page();
                }

                var user = result.Data;
                UserInput = new UpdateUserCommand
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Phone = user.Phone,
                    RoleId = user.Role.Id,
                    HotelIds = user.Hotels.Select(h => h.Id).ToList()
                };

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user");
                ErrorMessage = "An error occurred while fetching the user.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");

            if (hotelsResult == null || rolesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for user update.";
                return RedirectToPage("./Index");
            }

            if (hotelsResult.IsSuccess && hotelsResult.Data != null &&
                rolesResult.IsSuccess && rolesResult.Data != null)
            {
                Hotels = hotelsResult.Data;
                Roles = rolesResult.Data;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PutAsync<Result>($"api/user/{UserInput.Id}", UserInput);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update user.");
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "User updated successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    foreach (var error in result.Messages)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the user.");
                return Page();
            }
        }
    }
} 
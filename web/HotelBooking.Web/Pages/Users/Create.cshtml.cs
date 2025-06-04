using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Application.CQRS.User.Commands.CreateUser;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelBooking.Domain.Utils.Enum;
using System.Security.Claims;

namespace HotelBooking.Web.Pages.Users
{
    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public class CreateModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(
            IApiService apiService, 
            ILogger<CreateModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public CreateUserCommand UserInput { get; set; } = new();

        [BindProperty]
        public List<RoleDto> Roles { get; set; } = new();

        [BindProperty]
        public List<HotelDto> Hotels { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public bool IsSuperAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                IsSuperAdmin = User.IsInRole(Role.SuperAdmin.ToString());

                // Get hotels based on user role
                if (IsSuperAdmin)
                {
                    var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
                    if (hotelsResult?.Data != null)
                    {
                        Hotels = hotelsResult.Data;
                    }
                }
                else
                {
                    // For hotel manager, only get their managed hotels
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var userHotelsResult = await _apiService.GetAsync<List<HotelDto>>($"api/user/{userId}/hotels");
                    if (userHotelsResult?.Data != null)
                    {
                        Hotels = userHotelsResult.Data;
                    }
                }

                // Get roles based on user role
                var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");
                if (rolesResult?.Data != null)
                {
                    if (IsSuperAdmin)
                    {
                        Roles = rolesResult.Data;
                    }
                    else
                    {
                        // For hotel manager, only show frontdesk role
                        Roles = rolesResult.Data.Where(r => r.Id == (int)Role.FrontDesk).ToList();
                        UserInput.RoleId = (int)Role.FrontDesk;
                    }
                }

                if (Hotels.Count == 0 || Roles.Count == 0)
                {
                    TempData["ErrorMessage"] = "Failed to fetch data for user creation.";
                    return RedirectToPage("./Index");
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data for user creation");
                ErrorMessage = "An error occurred while fetching data.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Get current user's role
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var currentUserResult = await _apiService.GetAsync<UserDto>($"api/user/{userId}");
                if (currentUserResult?.Data == null)
                {
                    TempData["ErrorMessage"] = "Failed to fetch current user data.";
                    return RedirectToPage("./Index");
                }

                IsSuperAdmin = User.IsInRole(Role.SuperAdmin.ToString());

                // Validate role assignment
                if (!IsSuperAdmin && UserInput.RoleId != (int)Role.FrontDesk)
                {
                    UserInput.RoleId = (int)Role.FrontDesk;
                }

                // Validate hotel assignment for hotel managers
                if (!IsSuperAdmin)
                {
                    var userHotelsResult = await _apiService.GetAsync<List<HotelDto>>($"api/user/{userId}/hotels");
                    if (userHotelsResult?.Data != null)
                    {
                        var allowedHotelIds = userHotelsResult.Data.Select(h => h.Id).ToList();
                        var invalidHotelIds = UserInput.HotelIds.Except(allowedHotelIds).ToList();
                        
                        if (invalidHotelIds.Any())
                        {
                            ModelState.AddModelError(string.Empty, "You can only assign users to hotels you manage.");
                            await LoadDataAsync();
                            return Page();
                        }
                    }
                }

                if (!ModelState.IsValid)
                {
                    await LoadDataAsync();
                    return Page();
                }

                var result = await _apiService.PostAsync<int>("api/user", UserInput);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create user.");
                    await LoadDataAsync();
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "User created successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    foreach (var error in result.Messages)
                    {
                        ModelState.AddModelError(string.Empty, error.Message);
                    }
                    await LoadDataAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                await LoadDataAsync();
                return Page();
            }
        }

        private async Task LoadDataAsync()
        {
            if (IsSuperAdmin)
            {
                var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
                if (hotelsResult?.Data != null)
                {
                    Hotels = hotelsResult.Data;
                }
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var userHotelsResult = await _apiService.GetAsync<List<HotelDto>>($"api/user/{userId}/hotels");
                if (userHotelsResult?.Data != null)
                {
                    Hotels = userHotelsResult.Data;
                }
            }

            var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");
            if (rolesResult?.Data != null)
            {
                if (IsSuperAdmin)
                {
                    Roles = rolesResult.Data;
                }
                else
                {
                    Roles = rolesResult.Data.Where(r => r.Id == (int)Role.FrontDesk).ToList();
                }
            }
        }
    }
} 
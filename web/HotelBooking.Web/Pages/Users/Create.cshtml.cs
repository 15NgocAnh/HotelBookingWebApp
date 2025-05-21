using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Application.CQRS.User.Commands.CreateUser;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Users
{
    [Authorize(Roles = "SuperAdmin")]
    public class CreateModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(IApiService apiService, ILogger<CreateModel> logger)
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

        public async Task<IActionResult> OnGetAsync()
        {
            var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");

            if (hotelsResult == null || rolesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for user creation.";
                return RedirectToPage("./Index");
            }

            if (hotelsResult.IsSuccess && hotelsResult.Data != null &&
                rolesResult.IsSuccess && rolesResult.Data != null)
            {
                Hotels = hotelsResult.Data;
                Roles = rolesResult.Data;
                return Page();
            }

            var errorMessage = hotelsResult.Messages.FirstOrDefault()?.Message ??
                             rolesResult.Messages.FirstOrDefault()?.Message ??
                             "Failed to fetch data for user creation.";
            TempData["ErrorMessage"] = errorMessage;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            var rolesResult = await _apiService.GetAsync<List<RoleDto>>("api/role");

            if (hotelsResult != null && hotelsResult.IsSuccess && hotelsResult.Data != null)
            {
                Hotels = hotelsResult.Data;
            }
            if (rolesResult != null && rolesResult.IsSuccess && rolesResult.Data != null)
            {
                Roles = rolesResult.Data;
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PostAsync<int>("api/user", UserInput);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create user.");
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
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
                return Page();
            }
        }
    }
} 
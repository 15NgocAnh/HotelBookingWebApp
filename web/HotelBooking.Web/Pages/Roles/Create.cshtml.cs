using HotelBooking.Application.CQRS.Role.Commands.CreateRole;
using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Roles
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
        public CreateRoleCommand RoleInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PostAsync<int>("api/role", RoleInput);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create role.");
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Role created successfully!";
                    return RedirectToPage("./Index");
                }
                
                foreach (var message in result.Messages)
                {
                    ModelState.AddModelError(string.Empty, message.Message);
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the role.");
                return Page();
            }
        }
    }
} 
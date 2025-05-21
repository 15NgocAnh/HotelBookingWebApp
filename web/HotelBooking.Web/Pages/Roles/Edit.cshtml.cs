using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Role.Commands.UpdateRole;
using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Roles
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
        public UpdateRoleCommand RoleInput { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var result = await _apiService.GetAsync<RoleDto>($"api/role/{id}");
                if (result == null)
                {
                    return NotFound();
                }

                if (result.IsSuccess && result.Data != null)
                {
                    RoleInput = new UpdateRoleCommand
                    {
                        Id = result.Data.Id,
                        Name = result.Data.Name,
                        Description = result.Data.Description
                    };
                    return Page();
                }
                else
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch role.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role");
                ErrorMessage = "An error occurred while fetching the role.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PutAsync<Result>($"api/role/{RoleInput.Id}", RoleInput);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update role.");
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Role updated successfully!";
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
                _logger.LogError(ex, "Error updating role");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the role.");
                return Page();
            }
        }
    }
} 
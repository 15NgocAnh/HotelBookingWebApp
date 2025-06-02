using HotelBooking.Application.CQRS.Role.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Roles
{
    [Authorize(Roles = "SuperAdmin")]
    public class DetailModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<DetailModel> _logger;

        public DetailModel(IApiService apiService, ILogger<DetailModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public RoleDto Role { get; set; } = new();
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
                    Role = result.Data;
                    return Page();
                }
                else
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch role details.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching role details");
                ErrorMessage = "An error occurred while fetching role details.";
                return Page();
            }
        }
    }
} 
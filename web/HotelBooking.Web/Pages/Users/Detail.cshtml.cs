using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.User.DTOs;
using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Users
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

        public UserDto User { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var result = await _apiService.GetAsync<UserDto>($"api/user/{id}");
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to fetch user.";
                    return RedirectToPage("./Index");
                }

                if (result.IsSuccess && result.Data != null)
                {
                    User = result.Data;
                    return Page();
                }

                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch user.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user details");
                ErrorMessage = "An error occurred while fetching user details.";
                return Page();
            }
        }
    }
} 
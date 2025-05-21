using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Amenities
{
    public class IndexModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public List<AmenityDto> Amenities { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var result = await _apiService.GetAsync<List<AmenityDto>>("api/amenity");
                if (result == null)
                {
                    ErrorMessage = "Failed to fetch amenities.";
                    return Page();
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Amenities = result.Data;
                }
                else
                {
                    ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch amenities.";
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching amenities");
                ErrorMessage = "An error occurred while fetching amenities. Please try again later.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/amenity/{id}");
                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Amenity deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete amenity.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting amenity");
                TempData["ErrorMessage"] = "An error occurred while deleting the amenity.";
            }
            return RedirectToPage();
        }
    }
} 
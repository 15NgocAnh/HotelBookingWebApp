using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Domain.AggregateModels.ExtraCategoryAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Amenities
{
    public class DetailModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<DetailModel> _logger;

        public DetailModel(IApiService apiService, ILogger<DetailModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public AmenityDto Amenity { get; set; } = new();
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var result = await _apiService.GetAsync<AmenityDto>($"api/amenity/{id}");
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to fetch amenity.";
                    return RedirectToPage("./Index");
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Amenity = result.Data;
                    return Page();
                }

                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch amenity.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching amenity details");
                ErrorMessage = "An error occurred while fetching the amenity details.";
                return Page();
            }
        }
    }
} 
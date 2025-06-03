using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Hotels;

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

    public List<HotelDto> Hotels { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<HotelDto>>("api/hotel") ?? new();
            if (result == null)
            {
                ErrorMessage = "Failed to fetch hotels.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Hotels = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotels.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching hotels");
            ErrorMessage = "An error occurred while fetching hotels. Please try again later.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/hotel/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Hotel deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete hotel.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting hotel");
            TempData["ErrorMessage"] = "An error occurred while deleting the hotel.";
        }
        return RedirectToPage();
    }
} 
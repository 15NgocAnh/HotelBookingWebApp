using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Hotels;

public class DetailsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IApiService apiService, ILogger<DetailsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public HotelDto Hotel { get; set; }
    public List<BuildingDto> Buildings { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<HotelDto>($"api/hotel/{id}");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch hotel.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Hotel = result.Data;

                var resultBuildings = await _apiService.GetAsync<List<BuildingDto>>($"api/building/hotel/{Hotel.Id}");
                if (resultBuildings == null)
                {
                    ErrorMessage = "Failed to fetch buildings.";
                    return Page();
                }

                if (resultBuildings.IsSuccess && resultBuildings.Data != null)
                {
                    Buildings = resultBuildings.Data;
                }
                else
                {
                    ErrorMessage = resultBuildings.Messages.FirstOrDefault()?.Message ?? "Failed to fetch buildings.";
                }
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotel.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting hotel with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the hotel.";
            return RedirectToPage("./Index");
        }
    }


    public async Task<IActionResult> OnPostDeleteBuildingAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/building/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Building deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete building.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting building");
            TempData["ErrorMessage"] = "An error occurred while deleting the building.";
        }
        return RedirectToPage();
    }
} 
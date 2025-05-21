using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Buildings;

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
    public List<HotelDto> Hotels { get; set; }

    [BindProperty]
    public BuildingDto Building { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var hotelsResult = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            if (hotelsResult == null || !hotelsResult.IsSuccess || hotelsResult.Data == null)
            {
                TempData["ErrorMessage"] = hotelsResult?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotels.";
                return RedirectToPage("./Index");
            }
            Hotels = hotelsResult.Data;

            var buildingResult = await _apiService.GetAsync<BuildingDto>($"api/building/{id}");
            if (buildingResult == null)
            {
                return NotFound();
            }

            if (buildingResult.IsSuccess && buildingResult.Data != null)
            {
                Building = buildingResult.Data;
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = buildingResult.Messages.FirstOrDefault()?.Message ?? "Failed to fetch building.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching building");
            TempData["ErrorMessage"] = "An error occurred while fetching the building.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var result = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            if (result != null && result.IsSuccess && result.Data != null)
            {
                Hotels = result.Data;
            }
            return Page();
        }

        try
        {
            var result = await _apiService.PutAsync<Result>($"api/building/{Building.Id}", Building);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update building.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Building updated successfully!";
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
            _logger.LogError(ex, "Error updating building");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the building.");
            return Page();
        }
    }
} 
 
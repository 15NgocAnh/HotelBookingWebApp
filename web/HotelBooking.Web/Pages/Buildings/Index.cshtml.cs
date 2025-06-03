using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Buildings;

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

    public List<BuildingDto> Buildings { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<BuildingDto>>("api/building") ?? new();
            if (result == null)
            {
                ErrorMessage = "Failed to fetch buildings.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Buildings = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch buildings.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching buildings");
            ErrorMessage = "An error occurred while fetching buildings. Please try again later.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
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
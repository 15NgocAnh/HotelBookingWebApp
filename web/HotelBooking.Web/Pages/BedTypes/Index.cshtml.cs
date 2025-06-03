using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.BedTypes;

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

    public List<BedTypeDto> BedTypes { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<BedTypeDto>>("api/bedtype");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch bed types.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                BedTypes = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bed types.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching bed types");
            ErrorMessage = "An error occurred while fetching bed types. Please try again later.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/bedtype/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Bed type deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete bed type.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting bed type {BedTypeId}", id);
            TempData["ErrorMessage"] = "An error occurred while deleting the bed type. Please try again later.";
        }

        return RedirectToPage();
    }
} 
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.ExtraItems;

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

    public List<ExtraItemDto> ExtraItems { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<ExtraItemDto>>("api/extraitem") ?? new();
            if (result == null)
            {
                ErrorMessage = "Failed to fetch extra items.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                ExtraItems = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra items.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching extra items");
            ErrorMessage = "An error occurred while fetching extra items. Please try again later.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/extraitem/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra item deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete extra item.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting extra item");
            TempData["ErrorMessage"] = "An error occurred while deleting the extra item.";
        }
        return RedirectToPage();
    }
} 
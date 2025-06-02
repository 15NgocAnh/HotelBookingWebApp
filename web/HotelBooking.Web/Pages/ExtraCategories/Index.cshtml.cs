using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.ExtraCategories;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<ExtraCategoryDto> ExtraCategories { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<ExtraCategoryDto>>("api/extracategory");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch extra categories.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                ExtraCategories = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra categories.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching extra categories");
            ErrorMessage = "An error occurred while fetching extra categories. Please try again later.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(string id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/extracategory/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra category deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete extra category.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting extra category");
            TempData["ErrorMessage"] = "An error occurred while deleting the extra category.";
        }
        return RedirectToPage();
    }
} 
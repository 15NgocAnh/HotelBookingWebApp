using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.ExtraItems;

public class DetailsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IApiService apiService, ILogger<DetailsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public ExtraItemDto ExtraItem { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<ExtraItemDto>($"api/extraitem/{id}");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch extra item.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                ExtraItem = result.Data;
                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra item.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting extra item with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the extra item.";
            return RedirectToPage("./Index");
        }
    }
} 
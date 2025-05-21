using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.RoomTypes;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<RoomTypeDto> RoomTypes { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch room types.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                RoomTypes = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room types.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching room types");
            ErrorMessage = "An error occurred while fetching room types. Please try again later.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/roomtype/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room type deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete room type.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting room type");
            TempData["ErrorMessage"] = "An error occurred while deleting the room type.";
        }
        return RedirectToPage();
    }
}

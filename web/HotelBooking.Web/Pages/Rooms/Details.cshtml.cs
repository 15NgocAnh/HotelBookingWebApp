using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Rooms;

public class DetailsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IApiService apiService, ILogger<DetailsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [BindProperty]
    public RoomDto Room { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            TempData["ErrorMessage"] = "Failed to fetch room.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }

        try
        {
            var result = await _apiService.GetAsync<RoomDto>($"api/room/{id}");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch room.";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                Room = result.Data;
                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting room with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the room.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
    }
} 
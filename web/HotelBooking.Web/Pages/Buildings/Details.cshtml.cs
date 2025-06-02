using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Buildings;

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
    public BuildingDto Building { get; set; } = new();

    [BindProperty]
    public List<RoomDto> Rooms { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    public string? ErrorMessage { get; set; }

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
            var result = await _apiService.GetAsync<BuildingDto>($"api/building/{id}");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch building.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Building = result.Data;

                var resultRooms = await _apiService.GetAsync<List<RoomDto>>($"api/room/building/{Building.Id}");
                if (resultRooms == null)
                {
                    ErrorMessage = "Failed to fetch room.";
                    return Page();
                }

                if (resultRooms.IsSuccess && resultRooms.Data != null)
                {
                    Rooms = resultRooms.Data;
                }
                else
                {
                    ErrorMessage = resultRooms.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room.";
                }
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room.";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting building with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the building.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostDeleteRoomAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/room/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete room.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting room");
            TempData["ErrorMessage"] = "An error occurred while deleting the room.";
        }
        return RedirectToPage();
    }
} 
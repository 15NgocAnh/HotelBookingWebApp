using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HotelBooking.Application.CQRS.Room.Commands;
using HotelBooking.Domain.AggregateModels.RoomAggregate;

namespace HotelBooking.Web.Pages.Rooms;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<RoomDto> Rooms { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<RoomDto>>("api/room") ?? new();
            if (result == null)
            {
                ErrorMessage = "Failed to fetch rooms.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Rooms = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch rooms.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching rooms");
            ErrorMessage = "An error occurred while fetching rooms. Please try again later.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin,HotelManager")]
    public async Task<IActionResult> OnPostDeleteAsync(int id)
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

    public async Task<IActionResult> OnPostChangeStatusAsync(int id, string status)
    {
        try
        {
            var updateRoomStatus = new ChangeRoomStatusCommand()
            {
                Id = id,
                Status = Enum.Parse<RoomStatus>(status, ignoreCase: true)
            };
            var result = await _apiService.PutAsync<Result>($"api/room/{id}/status", updateRoomStatus);
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room status updated successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to update room status.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating room status");
            TempData["ErrorMessage"] = "An error occurred while updating room status.";
        }
        return RedirectToPage();
    }
} 
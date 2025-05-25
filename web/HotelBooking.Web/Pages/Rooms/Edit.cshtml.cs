using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.Commands;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Rooms;

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
    public RoomDto Room { get; set; } = new();

    [BindProperty]
    public List<RoomTypeDto> RoomTypes { get; set; } = new();

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
            var roomResult = await _apiService.GetAsync<RoomDto>($"api/room/{id}");
            var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");

            if (roomResult == null || roomTypesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for room edit.";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
            }

            if (roomResult.IsSuccess && roomResult.Data != null)
            {
                Room = roomResult.Data;
            }
            else
            {
                TempData["ErrorMessage"] = roomResult.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room.";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
            }

            if (roomTypesResult.IsSuccess && roomTypesResult.Data != null)
            {
                RoomTypes = roomTypesResult.Data;
            }

            return Page();
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

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");

            if (roomTypesResult != null && roomTypesResult.IsSuccess && roomTypesResult.Data != null)
            {
                RoomTypes = roomTypesResult.Data;
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var updateRoom = new UpdateRoomCommand()
            {
                Id = Room.Id,
                Name = Room.Name,
                FloorId = Room.FloorId,
                RoomTypeId = Room.RoomTypeId
            };

            var result = await _apiService.PutAsync<Result>($"api/room/{Room.Id}", updateRoom);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update room.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room updated successfully.";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
            }

            foreach (var message in result.Messages)
            {
                ModelState.AddModelError(string.Empty, message.Message);
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating room with ID {Id}", Room.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the room.");
            return Page();
        }
    }
}
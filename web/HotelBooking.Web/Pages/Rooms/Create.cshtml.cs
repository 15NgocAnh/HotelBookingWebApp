using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Room.Commands;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Rooms;

public class CreateModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IApiService apiService, ILogger<CreateModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [BindProperty]
    public CreateRoomCommand Room { get; set; }

    [BindProperty]
    public List<FloorDto> Floors { get; set; } = new();

    [BindProperty]
    public List<RoomTypeDto> RoomTypes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var floorsResult = await _apiService.GetAsync<List<FloorDto>>("api/building/floor");
            var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");

            if (floorsResult == null || roomTypesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for room creation.";
                return RedirectToPage("./Index");
            }

            if (floorsResult.IsSuccess && floorsResult.Data != null &&
                roomTypesResult.IsSuccess && roomTypesResult.Data != null)
            {
                Floors = floorsResult.Data;
                RoomTypes = roomTypesResult.Data;
                return Page();
            }

            var errorMessage = floorsResult.Messages.FirstOrDefault()?.Message ?? 
                             roomTypesResult.Messages.FirstOrDefault()?.Message ?? 
                             "Failed to fetch data for room creation.";
            TempData["ErrorMessage"] = errorMessage;
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var floorsResult = await _apiService.GetAsync<List<FloorDto>>("api/floor");
            var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");

            if (floorsResult != null && floorsResult.IsSuccess && floorsResult.Data != null)
            {
                Floors = floorsResult.Data;
            }
            if (roomTypesResult != null && roomTypesResult.IsSuccess && roomTypesResult.Data != null)
            {
                RoomTypes = roomTypesResult.Data;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return RedirectToPage("./Index");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/room", Room);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create room.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room created successfully!";
                return RedirectToPage("./Index");
            }
            
            foreach (var message in result.Messages)
            {
                ModelState.AddModelError(string.Empty, message.Message);
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating room");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the room.");
            return Page();
        }
    }
} 
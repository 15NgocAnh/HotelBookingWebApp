using HotelBooking.Application.CQRS.Booking.Commands.CreateBooking;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Bookings;

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
    public CreateBookingCommand Booking { get; set; } = new();

    public List<RoomDto> AvailableRooms { get; set; } = new();
    public List<ExtraItemDto> AvailableExtraItems { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            // Get available rooms
            var roomsResult = await _apiService.GetAsync<List<RoomDto>>("api/room/available");
            if (roomsResult == null || !roomsResult.IsSuccess || roomsResult.Data == null)
            {
                TempData["ErrorMessage"] = roomsResult?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch available rooms.";
                return RedirectToPage("./Index");
            }

            // Get available extra items
            var extraItemsResult = await _apiService.GetAsync<List<ExtraItemDto>>("api/extraitem");
            if (extraItemsResult == null || !extraItemsResult.IsSuccess || extraItemsResult.Data == null)
            {
                TempData["ErrorMessage"] = extraItemsResult?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra items.";
                return RedirectToPage("./Index");
            }

            AvailableRooms = roomsResult.Data;
            AvailableExtraItems = extraItemsResult.Data;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading create booking page");
            TempData["ErrorMessage"] = "An error occurred while loading the page.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            // Reload available rooms and extra items
            var roomsResult = await _apiService.GetAsync<List<RoomDto>>("api/room/available");
            if (roomsResult?.IsSuccess == true && roomsResult.Data != null)
            {
                AvailableRooms = roomsResult.Data;
            }

            var extraItemsResult = await _apiService.GetAsync<List<ExtraItemDto>>("api/extraitem");
            if (extraItemsResult?.IsSuccess == true && extraItemsResult.Data != null)
            {
                AvailableExtraItems = extraItemsResult.Data;
            }

            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/booking", Booking);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to create booking.";
                return Page();
            }

            TempData["SuccessMessage"] = "Booking created successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            TempData["ErrorMessage"] = "An error occurred while creating the booking.";
            return Page();
        }
    }
} 
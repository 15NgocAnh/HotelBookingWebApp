using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBooking;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Bookings;

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
    public UpdateBookingCommand Booking { get; set; } = new();

    [BindProperty]
    public List<RoomDto> AvailableRooms { get; set; } = new();

    [BindProperty]
    public List<ExtraItemDto> AvailableExtraItems { get; set; } = new();

    [BindProperty]
    public BookingStatus CurrentStatus { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            // Get booking details
            var bookingResult = await _apiService.GetAsync<BookingDto>($"api/booking/{id}");
            if (bookingResult == null || !bookingResult.IsSuccess || bookingResult.Data == null)
            {
                TempData["ErrorMessage"] = bookingResult?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch booking details.";
                return RedirectToPage("./Index");
            }

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

            var booking = bookingResult.Data;
            CurrentStatus = booking.Status;

            // Map booking data to command
            Booking = new UpdateBookingCommand
            {
                Id = booking.Id,
                RoomId = booking.RoomId,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                Notes = booking.Notes,
                Guests = booking.Guests.Select(g => new GuestDto
                {
                    Id = g.Id,
                    FirstName = g.FirstName,
                    LastName = g.LastName,
                    PhoneNumber = g.PhoneNumber,
                    CitizenIdNumber = g.CitizenIdNumber,
                    PassportNumber = g.PassportNumber
                }).ToList(),
                ExtraUsages = booking.ExtraUsages.Select(e => new ExtraUsageDto
                {
                    Id = e.Id,
                    ExtraItemId = e.ExtraItemId,
                    Quantity = e.Quantity
                }).ToList()
            };

            AvailableRooms = roomsResult.Data;
            AvailableExtraItems = extraItemsResult.Data;

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading edit booking page");
            TempData["ErrorMessage"] = "An error occurred while loading the page.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostEditAsync()
    {
        if (!ModelState.IsValid)
        {
            await ReloadDataAsync();
            return Page();
        }

        try
        {
            // Get current booking status
            var bookingResult = await _apiService.GetAsync<BookingDto>($"api/booking/{Booking.Id}");
            if (bookingResult == null || !bookingResult.IsSuccess || bookingResult.Data == null)
            {
                TempData["ErrorMessage"] = "Failed to verify booking status.";
                return RedirectToPage("./Index");
            }

            var currentStatus = bookingResult.Data.Status;

            // Validate status for full edit
            if (currentStatus != BookingStatus.Pending && currentStatus != BookingStatus.Confirmed)
            {
                TempData["ErrorMessage"] = "Only pending or confirmed bookings can be fully edited.";
                return RedirectToPage("./Index");
            }

            // Ensure check-in time is not changed if it exists
            if (bookingResult.Data.CheckInTime.HasValue)
            {
                Booking.CheckInDate = bookingResult.Data.CheckInDate;
            }

            var result = await _apiService.PutAsync<Result>($"api/booking/{Booking.Id}", Booking);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to update booking.";
                await ReloadDataAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Booking updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking");
            TempData["ErrorMessage"] = "An error occurred while updating the booking.";
            await ReloadDataAsync();
            return Page();
        }
    }

    public async Task<IActionResult> OnPostAddServicesAsync()
    {
        if (!ModelState.IsValid)
        {
            await ReloadDataAsync();
            return Page();
        }

        try
        {
            // Get current booking status
            var bookingResult = await _apiService.GetAsync<BookingDto>($"api/booking/{Booking.Id}");
            if (bookingResult == null || !bookingResult.IsSuccess || bookingResult.Data == null)
            {
                TempData["ErrorMessage"] = "Failed to verify booking status.";
                return RedirectToPage("./Index");
            }

            var currentStatus = bookingResult.Data.Status;

            // Validate status for adding services
            if (currentStatus != BookingStatus.CheckedIn)
            {
                TempData["ErrorMessage"] = "Services can only be added to checked-in bookings.";
                return RedirectToPage("./Index");
            }

            // Create a limited update command with only services and notes
            var limitedUpdate = new UpdateBookingCommand
            {
                Id = Booking.Id,
                ExtraUsages = Booking.ExtraUsages
            };

            var result = await _apiService.PutAsync<Result>($"api/booking/{Booking.Id}", limitedUpdate);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to update booking services.";
                await ReloadDataAsync();
                return Page();
            }

            TempData["SuccessMessage"] = "Booking services updated successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking services");
            TempData["ErrorMessage"] = "An error occurred while updating the booking services.";
            await ReloadDataAsync();
            return Page();
        }
    }

    private async Task ReloadDataAsync()
    {
        // Reload available rooms
        var roomsResult = await _apiService.GetAsync<List<RoomDto>>("api/room/available");
        if (roomsResult?.IsSuccess == true && roomsResult.Data != null)
        {
            AvailableRooms = roomsResult.Data;
        }

        // Reload available extra items
        var extraItemsResult = await _apiService.GetAsync<List<ExtraItemDto>>("api/extraitem");
        if (extraItemsResult?.IsSuccess == true && extraItemsResult.Data != null)
        {
            AvailableExtraItems = extraItemsResult.Data;
        }
    }
} 
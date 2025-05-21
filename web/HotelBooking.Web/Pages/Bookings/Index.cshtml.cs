using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Bookings;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<BookingDto> Bookings { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<BookingDto>>("api/booking");
            if (result == null)
            {
                ErrorMessage = "Failed to fetch bookings.";
                return Page();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Bookings = result.Data;
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching bookings");
            ErrorMessage = "An error occurred while fetching bookings. Please try again later.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/booking/{id}");
            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Booking deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to delete booking.";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking");
            TempData["ErrorMessage"] = "An error occurred while deleting the booking.";
        }
        return RedirectToPage();
    }
} 
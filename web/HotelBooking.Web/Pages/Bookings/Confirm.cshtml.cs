using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HotelBooking.Web.Services;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus;

namespace HotelBooking.Web.Pages.Bookings;

public class ConfirmModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<ConfirmModel> _logger;

    public ConfirmModel(IApiService apiService, ILogger<ConfirmModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [BindProperty]
    public BookingDto Booking { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<BookingDto>($"api/booking/{id}");
            if (result == null || !result.IsSuccess || result.Data == null)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch booking details.";
                return RedirectToPage("./Index");
            }

            Booking = result.Data;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching booking details");
            TempData["ErrorMessage"] = "An error occurred while loading booking details.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostConfirmAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var updateBooking = new UpdateBookingStatusCommand()
            {
                Id = Booking.Id,
                Status = Domain.AggregateModels.BookingAggregate.BookingStatus.Confirmed
            };
            var result = await _apiService.PutAsync<Result>($"api/booking/{Booking.Id}/status", updateBooking);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to confirm booking.";
                return Page();
            }

            TempData["SuccessMessage"] = "Booking has been confirmed successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming booking");
            TempData["ErrorMessage"] = "An error occurred while confirming the booking.";
            return Page();
        }
    }
} 
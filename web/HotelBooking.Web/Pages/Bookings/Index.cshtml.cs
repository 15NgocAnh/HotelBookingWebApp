using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HotelBooking.Application.CQRS.Booking.Commands.UpdateBookingStatus;
using HotelBooking.Domain.AggregateModels.BookingAggregate;
using HotelBooking.Application.CQRS.Booking.Commands.CheckOut;
using HotelBooking.Application.CQRS.Booking.Commands.CheckIn;
using HotelBooking.Application.CQRS.Invoice.Commands.CreateInvoice;

namespace HotelBooking.Web.Pages.Bookings;

[Authorize(Roles = "SuperAdmin,HotelManager,FrontDesk")]
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
            var result = await _apiService.GetAsync<List<BookingDto>>("api/booking") ?? new();
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

    [Authorize(Roles = "SuperAdmin,HotelManager")]
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

    public async Task<IActionResult> OnPostCheckInAsync(int id, string notes)
    {
        try
        {
            var resultBookings = await _apiService.GetAsync<List<BookingDto>>("api/booking");
            if (resultBookings == null)
            {
                ErrorMessage = "Failed to fetch bookings.";
            }

            if (resultBookings.IsSuccess && resultBookings.Data != null)
            {
                Bookings = resultBookings.Data;
            }
            else
            {
                ErrorMessage = resultBookings.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
            }

            var checkInCommand = new CheckInCommand()
            {
                Id = id,
                Notes = notes
            };
            var result = await _apiService.PutAsync<Result>($"api/booking/{id}/checkin", checkInCommand);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to checkin booking.";
                return Page();
        }

            TempData["SuccessMessage"] = "Booking has been checkin successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checkin booking");
            TempData["ErrorMessage"] = "An error occurred while checkin the booking.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostCheckOutAsync(int id)
    {
        try
        {
            var resultBookings = await _apiService.GetAsync<List<BookingDto>>("api/booking");
            if (resultBookings == null)
            {
                ErrorMessage = "Failed to fetch bookings.";
            }

            if (resultBookings.IsSuccess && resultBookings.Data != null)
            {
                Bookings = resultBookings.Data;
            }
            else
            {
                ErrorMessage = resultBookings.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
            }

            var checkOutCommand = new CheckOutCommand()
            {
                Id = id
            };
            var result = await _apiService.PutAsync<Result>($"api/booking/{id}/checkout", checkOutCommand);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to checkout booking.";
                return Page();
            }

            TempData["SuccessMessage"] = "Booking has been checkout successfully!";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checkin booking");
            TempData["ErrorMessage"] = "An error occurred while checkout the booking.";
            return Page();
        }
    }

    [Authorize(Roles = "SuperAdmin,HotelManager,FrontDesk")]
    public async Task<IActionResult> OnPostConfirmAsync(int id)
    {
        try
        {
            var resultBookings = await _apiService.GetAsync<List<BookingDto>>("api/booking");
            if (resultBookings == null)
            {
                ErrorMessage = "Failed to fetch bookings.";
            }

            if (resultBookings.IsSuccess && resultBookings.Data != null)
            {
                Bookings = resultBookings.Data;
            }
            else
            {
                ErrorMessage = resultBookings.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
            }

            var updateBooking = new UpdateBookingStatusCommand()
            {
                Id = id,
                Status = BookingStatus.Confirmed
            };
            var result = await _apiService.PutAsync<Result>($"api/booking/{id}/status", updateBooking);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to confirm booking.";
                return Page();
            }

            TempData["SuccessMessage"] = "Booking has been confirmed successfully!"; 
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error confirming booking");
            TempData["ErrorMessage"] = "An error occurred while confirming the booking.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostCreateInvoiceAsync(int bookingId, string? notes)
    {
        var resultBookings = await _apiService.GetAsync<List<BookingDto>>("api/booking");
        if (resultBookings == null)
        {
            ErrorMessage = "Failed to fetch bookings.";
            return Page();
        }

        if (resultBookings.IsSuccess && resultBookings.Data != null)
        {
            Bookings = resultBookings.Data;
        }
        else
        {
            ErrorMessage = resultBookings.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var createInvoice = new CreateInvoiceCommand()
            {
                BookingId = bookingId,
                Notes = notes
            };
            var result = await _apiService.PostAsync<int>($"api/invoice", createInvoice);

            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to create invoice booking.";
                return Page();
            }

            TempData["SuccessMessage"] = "Booking has been create invoice successfully!"; 
            return RedirectToPage("/Invoices/Details", new { id = result.Data });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error create invoice booking");
            TempData["ErrorMessage"] = "An error occurred while create invoice the booking.";
            return Page();
        }
    }
} 
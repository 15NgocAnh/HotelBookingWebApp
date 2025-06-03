using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Bookings;

[Authorize(Roles = "SuperAdmin,HotelManager,FrontDesk")]
public class CalendarModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<CalendarModel> _logger;

    public CalendarModel(IApiService apiService, ILogger<CalendarModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<CalendarEvent> BookingEvents { get; set; } = new();
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
                BookingEvents = result.Data.Select(b => new CalendarEvent
                {
                    Id = b.Id.ToString(),
                    Title = $"{b.RoomName} - {string.Join(", ", b.Guests.Select(g => g.LastName))}",
                    Start = b.CheckInDate,
                    End = b.CheckOutDate,
                    ClassName = b.Status.ToString().ToLower(),
                    ExtendedProps = new
                    {
                        id = b.Id,
                        roomName = b.RoomName,
                        roomType = b.RoomTypeName,
                        checkInDate = b.CheckInDate.ToString("MMM dd, yyyy"),
                        checkOutDate = b.CheckOutDate.ToString("MMM dd, yyyy"),
                        status = b.Status.ToString(),
                        guestName = string.Join(", ", b.Guests.Select(g => g.LastName)),
                    }
                }).ToList();
            }
            else
            {
                ErrorMessage = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bookings.";
            }
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching bookings for calendar");
            ErrorMessage = "An error occurred while fetching bookings.";
            return Page();
        }
    }
}

public class CalendarEvent
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public object ExtendedProps { get; set; } = new();
} 
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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
    public string GuestName { get; set; }

    [BindProperty]
    public string GuestEmail { get; set; }

    [BindProperty]
    public string GuestPhone { get; set; }

    [BindProperty]
    public int RoomId { get; set; }

    [BindProperty]
    public DateTime CheckInDate { get; set; }

    [BindProperty]
    public DateTime CheckOutDate { get; set; }

    [BindProperty]
    public string SpecialRequests { get; set; }

    public List<RoomDto> AvailableRooms { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<RoomDto>>("api/room/available");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch available rooms.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                AvailableRooms = result.Data;
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch available rooms.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching available rooms");
            TempData["ErrorMessage"] = "An error occurred while loading available rooms.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            try
            {
                var result = await _apiService.GetAsync<List<RoomDto>>("api/room/available");
                if (result != null && result.IsSuccess && result.Data != null)
                {
                    AvailableRooms = result.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching available rooms");
                TempData["ErrorMessage"] = "An error occurred while loading available rooms.";
                return RedirectToPage("./Index");
            }
            return Page();
        }

        try
        {
            var booking = new BookingDto
            {
                CustomerName = GuestName,
                CustomerEmail = GuestEmail,
                CustomerPhone = GuestPhone,
                RoomId = RoomId,
                CheckInDate = CheckInDate,
                CheckOutDate = CheckOutDate,
                SpecialRequests = SpecialRequests,
                Status = Domain.AggregateModels.BookingAggregate.BookingStatus.Pending
            };

            var result = await _apiService.PostAsync<BookingDto>("api/booking", booking);

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create booking.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Booking created successfully!";
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
            _logger.LogError(ex, "Error creating booking");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the booking.");
            return Page();
        }
    }
} 
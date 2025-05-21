using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Room.DTOs;
using HotelBooking.Domain.AggregateModels.AmenityAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Rooms;

public class DetailsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IApiService apiService, ILogger<DetailsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public RoomDto Room { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<RoomDto>($"api/room/{id}");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch room.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                Room = result.Data;
                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting room with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the room.";
            return RedirectToPage("./Index");
        }
    }
} 
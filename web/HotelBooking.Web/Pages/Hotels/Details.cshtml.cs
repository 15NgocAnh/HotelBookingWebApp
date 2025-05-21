using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Hotels;

public class DetailsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IApiService apiService, ILogger<DetailsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public HotelDto Hotel { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<HotelDto>($"api/hotel/{id}");
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess && result.Data != null)
            {
                Hotel = result.Data;
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotel.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting hotel with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the hotel.";
            return RedirectToPage("./Index");
        }
    }
} 
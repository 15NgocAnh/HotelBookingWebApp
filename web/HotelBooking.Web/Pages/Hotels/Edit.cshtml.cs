using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Hotel.Commands;
using HotelBooking.Application.CQRS.Hotel.Commands.UpdateHotel;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace HotelBooking.Web.Pages.Hotels;

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

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var updateHotel = new UpdateHotelCommand()
            {
                Id = Hotel.Id,
                Name = Hotel.Name,
                Address = Hotel.Address,
                Phone = Hotel.Phone,
                Email = Hotel.Email,
                Description = Hotel.Description
            };

            var result = await _apiService.PutAsync<Result>($"api/hotel/{Hotel.Id}", updateHotel);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update hotel.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Hotel updated successfully.";
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
            _logger.LogError(ex, "Error occurred while updating hotel with ID {Id}", Hotel.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the hotel.");
            return Page();
        }
    }
} 
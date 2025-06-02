using HotelBooking.Application.CQRS.Hotel.Commands;
using HotelBooking.Application.CQRS.Hotel.Commands.CreateHotel;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Hotels;

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
    public CreateHotelCommand Hotel { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/hotel", Hotel);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create hotel.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Hotel created successfully!";
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
            _logger.LogError(ex, "Error creating hotel");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the hotel.");
            return Page();
        }
    }
} 
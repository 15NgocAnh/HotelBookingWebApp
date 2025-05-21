using HotelBooking.Application.CQRS.Building.Commands;
using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Hotel.Queries.GetHotelById;
using HotelBooking.Application.Common.Models;
using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Buildings;

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
    public int HotelId { get; set; }

    [BindProperty]
    public List<HotelDto> Hotels { get; set; }

    [BindProperty]
    public string Name { get; set; }

    [BindProperty]
    public int TotalFloors { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
        if (result == null)
        {
            TempData["ErrorMessage"] = "Failed to fetch hotels.";
            return RedirectToPage("./Index");
        }

        if (result.IsSuccess && result.Data != null)
        {
            Hotels = result.Data;
        }
        else
        {
            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotels.";
            return RedirectToPage("./Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            var result = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
            if (result != null && result.IsSuccess && result.Data != null)
            {
                Hotels = result.Data;
            }
            return Page();
        }

        try
        {
            var building = new CreateBuildingCommand { HotelId = HotelId, Name = Name, TotalFloors = TotalFloors };
            var result = await _apiService.PostAsync<int>("api/building", building);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create building.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Building created successfully!";
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
            _logger.LogError(ex, "Error creating building");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the building.");
            return Page();
        }
    }
} 
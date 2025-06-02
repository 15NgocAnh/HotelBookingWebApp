using HotelBooking.Application.CQRS.Building.Commands;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
    public CreateBuildingCommand Building { get; set; } = new();

    [BindProperty]
    public List<HotelDto> Hotels { get; set; } = new List<HotelDto>();

    [BindProperty(SupportsGet = true)] 
    public string? ReturnUrl { get; set; }

    public bool IsFromHotelContext => !string.IsNullOrEmpty(Building.HotelId.ToString());

    public async Task<IActionResult> OnGetAsync(int? hotelId)
    {
        if (hotelId.HasValue)
            Building.HotelId = hotelId.Value;

        var result = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
        if (result == null)
        {
            TempData["ErrorMessage"] = "Failed to fetch hotels.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }

        if (result.IsSuccess && result.Data != null)
        {
            Hotels = result.Data;
        }
        else
        {
            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch hotels.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var hotels = await _apiService.GetAsync<List<HotelDto>>("api/hotel");
        if (hotels != null && hotels.IsSuccess && hotels.Data != null)
        {
            Hotels = hotels.Data;
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/building", Building);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create building.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Building created successfully!";
                return !string.IsNullOrEmpty(ReturnUrl)
                    ? Redirect(ReturnUrl)
                    : RedirectToPage("./Index");
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
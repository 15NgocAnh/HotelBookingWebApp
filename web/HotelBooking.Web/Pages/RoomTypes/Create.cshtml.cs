using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Application.CQRS.RoomType.Commands;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.RoomTypes;

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
    public CreateRoomTypeCommand RoomType { get; set; }

    [BindProperty]
    public List<AmenityDto> Amenities { get; set; } = new();

    [BindProperty]
    public List<BedTypeDto> BedTypes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var amenitiesResult = await _apiService.GetAsync<List<AmenityDto>>("api/amenity");
            var bedTypesResult = await _apiService.GetAsync<List<BedTypeDto>>("api/bedtype");

            if (amenitiesResult == null || bedTypesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for room type creation.";
                return RedirectToPage("./Index");
            }

            if (amenitiesResult.IsSuccess && amenitiesResult.Data != null &&
                bedTypesResult.IsSuccess && bedTypesResult.Data != null)
            {
                Amenities = amenitiesResult.Data;
                BedTypes = bedTypesResult.Data;
                return Page();
            }

            var errorMessage = amenitiesResult.Messages.FirstOrDefault()?.Message ??
                             bedTypesResult.Messages.FirstOrDefault()?.Message ??
                             "Failed to fetch data for room type creation.";
            TempData["ErrorMessage"] = errorMessage;
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room type creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var amenitiesResult = await _apiService.GetAsync<List<AmenityDto>>("api/amenity");
            var bedTypesResult = await _apiService.GetAsync<List<BedTypeDto>>("api/bedtype");

            if (amenitiesResult == null || bedTypesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for room type creation.";
                return RedirectToPage("./Index");
            }

            if (amenitiesResult.IsSuccess && amenitiesResult.Data != null &&
                bedTypesResult.IsSuccess && bedTypesResult.Data != null)
            {
                Amenities = amenitiesResult.Data;
                BedTypes = bedTypesResult.Data;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room type creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return RedirectToPage("./Index");
        }

        RoomType.AmenitySetupDetails.RemoveAll(a => !a.IsSelected || a.Quantity <= 0);
        RoomType.BedTypeSetupDetails.RemoveAll(b => !b.IsSelected || b.Quantity <= 0);

        ModelState.Clear(); 
        TryValidateModel(RoomType); 

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/roomtype", RoomType);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create room type.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room type created successfully!";
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
            _logger.LogError(ex, "Error creating room type");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the room type.");
            return Page();
        }
    }
} 
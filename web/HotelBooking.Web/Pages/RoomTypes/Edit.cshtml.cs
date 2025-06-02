using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HotelBooking.Application.CQRS.RoomType.Commands;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Application.CQRS.BedType.DTOs;

namespace HotelBooking.Web.Pages.RoomTypes;

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
    public RoomTypeDto RoomType { get; set; }

    [BindProperty]
    public List<AmenityDto> Amenities { get; set; } = new();

    [BindProperty]
    public List<BedTypeDto> BedTypes { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var amenitiesResult = await _apiService.GetAsync<List<AmenityDto>>("api/amenity");
            var bedTypesResult = await _apiService.GetAsync<List<BedTypeDto>>("api/bedtype");

            if (amenitiesResult == null || bedTypesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for room type update.";
                return RedirectToPage("./Index");
            }

            if (amenitiesResult.IsSuccess && amenitiesResult.Data != null &&
                bedTypesResult.IsSuccess && bedTypesResult.Data != null)
            {
                Amenities = amenitiesResult.Data;
                BedTypes = bedTypesResult.Data;
            }

            var result = await _apiService.GetAsync<RoomTypeDto>($"api/roomtype/{id}");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch room type.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                RoomType = result.Data;

                // Ensure BedTypes list exists
                var bedTypeSetupDetails = BedTypes
                    .Select(b => new BedTypeSetupDetailDto { Id = b.Id, BedTypeId = b.Id, BedTypeName = b.Name })
                    .ToList();

                foreach (var bedType in RoomType.BedTypeSetupDetails)
                {
                    var match = bedTypeSetupDetails.FirstOrDefault(b => b.BedTypeId == bedType.BedTypeId);
                    if (match != null)
                    {
                        match.IsSelected = true;
                        match.Quantity = bedType.Quantity;
                    }
                }
                RoomType.BedTypeSetupDetails = bedTypeSetupDetails;


                var amenitySetupDetails = Amenities
                    .Select(a => new AmenitySetupDetailDto { Id = a.Id, AmenityId = a.Id, AmenityName = a.Name })
                    .ToList();

                foreach (var amenity in RoomType.AmenitySetupDetails)
                {
                    var match = amenitySetupDetails.FirstOrDefault(a => a.AmenityId == amenity.AmenityId);
                    if (match != null)
                    {
                        match.IsSelected = true;
                        match.Quantity = amenity.Quantity;
                    }
                }
                RoomType.AmenitySetupDetails = amenitySetupDetails;


                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch room type.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting room type with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the room type.";
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
            var updateRoomType = new UpdateRoomTypeCommand()
            {
                Id = RoomType.Id,
                Name = RoomType.Name,
                AmenitySetupDetails = RoomType.AmenitySetupDetails,
                BedTypeSetupDetails = RoomType.BedTypeSetupDetails,
                Price = RoomType.Price
            };

            var result = await _apiService.PutAsync<Result>($"api/roomtype/{RoomType.Id}", RoomType);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update room type.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room type updated successfully.";
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
            _logger.LogError(ex, "Error occurred while updating room type with ID {Id}", RoomType.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the room type.");
            return Page();
        }
    }
} 
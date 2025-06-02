using HotelBooking.Application.CQRS.Building.DTOs;
using HotelBooking.Application.CQRS.Hotel.DTOs;
using HotelBooking.Application.CQRS.Room.Commands;
using HotelBooking.Application.CQRS.RoomType.DTOs;
using HotelBooking.Domain.AggregateModels.BuildingAggregate;
using HotelBooking.Domain.AggregateModels.HotelAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace HotelBooking.Web.Pages.Rooms;

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
    public CreateRoomCommand Room { get; set; } = new();

    [BindProperty]
    public List<HotelDto> Hotels { get; set; } = new();

    [BindProperty]
    public List<BuildingDto> Buildings { get; set; } = new();
    [BindProperty]
    public List<FloorDto> Floors { get; set; } = new();

    [BindProperty]
    public List<RoomTypeDto> RoomTypes { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [BindProperty(SupportsGet = true)]
    public bool IsFromBuildingContext { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedHotelId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? SelectedBuildingId { get; set; }

    public async Task<IActionResult> OnGetAsync(int? buildingId)
    {
        try
        {
            if (buildingId.HasValue)
            {
                SelectedBuildingId = buildingId.Value;
                IsFromBuildingContext = true;
            }    

            if (IsFromBuildingContext)
            {
                var floorsResult = await _apiService.GetAsync<List<FloorDto>>($"api/building/floor/{SelectedBuildingId}");
                if (floorsResult != null && floorsResult.IsSuccess && floorsResult.Data != null)
                {
                    Floors = floorsResult.Data;
                }

                var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");
                if (roomTypesResult != null && roomTypesResult.IsSuccess && roomTypesResult.Data != null)
                {
                    RoomTypes = roomTypesResult.Data;
                }
            }
            else
            {
                await LoadDropdownsAsync();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            ModelState.Clear();
            await LoadDropdownsAsync();
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        try
        {
            if (IsFromBuildingContext)
            {
                var floorsResult = await _apiService.GetAsync<List<FloorDto>>($"api/building/floor/{SelectedBuildingId}");
                if (floorsResult != null && floorsResult.IsSuccess && floorsResult.Data != null)
                {
                    Floors = floorsResult.Data;
                }

                var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");
                if (roomTypesResult != null && roomTypesResult.IsSuccess && roomTypesResult.Data != null)
                {
                    RoomTypes = roomTypesResult.Data;
                }
            }
            else
            {
                await LoadDropdownsAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data for room creation");
            TempData["ErrorMessage"] = "An error occurred while loading the form data.";
            return !string.IsNullOrEmpty(ReturnUrl)
                ? Redirect(ReturnUrl)
                : RedirectToPage("./Index");
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/room", Room);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create room.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Room created successfully!";
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
            _logger.LogError(ex, "Error creating room");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the room.");
            return Page();
        }
    }

    private async Task LoadDropdownsAsync()
    {
        var hotelsResult = await _apiService.GetAsync<List<HotelDto>>($"api/hotel");
        if (hotelsResult != null && hotelsResult.IsSuccess && hotelsResult.Data != null)
        {
            Hotels = hotelsResult.Data; 
            if (Hotels.Count != 0 && !Hotels.Any(h => h.Id == SelectedHotelId))
            {
                SelectedHotelId = Hotels.FirstOrDefault()?.Id;
            }
        }

        if (SelectedHotelId != null && SelectedHotelId != 0)
        {
            var buildingsResult = await _apiService.GetAsync<List<BuildingDto>>($"api/building/hotel/{SelectedHotelId}");
            if (buildingsResult != null && buildingsResult.IsSuccess && buildingsResult.Data != null)
            {
                Buildings = buildingsResult.Data;
                if (Buildings.Count != 0 && !Buildings.Any(b => b.Id == SelectedBuildingId))
                {
                    SelectedBuildingId = Buildings.FirstOrDefault()?.Id;
                }
            }
        }

        if (SelectedBuildingId != null && SelectedBuildingId != 0)
        {
            var floorsResult = await _apiService.GetAsync<List<FloorDto>>($"api/building/floor/{SelectedBuildingId}");
            if (floorsResult != null && floorsResult.IsSuccess && floorsResult.Data != null)
            {
                Floors = floorsResult.Data;
            }
        }

        var roomTypesResult = await _apiService.GetAsync<List<RoomTypeDto>>("api/roomtype");
        if (roomTypesResult != null && roomTypesResult.IsSuccess && roomTypesResult.Data != null)
        {
            RoomTypes = roomTypesResult.Data;
        }
    }
} 
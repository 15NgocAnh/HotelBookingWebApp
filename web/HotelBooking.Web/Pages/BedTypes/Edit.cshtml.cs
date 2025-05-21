using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.BedType.Commands.UpdateBedType;
using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.BedTypes;

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
    public BedTypeDto BedType { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var result = await _apiService.GetAsync<BedTypeDto>($"api/bedtype/{id}");
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess && result.Data != null)
            {
                BedType = result.Data;
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch bed type.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching bed type");
            TempData["ErrorMessage"] = "An error occurred while fetching the bed type.";
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
            var updateBedType = new UpdateBedTypeCommand()
            {
                Id = BedType.Id,
                Name = BedType.Name
            };

            var result = await _apiService.PutAsync<Result>($"api/bedtype/{BedType.Id}", updateBedType);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update bed type.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Bed type updated successfully!";
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
            _logger.LogError(ex, "Error updating bed type");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the bed type.");
            return Page();
        }
    }
} 
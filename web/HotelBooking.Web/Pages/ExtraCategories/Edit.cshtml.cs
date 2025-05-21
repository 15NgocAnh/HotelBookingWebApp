using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using HotelBooking.Application.CQRS.ExtraCategory.Commands;

namespace HotelBooking.Web.Pages.ExtraCategories;

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
    public ExtraCategoryDto ExtraCategory { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var result = await _apiService.GetAsync<ExtraCategoryDto>($"api/extracategory/{id}");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch extra category.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                ExtraCategory = result.Data;
                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra category.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting extra category with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the extra category.";
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
            var updateExtraCategory = new UpdateExtraCategoryCommand()
            {
                Id = ExtraCategory.Id,
                Name = ExtraCategory.Name
            };
            var result = await _apiService.PutAsync<Result>($"api/extracategory/{ExtraCategory.Id}", updateExtraCategory);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update extra category.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra category updated successfully.";
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
            _logger.LogError(ex, "Error occurred while updating extra category with ID {Id}", ExtraCategory.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the extra category.");
            return Page();
        }
    }
} 
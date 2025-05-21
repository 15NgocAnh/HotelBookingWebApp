using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Application.CQRS.ExtraItem.Commands;
using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.ExtraItems;

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
    public ExtraItemDto ExtraItem { get; set; }

    public List<ExtraCategoryDto> Categories { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var extraItemResult = await _apiService.GetAsync<ExtraItemDto>($"api/extraitem/{id}");
            var categoriesResult = await _apiService.GetAsync<List<ExtraCategoryDto>>("api/extracategory");

            if (extraItemResult == null || categoriesResult == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch data for extra item edit.";
                return RedirectToPage("./Index");
            }

            if (extraItemResult.IsSuccess && extraItemResult.Data != null)
            {
                ExtraItem = extraItemResult.Data;
            }
            else
            {
                TempData["ErrorMessage"] = extraItemResult.Messages.FirstOrDefault()?.Message ?? "Failed to fetch extra item.";
                return RedirectToPage("./Index");
            }

            if (categoriesResult.IsSuccess && categoriesResult.Data != null)
            {
                Categories = categoriesResult.Data;
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting extra item with ID {Id}", id);
            TempData["ErrorMessage"] = "An error occurred while retrieving the extra item.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            try
            {
                var result = await _apiService.GetAsync<List<ExtraCategoryDto>>("api/extracategory");
                if (result != null && result.IsSuccess && result.Data != null)
                {
                    Categories = result.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching categories");
                TempData["ErrorMessage"] = "An error occurred while fetching categories.";
                return RedirectToPage("./Index");
            }
            return Page();
        }

        try
        {
            var updateExtraItem = new UpdateExtraItemCommand()
            {
                Id = ExtraItem.Id,
                Name = ExtraItem.Name,
                ExtraCategoryId = ExtraItem.ExtraCategoryId,
            };
            var result = await _apiService.PutAsync<Result>($"api/extraitem/{ExtraItem.Id}", updateExtraItem);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to update extra item.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra item updated successfully.";
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
            _logger.LogError(ex, "Error occurred while updating extra item with ID {Id}", ExtraItem.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the extra item.");
            return Page();
        }
    }
} 
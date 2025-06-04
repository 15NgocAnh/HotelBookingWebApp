using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Application.CQRS.ExtraItem.Commands;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.ExtraItems;

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
    public CreateExtraItemCommand ExtraItem { get; set; }

    public List<ExtraCategoryDto> Categories { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<ExtraCategoryDto>>("api/extracategory");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch categories.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                Categories = result.Data;
                return Page();
            }

            TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch categories.";
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            TempData["ErrorMessage"] = "An error occurred while fetching categories.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var extracategories = await _apiService.GetAsync<List<ExtraCategoryDto>>("api/extracategory");
        if (extracategories != null && extracategories.IsSuccess && extracategories.Data != null)
        {
            Categories = extracategories.Data;
        }
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var result = await _apiService.PostAsync<int>("api/extraitem", ExtraItem);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create extra item.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra item created successfully!";
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
            _logger.LogError(ex, "Error creating extra item");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the extra item.");
            return Page();
        }
    }
} 
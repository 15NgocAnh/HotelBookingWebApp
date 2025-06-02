using HotelBooking.Application.CQRS.ExtraCategory.Commands;
using HotelBooking.Application.CQRS.ExtraCategory.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.ExtraCategories;

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
    public string Name { get; set; }

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
            var extraCategory = new CreateExtraCategoryCommand { Name = Name };
            var result = await _apiService.PostAsync<int>("api/extracategory", extraCategory);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create extra category.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Extra category created successfully!";
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
            _logger.LogError(ex, "Error creating extra category");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the extra category.");
            return Page();
        }
    }
} 
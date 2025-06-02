using HotelBooking.Application.CQRS.BedType.Commands.CreateBedType;
using HotelBooking.Application.CQRS.BedType.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.BedTypes;

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
            var bedType = new CreateBedTypeCommand { Name = Name };
            var result = await _apiService.PostAsync<int>("api/bedtype", bedType);
            
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create bed type.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Bed type created successfully!";
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
            _logger.LogError(ex, "Error creating bed type");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the bed type.");
            return Page();
        }
    }
} 
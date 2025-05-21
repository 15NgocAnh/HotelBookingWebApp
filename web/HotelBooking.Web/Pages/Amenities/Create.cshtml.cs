using HotelBooking.Application.CQRS.Amenity.Commands.CreateAmenity;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Amenities
{
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
        public CreateAmenityCommand Amenity { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _apiService.PostAsync<int>("api/amenity", Amenity);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to create amenity.");
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Amenity created successfully!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    foreach (var message in result.Messages)
                    {
                        ModelState.AddModelError(string.Empty, message.Message);
                    }
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating amenity");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the amenity.");
                return Page();
            }
        }
    }
} 
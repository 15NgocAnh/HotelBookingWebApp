using HotelBooking.Application.Common.Models;
using HotelBooking.Application.CQRS.Amenity.Commands.UpdateAmenity;
using HotelBooking.Application.CQRS.Amenity.DTOs;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Amenities
{
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
        public AmenityDto Amenity { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            try
            {
                var result = await _apiService.GetAsync<AmenityDto>($"api/amenity/{id}");
                if (result == null)
                {
                    return NotFound();
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Amenity = result.Data;
                    return Page();
                }
                else
                {
                    TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch amenity.";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching amenity for edit");
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
                var updateAmenity = new UpdateAmenityCommand()
                {
                    Id = Amenity.Id,
                    Name = Amenity.Name
                };

                var result = await _apiService.PutAsync<Result>($"api/amenity/{Amenity.Id}", updateAmenity);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Failed to update amenity.");
                    return Page();
                }

                if (result.IsSuccess)
                {
                    TempData["SuccessMessage"] = "Amenity updated successfully!";
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
                _logger.LogError(ex, "Error updating amenity");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the amenity.");
                return Page();
            }
        }
    }
} 
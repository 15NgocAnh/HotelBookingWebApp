using HotelBooking.Application.CQRS.ExtraItem.DTOs;
using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Domain.AggregateModels.ExtraItemAggregate;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Invoices
{
    public class DetailsModel : PageModel
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CreateModel> _logger;

        public DetailsModel(IApiService apiService, ILogger<CreateModel> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        [BindProperty]
        public InvoiceDto Invoice { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch invoice.";
                return RedirectToPage("/Bookings/Index");
            }

            try
            {
                var result = await _apiService.GetAsync<InvoiceDto>($"api/invoice/{id}");
                if (result == null)
                {
                    TempData["ErrorMessage"] = "Failed to fetch invoice.";
                    return RedirectToPage("/Bookings/Index");
                }

                if (result.IsSuccess && result.Data != null)
                {
                    Invoice = result.Data;
                    return Page();
                }

                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch invoice.";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting invoice with ID {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while retrieving the invoice.";
                return RedirectToPage("/Bookings/Index");
            }
        }


    }
}

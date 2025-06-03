using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Application.CQRS.Invoice.Queries;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Invoices;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IApiService apiService, ILogger<IndexModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public List<InvoiceDto> Invoices { get; set; } = new();
    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<InvoiceDto>>("api/invoice");
            if (result == null || !result.IsSuccess || result.Data == null)
            {
                ErrorMessage = result?.Messages.FirstOrDefault()?.Message ?? "Failed to fetch invoices.";
                return Page();
            }

            Invoices = result.Data;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading invoices");
            ErrorMessage = "An error occurred while loading the invoices.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        try
        {
            var result = await _apiService.DeleteAsync($"api/invoice/{id}");
            if (result == null || !result.IsSuccess)
            {
                TempData["ErrorMessage"] = result?.Messages.FirstOrDefault()?.Message ?? "Failed to delete invoice.";
                return RedirectToPage();
            }

            TempData["SuccessMessage"] = "Invoice deleted successfully!";
            return RedirectToPage();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting invoice");
            TempData["ErrorMessage"] = "An error occurred while deleting the invoice.";
            return RedirectToPage();
        }
    }
} 
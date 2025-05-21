using HotelBooking.Application.CQRS.Invoice.DTOs;
using HotelBooking.Application.CQRS.Booking.DTOs;
using HotelBooking.Application.Common.Models;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace HotelBooking.Web.Pages.Invoices;

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
    public int BookingId { get; set; }

    [BindProperty]
    public DateTime IssueDate { get; set; }

    [BindProperty]
    public DateTime DueDate { get; set; }

    [BindProperty]
    public string Notes { get; set; }

    public List<BookingDto> ActiveBookings { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<List<BookingDto>>("api/booking/active");
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to fetch active bookings.";
                return RedirectToPage("./Index");
            }

            if (result.IsSuccess && result.Data != null)
            {
                ActiveBookings = result.Data;
                IssueDate = DateTime.Today;
                DueDate = DateTime.Today.AddDays(7);
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = result.Messages.FirstOrDefault()?.Message ?? "Failed to fetch active bookings.";
                return RedirectToPage("./Index");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active bookings");
            TempData["ErrorMessage"] = "An error occurred while loading active bookings.";
            return RedirectToPage("./Index");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            try
            {
                var result = await _apiService.GetAsync<List<BookingDto>>("api/booking/active");
                if (result != null && result.IsSuccess && result.Data != null)
                {
                    ActiveBookings = result.Data;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active bookings");
                TempData["ErrorMessage"] = "An error occurred while loading active bookings.";
                return RedirectToPage("./Index");
            }
            return Page();
        }

        try
        {
            var invoice = new InvoiceDto
            {
                BookingId = BookingId,
                IssueDate = IssueDate,
                DueDate = DueDate,
                Notes = Notes,
                Status = Domain.AggregateModels.InvoiceAggregate.InvoiceStatus.Pending
            };

            var result = await _apiService.PostAsync<InvoiceDto>("api/invoice", invoice);

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Failed to create invoice.");
                return Page();
            }

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Invoice created successfully!";
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
            _logger.LogError(ex, "Error creating invoice");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the invoice.");
            return Page();
        }
    }
} 
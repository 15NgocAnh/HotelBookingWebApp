using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Statistics;

[Authorize(Roles = "SuperAdmin,HotelManager")]
public class BookingsModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<BookingsModel> _logger;

    public BookingsModel(IApiService apiService, ILogger<BookingsModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<BookingStatisticsDto>("api/statistics/bookings") ?? new();
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch booking statistics: {Message}", 
                    result.Messages.FirstOrDefault()?.Message);
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching booking statistics");
            return Page();
        }
    }
} 
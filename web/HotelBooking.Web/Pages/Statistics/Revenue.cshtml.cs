using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Statistics;

[Authorize(Roles = "SuperAdmin,HotelManager")]
public class RevenueModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<RevenueModel> _logger;

    public RevenueStatisticsDto Statistics { get; set; } = new();

    public RevenueModel(IApiService apiService, ILogger<RevenueModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<RevenueStatisticsDto>("api/statistics/revenue") ?? new();
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch revenue statistics: {Message}", 
                    result.Messages.FirstOrDefault()?.Message);
                return Page();
            }

            Statistics = result.Data;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching revenue statistics");
            return Page();
        }
    }
} 
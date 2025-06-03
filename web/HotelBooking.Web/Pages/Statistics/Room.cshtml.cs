using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages.Statistics;

[Authorize(Roles = "SuperAdmin,HotelManager")]
public class RoomModel : PageModel
{
    private readonly IApiService _apiService;
    private readonly ILogger<RoomModel> _logger;

    public RoomModel(IApiService apiService, ILogger<RoomModel> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var result = await _apiService.GetAsync<RoomStatisticsDto>("api/statistics/room") ?? new();
            if (!result.IsSuccess)
            {
                _logger.LogError("Failed to fetch room statistics: {Message}", 
                    result.Messages.FirstOrDefault()?.Message);
                return Page();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching room statistics");
            return Page();
        }
    }
} 
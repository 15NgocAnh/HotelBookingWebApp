using HotelBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportingService _reportingService;

    public ReportsController(IReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("booking-statistics")]
    public async Task<IActionResult> GetBookingStatistics([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportingService.GetBookingStatisticsAsync(startDate, endDate);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet("revenue")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportingService.GetRevenueReportAsync(startDate, endDate);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet("occupancy")]
    public async Task<IActionResult> GetOccupancyReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportingService.GetOccupancyReportAsync(startDate, endDate);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet("guests")]
    public async Task<IActionResult> GetGuestReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var result = await _reportingService.GetGuestReportAsync(startDate, endDate);
        if (!result.IsSuccess)
            return BadRequest(result.Messages);

        return Ok(result.Data);
    }
} 
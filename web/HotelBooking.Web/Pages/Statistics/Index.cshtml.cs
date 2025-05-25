using HotelBooking.Application.CQRS.Statistic.Dtos;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.Statistics;

public class IndexModel : PageModel
{
    private readonly IApiService _apiService;

    public IndexModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public DateTime? StartDate { get; set; }

    [BindProperty]
    public DateTime? EndDate { get; set; }

    [BindProperty]
    public string FilterType { get; set; } = "month"; 
    public List<SelectListItem> FilterOptions { get; set; } = new()
    {
        new SelectListItem { Text = "Tuần này", Value = "week" },
        new SelectListItem { Text = "Tháng này", Value = "month" },
        new SelectListItem { Text = "Năm nay", Value = "year" },
        new SelectListItem { Text = "Tùy chọn", Value = "custom" },
    };

    public BookingStatisticsDto BookingStats { get; set; } = new();
    public RevenueStatisticsDto RevenueStats { get; set; } = new();
    public RoomStatisticsDto RoomStats { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (FilterType == "custom" && (!StartDate.HasValue || !EndDate.HasValue))
        {
            ModelState.AddModelError("", "Vui lòng chọn khoảng thời gian");
            return Page();
        }

        await LoadDataAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostExportReportAsync(string format)
    {
        var now = DateTime.Now;
        if (!StartDate.HasValue || !EndDate.HasValue)
        {
            switch (FilterType)
            {
                case "week":
                    StartDate = now.AddDays(-(int)now.DayOfWeek);
                    EndDate = StartDate.Value.AddDays(6);
                    break;
                case "month":
                    StartDate = new DateTime(now.Year, now.Month, 1);
                    EndDate = StartDate.Value.AddMonths(1).AddDays(-1);
                    break;
                case "year":
                    StartDate = new DateTime(now.Year, 1, 1);
                    EndDate = new DateTime(now.Year, 12, 31);
                    break;
            }
        }

        var request = new ExportReportRequest()
        {
            StartDate = StartDate,
            EndDate = EndDate,
            Format = format
        };

        var result = await _apiService.PostAsync<byte[]>("api/statistics/export", request);

        if (result == null || !result.IsSuccess || result.Data == null)
        {
            TempData["ErrorMessage"] = "Đã xảy ra lỗi trong quá trình xuất report. Vui lòng thử lại sau.";
            return Page();
        }

        TempData["SuccessMessage"] = "Export report successfully, please check file it downloaded.";

        if (request.Format?.ToLower() == "pdf")
        {
            return File(result.Data, "application/pdf", $"booking-statistics-{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        }
        else
        {
            return File(result.Data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"booking-statistics-{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
        }
    }

    private async Task LoadDataAsync()
    {
        var now = DateTime.Now;
        if (!StartDate.HasValue || !EndDate.HasValue)
        {
            switch (FilterType)
            {
                case "week":
                    StartDate = now.AddDays(-(int)now.DayOfWeek);
                    EndDate = StartDate.Value.AddDays(6);
                    break;
                case "month":
                    StartDate = new DateTime(now.Year, now.Month, 1);
                    EndDate = StartDate.Value.AddMonths(1).AddDays(-1);
                    break;
                case "year":
                    StartDate = new DateTime(now.Year, 1, 1);
                    EndDate = new DateTime(now.Year, 12, 31);
                    break;
            }
        }

        // Get booking statistics
        var bookingResult = await _apiService.GetAsync<BookingStatisticsDto>(
            $"api/statistics/booking?startDate={StartDate:yyyy-MM-dd}&endDate={EndDate:yyyy-MM-dd}");
        if (bookingResult?.IsSuccess == true && bookingResult.Data != null)
        {
            BookingStats = bookingResult.Data;
        }

        // Get revenue statistics
        var revenueResult = await _apiService.GetAsync<RevenueStatisticsDto>(
            $"api/statistics/revenue?startDate={StartDate:yyyy-MM-dd}&endDate={EndDate:yyyy-MM-dd}");
        if (revenueResult?.IsSuccess == true && revenueResult.Data != null)
        {
            RevenueStats = revenueResult.Data;
        }

        // Get room statistics
        var roomResult = await _apiService.GetAsync<RoomStatisticsDto>("api/statistics/room");
        if (roomResult?.IsSuccess == true && roomResult.Data != null)
        {
            RoomStats = roomResult.Data;
        }
    }
}
using HotelBooking.Application.CQRS.Dashboard.Dtos;
using HotelBooking.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelBooking.Web.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IApiService _apiService;
        public DashboardStatisticsDto Statistics { get; set; } = new();

        public DashboardModel(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task OnGetAsync()
        {
            var result = await _apiService.GetAsync<DashboardStatisticsDto>("api/dashboard/statistics");
            if (result?.IsSuccess == true && result.Data != null)
            {
                Statistics = result.Data;
            }
        }
    }
}

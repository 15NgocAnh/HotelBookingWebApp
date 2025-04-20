using HotelBooking.Domain.Constant;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace HotelBooking.Web.Pages.Admin
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DashboardModel : AbstractPageModel
    {
        public DashboardModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public int TotalUsers { get; set; }
        public decimal MonthlyRevenue { get; set; }

        public async Task OnGetAsync()
        {
            // Gi? l?p g?i API l?y t?ng s? user
            var userResponse = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            if (userResponse.IsSuccessStatusCode)
            {
                var users = await userResponse.Content.ReadAsStringAsync();
                TotalUsers = JsonSerializer.Deserialize<List<object>>(users)?.Count ?? 0;
            }

            // Gi? l?p g?i API l?y doanh thu tháng (mock data)
            var revenueResponse = await _httpClient.GetAsync("https://api.mocki.io/v1/ce5f60e2"); // API gi? l?p
            if (revenueResponse.IsSuccessStatusCode)
            {
                var revenueJson = await revenueResponse.Content.ReadAsStringAsync();
                MonthlyRevenue = JsonSerializer.Deserialize<decimal>(revenueJson);
            }
        }
    }
}

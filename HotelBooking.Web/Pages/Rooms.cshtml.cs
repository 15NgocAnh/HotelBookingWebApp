using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace HotelBooking.Web.Pages
{
    public class RoomsModel : AbstractPageModel
    {
        public RoomsModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<RoomDTO> Rooms { get; set; } = new();
        public string? SearchMessage { get; set; }

        public async Task OnGetAsync()
        {
            var tempData = TempData["RoomCondition"]?.ToString();
            if (tempData != null)
            {
                var searchCondition = JsonSerializer.Deserialize<RoomCondition>(tempData);
                if (searchCondition != null)
                {
                    // Build query string from condition
                    var queryString = $"api/v1/room/search?checkInDate={searchCondition.CheckInDate:yyyy-MM-dd}&" +
                                    $"checkOutDate={searchCondition.CheckOutDate:yyyy-MM-dd}&" +
                                    $"roomType={searchCondition.RoomType}&" +
                                    $"adultsCnt={searchCondition.AdultsCnt}&" +
                                    $"childrenCnt={searchCondition.ChildrenCnt}";

                    var response = await GetAsync<List<RoomDTO>>(queryString);
                    if (response != null)
                    {
                        Rooms = response;
                        SearchMessage = "Showing available rooms matching your criteria";
                    }
                    else
                    {
                        SearchMessage = "No rooms found matching your criteria";
                        Rooms = await GetAsync<List<RoomDTO>>("api/v1/room") ?? new List<RoomDTO>();
                    }
                }
            }
            else
            {
                Rooms = await GetAsync<List<RoomDTO>>("api/v1/room") ?? new List<RoomDTO>();
            }
        }
    }
}

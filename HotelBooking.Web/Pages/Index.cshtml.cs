using HotelBooking.Data.Models;
using HotelBooking.Domain.DTOs.Room;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HotelBooking.Web.Pages
{
    public class IndexModel : AbstractPageModel
    {
        [BindProperty]
        public List<RoomTypeModel> RoomType { get; set; }

        [BindProperty]
        public List<RoomDTO> Rooms { get; set; } = []; 

        [BindProperty]
        public RoomCondition Condition { get; set; } = new RoomCondition();

        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor) : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<IActionResult> OnGet() 
        {
            RoomType = await GetAsync<List<RoomTypeModel>>("api/v1/room/roomtypes") ?? new List<RoomTypeModel>();
            Rooms = await GetAsync<List<RoomDTO>>("api/v1/room") ?? new List<RoomDTO>();
            return Page();
        }

        public IActionResult OnPost()
        {
            var conditionData = JsonSerializer.Serialize(Condition);
            TempData["RoomCondition"] = conditionData;
            return Redirect("/Rooms");
        }
    }
}

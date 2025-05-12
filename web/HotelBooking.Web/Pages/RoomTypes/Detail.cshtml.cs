using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.RoomTypes
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public RoomTypeDTO RoomType { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            RoomType = await GetAsync<RoomTypeDTO>($"api/v1/roomtypes/{id}");
            if (RoomType == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 
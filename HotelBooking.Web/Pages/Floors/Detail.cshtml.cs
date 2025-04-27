using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Floors
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public FloorDTO Floor { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Floor = await GetAsync<FloorDTO>($"api/v1/floors/{id}");
            if (Floor == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 
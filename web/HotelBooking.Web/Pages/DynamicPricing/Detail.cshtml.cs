using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.DynamicPricing
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public DynamicPricingDTO Rule { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Rule = await GetAsync<DynamicPricingDTO>($"api/v1/dynamicpricing/{id}");
            if (Rule == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 
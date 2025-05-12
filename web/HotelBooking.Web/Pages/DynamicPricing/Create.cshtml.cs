using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBooking.Domain.Entities;

namespace HotelBooking.Web.Pages.DynamicPricing
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public CreateDynamicPricingDTO Rule { get; set; } = new();

        public List<SelectListItem> RoomTypes { get; set; } = new();
        public List<SelectListItem> RuleTypes { get; set; } = new();

        public async Task OnGetAsync()
        {
            var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = roomTypes?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList() ?? new List<SelectListItem>();

            RuleTypes = Enum.GetValues(typeof(DynamicPricingRuleType))
                .Cast<DynamicPricingRuleType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            var response = await PostAsync<CreateDynamicPricingDTO, DynamicPricingDTO>("api/v1/dynamicpricing", Rule);
            if (response != null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
} 
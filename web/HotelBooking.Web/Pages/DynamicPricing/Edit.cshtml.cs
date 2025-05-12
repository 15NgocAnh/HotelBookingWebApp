using AutoMapper;
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Domain.Entities;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.DynamicPricing
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class EditModel : AbstractPageModel
    {
        private readonly IMapper _mapper;

        public EditModel(IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
            _mapper = mapper;
        }

        [BindProperty]
        public UpdateDynamicPricingDTO Rule { get; set; } = new();

        public List<SelectListItem> RoomTypes { get; set; } = new();
        public List<SelectListItem> RuleTypes { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var rule = await GetAsync<DynamicPricingDTO>($"api/v1/dynamicpricing/{id}");
            if (rule == null)
            {
                return NotFound();
            }

            Rule = _mapper.Map<UpdateDynamicPricingDTO>(rule);

            var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = roomTypes?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name,
                Selected = rt.Id == rule.RoomTypeId
            }).ToList() ?? new List<SelectListItem>();

            RuleTypes = Enum.GetValues(typeof(DynamicPricingRuleType))
                .Cast<DynamicPricingRuleType>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(Rule.Id.ToString());
                return Page();
            }

            var response = await PutAsync<UpdateDynamicPricingDTO, DynamicPricingDTO>($"api/v1/dynamicpricing/{Rule.Id}", Rule);
            if (response != null)
            {
                return RedirectToPage("./Index");
            }

            return Page();
        }
    }
} 
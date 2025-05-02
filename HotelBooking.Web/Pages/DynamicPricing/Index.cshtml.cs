using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.DynamicPricing;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.DynamicPricing
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<DynamicPricingDTO> Rules { get; set; } = new();

        public async Task OnGetAsync()
        {
            Rules = await GetAsync<List<DynamicPricingDTO>>("api/v1/dynamicpricing") ?? new List<DynamicPricingDTO>();
        }

        public async Task<IActionResult> OnPostDelete(string ruleId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/dynamicpricing/{ruleId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "X�a quy t?c gi� th�nh c�ng!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"X�a quy t?c gi� th?t b?i: {errorMessage}";
                }
                return Redirect("/Dynamicpricing");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"C� l?i x?y ra khi x�a quy t?c gi�: {ex.Message}";
                return Redirect("/Dynamicpricing");
            }
        }
    }
} 
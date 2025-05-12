using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Branches
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class DetailModel : AbstractPageModel
    {
        public DetailModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public BranchDetailsDTO Branch { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Branch = await GetAsync<BranchDetailsDTO>($"api/v1/branches/{id}");

            if (Branch == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
} 
using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.Floors
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public CreateFloorDTO Floor { get; set; } = new();

        public List<SelectListItem> RoomTypes { get; set; } = new();
        public List<SelectListItem> Branches { get; set; } = new();

        public async Task OnGetAsync()
        {
            var roomTypes = await GetAsync<List<RoomTypeDTO>>("api/v1/roomtypes");
            RoomTypes = roomTypes?.Select(rt => new SelectListItem
            {
                Value = rt.Id.ToString(),
                Text = rt.Name
            }).ToList() ?? new List<SelectListItem>();

            var branches = await GetAsync<List<BranchDTO>>("api/v1/branches/all");
            Branches = branches?.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToList() ?? new List<SelectListItem>();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                var response = await PostAsync<CreateFloorDTO, FloorDTO>("api/v1/floors", Floor);
                
                if (response != null)
                {
                    TempData["SuccessMessage"] = "Thêm mới tầng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Thêm mới tầng thất bại");
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi thêm mới tầng: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
} 
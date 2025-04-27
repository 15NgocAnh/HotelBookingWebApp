using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.RoomType;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.RoomTypes
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class CreateModel : AbstractPageModel
    {
        public CreateModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public CreateRoomTypeDTO RoomType { get; set; } = new();

        public List<SelectListItem> Branches { get; set; } = new();

        public async Task OnGetAsync()
        {
            var branches = await GetAsync<List<BranchDTO>>("api/v1/branches");
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
                var response = await PostAsync<CreateRoomTypeDTO, HttpResponseMessage>("api/v1/roomtypes", RoomType);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Thêm mới loại phòng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Thêm mới loại phòng thất bại: {errorMessage}");
                    await OnGetAsync();
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi thêm mới loại phòng: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
} 
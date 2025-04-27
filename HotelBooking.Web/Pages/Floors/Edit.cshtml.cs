using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.DTOs.Floor;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBooking.Web.Pages.Floors
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class EditModel : AbstractPageModel
    {
        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UpdateFloorDTO Floor { get; set; } = new();

        public List<SelectListItem> Branches { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var floor = await GetAsync<FloorDTO>($"api/v1/floors/{id}");
            if (floor == null)
            {
                return NotFound();
            }

            Floor = new UpdateFloorDTO
            {
                Id = floor.Id,
                Name = floor.Name,
                Description = floor.Description,
                OrderFloor = floor.OrderFloor,
                IsActive = floor.IsActive,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var response = await PutAsync<UpdateFloorDTO, HttpResponseMessage>($"api/v1/floors/{Floor.Id}", Floor);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật tầng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Cập nhật tầng thất bại: {errorMessage}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi cập nhật tầng: {ex.Message}");
                var branches = await GetAsync<List<BranchDTO>>("api/v1/branches");
                return Page();
            }
        }
    }
} 
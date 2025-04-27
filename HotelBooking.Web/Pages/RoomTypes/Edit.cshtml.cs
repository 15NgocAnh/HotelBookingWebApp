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
    public class EditModel : AbstractPageModel
    {
        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        [BindProperty]
        public UpdateRoomTypeDTO RoomType { get; set; } = new();

        public List<SelectListItem> Branches { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var roomType = await GetAsync<RoomTypeDTO>($"api/v1/roomtypes/{id}");
            if (roomType == null)
            {
                return NotFound();
            }

            RoomType = new UpdateRoomTypeDTO
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Description = roomType.Description,
                NumberOfAdults = roomType.NumberOfAdults,
                NumberOfChildrent = roomType.NumberOfChildrent,
                RoomTypeSymbol = roomType.RoomTypeSymbol
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await PutAsync<UpdateRoomTypeDTO, HttpResponseMessage>($"api/v1/roomtypes/{RoomType.Id}", RoomType);
                
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật loại phòng thành công!";
                    return RedirectToPage("./Index");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Cập nhật loại phòng thất bại: {errorMessage}");
                    var branches = await GetAsync<List<BranchDTO>>("api/v1/branches");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Có lỗi xảy ra khi cập nhật loại phòng: {ex.Message}");
                var branches = await GetAsync<List<BranchDTO>>("api/v1/branches");
                return Page();
            }
        }
    }
} 
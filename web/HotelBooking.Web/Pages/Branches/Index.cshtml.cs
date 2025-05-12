using HotelBooking.Domain.Constant;
using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.Filtering;
using HotelBooking.Web.Pages.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBooking.Web.Pages.Branches
{
    [Authorize(Roles = CJConstant.ADMIN)]
    public class IndexModel : AbstractPageModel
    {
        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, httpContextAccessor)
        {
        }

        public List<BranchDTO> Branches { get; private set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public string SearchTerm { get; set; }

        public async Task OnGetAsync(int pageIndex = 1, string search = null)
        {
            SearchTerm = search;
            CurrentPage = pageIndex;

            // Lấy số lượng item mỗi trang từ configuration (mặc định là 10)
            int pageSize = _configuration.GetValue<int>("PageSize", 10);

            // Gọi API để lấy danh sách branches
            var apiResponse = await GetAsync<PagingReturnModel<BranchDTO>>($"api/v1/branches?pageIndex={pageIndex}&pageSize={pageSize}&search={search}");
            
            if (apiResponse?.Items != null)
            {
                Branches = apiResponse.Items ?? new List<BranchDTO>();
                TotalPages = apiResponse.TotalPages;
                CurrentPage = apiResponse.CurrentPage;
                TotalCount = apiResponse.TotalCount;
            }
            else
            {
                Branches = new List<BranchDTO>();
                TotalPages = 1;
                CurrentPage = pageIndex;
                TotalCount = 0;
            }
        }

        public async Task<IActionResult> OnPostDelete(string branchId)
        {
            try
            {
                var response = await DeleteAsync<HttpResponseMessage>($"api/v1/branches/{branchId}") ?? new HttpResponseMessage();
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Xóa cơ sở thành công!";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    TempData["ErrorMessage"] = $"Xóa cơ sở thất bại: {errorMessage}";
                }
                return Redirect("/Branches");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Có lỗi xảy ra khi xóa cơ sở: {ex.Message}";
                return Redirect("/Branches");
            }
        }
    }
} 
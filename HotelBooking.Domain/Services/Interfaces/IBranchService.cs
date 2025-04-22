using HotelBooking.Domain.DTOs.Branch;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Response;

namespace HotelBooking.Domain.Services.Interfaces
{
    public interface IBranchService
    {
        Task<ServiceResponse<List<BranchDTO>>> GetAllAsync();
        Task<ServiceResponse<PagingReturnModel<BranchDTO>>> GetPagedAsync(int pageIndex, int pageSize, string search = null);
        Task<ServiceResponse<BranchDetailsDTO>> FindByIdAsync(int id);
        Task<ServiceResponse<BranchCreateDTO>> SaveAsync(BranchCreateDTO branch);
        Task<ServiceResponse<BranchDetailsDTO>> UpdateAsync(int id, BranchDetailsDTO branch);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
        Task<ServiceResponse<bool>> DeleteMultipleAsync(int[] ids);
    }
} 
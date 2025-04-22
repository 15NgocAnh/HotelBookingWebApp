using HotelBooking.Data.Models;
using HotelBooking.Domain.Filtering;

namespace HotelBooking.Domain.Repositories.Interfaces
{
    public interface IBranchRepository
    {
        Task<List<BranchModel>> GetAllAsync();
        IQueryable<BranchModel> GetAllQueryable();
        Task<PagingReturnModel<BranchModel>> GetPagedAsync(int pageIndex, int pageSize, string search = null);
        Task<BranchModel> FindByIdAsync(int id);
        Task<BranchModel> AddAsync(BranchModel branch);
        Task<BranchModel> UpdateAsync(BranchModel branch);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteMultipleAsync(int[] ids);
    }
} 
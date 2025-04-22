using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Data.Models;
using HotelBooking.Domain.Filtering;
using HotelBooking.Domain.Repositories.Interfaces;
using HotelBooking.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace HotelBooking.Infrastructure.Repositories
{
    public class BranchRepository : GenericRepository<BranchModel>, IBranchRepository
    {
        public BranchRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<BranchModel>> GetAllAsync()
        {
            return await _context.Branches
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        public IQueryable<BranchModel> GetAllQueryable()
        {
            return _context.Branches
                .Where(x => !x.IsDeleted);
        }

        public async Task<PagingReturnModel<BranchModel>> GetPagedAsync(int pageIndex, int pageSize, string search = null)
        {
            var query = GetAllQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(b => 
                    b.Name.ToLower().Contains(search) ||
                    b.Address.ToLower().Contains(search) ||
                    b.Email.ToLower().Contains(search) ||
                    b.PhoneNumber.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(b => b.Name)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagingReturnModel<BranchModel>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<BranchModel> FindByIdAsync(int id)
        {
            return await _context.Branches
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<BranchModel> AddAsync(BranchModel branch)
        {
            _context.Branches.Add(branch);
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<BranchModel> UpdateAsync(BranchModel branch)
        {
            _context.Entry(branch).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return branch;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
                return false;

            branch.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMultipleAsync(int[] ids)
        {
            var branches = await _context.Branches
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            if (!branches.Any())
                return false;

            foreach (var branch in branches)
            {
                branch.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
} 
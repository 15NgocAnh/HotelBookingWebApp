using AutoMapper;
using HotelBooking.Data;
using HotelBooking.Domain.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBooking.Domain.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        protected readonly IMapper _mapper;
        public GenericRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(T entity)
        {
            try
            {
                await _context.Set<T>().AddAsync(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw;
            }

        }
        public async Task UpdateAsync(T entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task AddRangeAsync(IEnumerable<T> Data)
        {
            await _context.Set<T>().AddRangeAsync(Data);
            _context.SaveChanges();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public IQueryable<T> GetAllAsync()
        {
            return _context.Set<T>();
        }
        public T? GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return _context.SaveChangesAsync();
        }
        public Task RemoveRangeAsync(IEnumerable<T> Data)
        {
            _context.Set<T>().RemoveRange(Data);
            return _context.SaveChangesAsync();
        }
        public async Task SoftDelete(T entity)
        {
            var propertyInfo = entity.GetType().GetProperty("is_deleted");

            if (propertyInfo != null)
            {
                propertyInfo.SetValue(entity, true);
                await _context.SaveChangesAsync(); 
            }
        }
        public IQueryable<T> GetSoftDelete()
        {
            return _context.Set<T>().IgnoreQueryFilters().Where(e => EF.Property<bool>(e, "is_deleted"));
        }
    }
}

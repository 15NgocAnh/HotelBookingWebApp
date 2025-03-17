namespace HotelBooking.Domain.Filtering
{
    public interface IFilterHelper<T>
    {
        IQueryable<T> ApplySorting(IQueryable<T> Data, string? orderByQueryString);
        Task<PagingReturnModel<T>> ApplyPaging(IQueryable<T> source, int pageNumber, int pageSize);
        Task<PagingReturnModel<T>> ApplyPaging(IEnumerable<T> source, int pageNumber, int pageSize);
    }
}

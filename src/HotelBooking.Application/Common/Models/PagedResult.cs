namespace HotelBooking.Application.Common.Models;
public class PagedResult<T> where T : class
{
    public PagedResult(int page, int pageSize, int totalCount, T[] data)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        Data = data;
        TotalPages = CalculateTotalPages();
    }

    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public int TotalPages { get; init; }
    public T[] Data { get; init; } = [];

    private int CalculateTotalPages()
    {
        return PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    }
}
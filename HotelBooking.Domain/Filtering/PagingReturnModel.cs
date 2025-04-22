namespace HotelBooking.Domain.Filtering
{
    public class PagingReturnModel<T>
	{
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public List<T> Items { get; set; }

		public PagingReturnModel(List<T> items, int totalCount, int currentPage, int pageSize)
		{
			TotalCount = totalCount;
			PageSize = pageSize;
			CurrentPage = currentPage;
			TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
			Items = items;
		}
	}
}

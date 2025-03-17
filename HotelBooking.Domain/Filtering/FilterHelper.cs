using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace HotelBooking.Domain.Filtering
{
    public class FilterHelper<T> : IFilterHelper<T>
	{
		public IQueryable<T> ApplySorting(IQueryable<T> source, string? orderByQueryString)
		{
			if (!source.Any() || string.IsNullOrWhiteSpace(orderByQueryString))
				return source;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty == null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');

            if (string.IsNullOrWhiteSpace(orderQuery))
                return null;

            return source.OrderBy(orderQuery);
        }

		public async Task<PagingReturnModel<T>> ApplyPaging(IQueryable<T>? source, int pageNumber, int pageSize)
		{
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");
            }
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), "Page limit must be greater than or equal to 1.");
            }
            var count = source.Count();
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingReturnModel<T>(items, count, pageNumber, pageSize);
        }

        public async Task<PagingReturnModel<T>> ApplyPaging(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            if (source == null || pageNumber < 1 || pageSize < 1)
                return null;

            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagingReturnModel<T>(items, count, pageNumber, pageSize);
        }
    }
}

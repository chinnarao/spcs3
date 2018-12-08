using System.Collections.Generic;
using System.Linq;

namespace Repository.Extensions
{
    public static class EnumerableExtensions
    {
        public static PaginatedList<T> ToPaginatedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize) where T : class
        {
            var enumerable = source as T[] ?? source.ToArray();
            var count = enumerable.Length;
            var items = enumerable.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
namespace C_C_Test.Common
{
    using Microsoft.EntityFrameworkCore;

    public static class PaginationExtensions
    {
        public static PaginatedList<T> ToPagedList<T>(this IQueryable<T> source, int page = 1, int pageSize = 25)
            where T : class
        {
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PaginatedList<T>(items, count, page, pageSize);
        }

        public static async Task<PaginatedList<TResult>> ToPagedList<T, TResult>(
            this IQueryable<T> source,
            Func<T, TResult> selectFunc,
            int page = 1,
            int pageSize = 25)
            where TResult : class
            where T : class
        {
            var count = await source.CountAsync();
            var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<TResult>(items.Select(selectFunc).ToList(), count, page, pageSize);
        }
    }
}

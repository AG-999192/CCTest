namespace C_C_Test.Common
{
    using Microsoft.EntityFrameworkCore;

    public class PaginatedList<T> : List<T>, IPaginatedList
        where T : class
    {
        /// <summary>
        /// Maximum number of pages to display in pagination memu.
        /// </summary>
        private const int MaxDisplaySize = 10;

        public PaginatedList()
        {
        }

        public PaginatedList(List<T> items, int count, int currentPage, int pageSize)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalItems = count;
            this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.Start = 1;

            if (this.TotalPages > MaxDisplaySize)
            {
                this.DisplayList = MaxDisplaySize;

                if (this.CurrentPage > MaxDisplaySize)
                {
                    this.Start = this.CurrentPage - this.DisplayList + 1;
                }
            }
            else
            {
                this.DisplayList = this.TotalPages;
            }

            this.AddRange(items);
        }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int TotalItems { get; private set; }

        public bool HasPrevious => this.CurrentPage > 1;

        public bool HasNext => this.CurrentPage < this.TotalPages;

        public int DisplayList { get; private set; }

        public int Start { get; private set; }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int currentPage, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, currentPage, pageSize);
        }
    }
}

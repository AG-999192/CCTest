namespace C_C_Test.Common
{
    using System.Collections;

    /// <summary>
    /// Pagination Interface for common pagination results.
    /// </summary>
    public interface IPaginatedList : IList
    {
        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage { get; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// Gets the total items.
        /// </summary>
        public int TotalItems { get; }

        /// <summary>
        /// Gets a value indicating whether there is previous pages.
        /// </summary>
        public bool HasPrevious { get; }

        /// <summary>
        /// Gets a value indicating whether there are more pages.
        /// </summary>
        public bool HasNext { get; }

        /// <summary>
        /// Gets the display list.
        /// </summary>
        public int DisplayList { get; }

        /// <summary>
        /// Gets the start page.
        /// </summary>
        public int Start { get; }
    }
}

using C_C_Test.Models;
using MediatR;

namespace C_C_Test.Queries
{
    /// <summary>
    /// Implementation
    /// Returns List<DataViewModel>
    /// </summary>
    public class GetPaginatedDataQuery : IRequest<PaginatedDataViewModel>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }


        /// <summary>
        /// constructor.
        /// </summary>
        public GetPaginatedDataQuery()
        {
        }

        /// <summary>
        /// constructor.
        /// </summary>
        public GetPaginatedDataQuery(int pageIndex, int size)
        {
            this.PageIndex = pageIndex;
            this.PageSize = size;
        }
    }
}

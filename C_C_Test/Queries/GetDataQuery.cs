using C_C_Test.Models;
using MediatR;

namespace C_C_Test.Queries
{
    /// <summary>
    /// Implementation
    /// Returns List<DataViewModel>
    /// </summary>
    public class GetDataQuery : IRequest<List<DataViewModel>>
    {
        /// <summary>
        /// constructor.
        /// </summary>
        public GetDataQuery()
        {
        }
    }
}

using C_C_Test.Models;
using MediatR;

namespace C_C_Test.Queries
{
    /// <summary>
    /// Implementation
    /// Returns DatabaseStatusModel
    /// </summary>
    public class UpdateDatabaseQuery : IRequest<DatabaseStatusModel>
    {
        /// <summary>
        /// constructor.
        /// </summary>
        public UpdateDatabaseQuery() { }
    }
}

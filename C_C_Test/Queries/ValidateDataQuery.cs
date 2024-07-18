using C_C_Test.Models;
using MediatR;

namespace C_C_Test.Queries
{
    /// <summary>
    /// Implementation
    /// Returns ValidationViewModel
    /// </summary>
    public class ValidateDataQuery : IRequest<ValidationViewModel>
    {
        /// <summary>
        /// constructor.
        /// </summary>
        public ValidateDataQuery() { }
    }
}

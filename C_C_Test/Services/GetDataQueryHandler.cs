using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Models;
using C_C_Test.Pages;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    /// <summary>
    /// Implementation of GetDataQueryHandler
    /// </summary>
    public class GetDataQueryHandler : IRequestHandler<GetDataQuery, List<DataViewModel>>
    {
        private readonly IDataRepository dataRepository;

        private readonly ILogger logger;

        private readonly IConversion conversion;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataRepository"></param>
        /// <param name="logger"></param>
        /// <param name="conversion"></param>
        public GetDataQueryHandler(IDataRepository dataRepository, ILogger<GetDataQueryHandler> logger, IConversion conversion)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.conversion = conversion;
        }

        /// <summary>
        /// Handles query.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>List<DataViewModel></returns>
        public async Task<List<DataViewModel>> Handle(GetDataQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug("Handle called in GetDataQueryHandler");
            List<DataViewModel> viewDatabase = new List<DataViewModel>();

            var retVal = await this.dataRepository.GetData();

            viewDatabase = this.conversion.MapRetrievedDataToDataView(retVal);

            return viewDatabase;
        }
    }
}

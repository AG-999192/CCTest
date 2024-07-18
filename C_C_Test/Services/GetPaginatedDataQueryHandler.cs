using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    /// <summary>
    /// Implementation of GetDataQueryHandler
    /// </summary>
    public class GetPaginatedDataQueryHandler : IRequestHandler<GetPaginatedDataQuery, PaginatedDataViewModel>
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
        public GetPaginatedDataQueryHandler(IDataRepository dataRepository, ILogger<GetPaginatedDataQueryHandler> logger, IConversion conversion)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.conversion = conversion;
        }

        public async Task<PaginatedDataViewModel> Handle(GetPaginatedDataQuery request, CancellationToken cancellationToken)
        {
            List<DataViewModel> viewDatabase = new List<DataViewModel>();
            PaginatedDataViewModel ret = new PaginatedDataViewModel();

            var retVal = await this.dataRepository.GetData();
            var pagRet = GetPaginatedResult(retVal, request.PageIndex, request.PageSize);
            ret.PaginatedViewDatabase = this.conversion.MapRetrievedDataToDataView(pagRet);
            ret.TotalSize = retVal.Count;

            return ret;
        }

        public List<RetrievedDataDto> GetPaginatedResult(List<RetrievedDataDto> data, int currentPage, int pageSize = 10)
        {
            return data.OrderBy(d => d.MPAN).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}

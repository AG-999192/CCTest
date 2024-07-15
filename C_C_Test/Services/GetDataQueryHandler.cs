using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Models;
using C_C_Test.Pages;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    public class GetDataQueryHandler : IRequestHandler<GetDataQuery, List<DataViewModel>>
    {
        private readonly IDataRepository dataRepository;

        private readonly ILogger logger;

        private readonly IConversion conversion;

        public GetDataQueryHandler(IDataRepository dataRepository, ILogger<ViewDatabaseModel> logger, IConversion conversion)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.conversion = conversion;
        }

        public async Task<List<DataViewModel>> Handle(GetDataQuery request, CancellationToken cancellationToken)
        {
            List<DataViewModel> viewDatabase = new List<DataViewModel>();

            var retVal = await this.dataRepository.GetData();

            viewDatabase = this.conversion.MapRetrievedDataToDataView(retVal);

            return viewDatabase;
        }
    }
}

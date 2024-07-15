using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.FileIO;
using C_C_Test.Models;
using C_C_Test.Pages;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    public class UpdateDatabaseQueryHandler : IRequestHandler<UpdateDatabaseQuery, DatabaseStatusModel>
    {
        private readonly ILogger logger;
        private readonly IFileParsing fileParsing;
        private readonly IDataRepository dataRepository;

        public UpdateDatabaseQueryHandler(IDataRepository dataRepository, ILogger<ViewDatabaseModel> logger, IFileParsing fileParsing)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.fileParsing = fileParsing;
        }

        public async Task<DatabaseStatusModel> Handle(UpdateDatabaseQuery request, CancellationToken cancellationToken)
        {
            var dbStatus = new DatabaseStatusModel();
            DBStatusDto status = new DBStatusDto();

            var parsedData = await this.fileParsing.ParseFile(new ValidationViewModel(), new List<string>());

            await this.dataRepository.AddData(parsedData, status);

            dbStatus.QueryStatus = status.QueryStatus;
            dbStatus.SuccessfulWrites = status.SuccessfulWrites;
            dbStatus.FailedWrites = status.FailedWrites;

            return dbStatus;
        }
    }
}

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
    /// <summary>
    /// Implewents UpdateDatabaseQueryHandler
    /// </summary>
    public class UpdateDatabaseQueryHandler : IRequestHandler<UpdateDatabaseQuery, DatabaseStatusModel>
    {
        private readonly ILogger logger;
        private readonly IFileParsing fileParsing;
        private readonly IDataRepository dataRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataRepository"></param>
        /// <param name="logger"></param>
        /// <param name="fileParsing"></param>
        public UpdateDatabaseQueryHandler(IDataRepository dataRepository, ILogger<UpdateDatabaseQueryHandler> logger, IFileParsing fileParsing)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.fileParsing = fileParsing;
        }

        /// <summary>
        /// Query Handler
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DatabaseStatusModel> Handle(UpdateDatabaseQuery request, CancellationToken cancellationToken)
        {
            var dbStatus = new DatabaseStatusModel();
            DBStatusDto status = new DBStatusDto();

            var parsedData = await this.fileParsing.ParseFile();

            await this.dataRepository.AddData(parsedData, status);

            dbStatus.QueryStatus = status.QueryStatus;
            dbStatus.SuccessfulWrites = status.SuccessfulWrites;
            dbStatus.FailedWrites = status.FailedWrites;

            return dbStatus;
        }
    }
}

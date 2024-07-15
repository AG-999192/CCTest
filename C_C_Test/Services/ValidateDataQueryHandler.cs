using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.FileIO;
using C_C_Test.Models;
using C_C_Test.Pages;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    public class ValidateDataQueryHandler : IRequestHandler<ValidateDataQuery, ValidationViewModel>
    {
        private readonly ILogger logger;

        private readonly IConversion conversion;

        private readonly IFileParsing fileParsing;

        public ValidateDataQueryHandler(IFileParsing fileParsing, ILogger<ViewDatabaseModel> logger, IConversion conversion)
        {
            this.fileParsing = fileParsing;
            this.logger = logger;
            this.conversion = conversion;
        }

        public async Task<ValidationViewModel> Handle(ValidateDataQuery request, CancellationToken cancellationToken)
        {
            ValidationViewModel validateData = new ValidationViewModel { RejectedRowsList = new List<string>()};

            await this.fileParsing.ParseFile(validateData);

            return validateData;
        }
    }
}

using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace C_C_Test.Pages
{
    public class ViewDatabaseModel : PageModel
    {
        private readonly IDataRepository dataRepository;

        private readonly ILogger logger;

        private readonly IConversion conversion;

        private readonly IMediator mediator;

        public ViewDatabaseModel(IDataRepository dataRepository, ILogger<ViewDatabaseModel> logger, IConversion conversion, IMediator mediator)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.conversion = conversion;
            this.mediator = mediator;
        }
     
        public List<DataViewModel> ViewDatabase { get; set; } = null!;

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on ViewDatabaseModel");

            var existingData = await this.mediator.Send(new GetDataQuery() { });

            var retVal = await this.dataRepository.GetData();

            ViewDatabase = this.conversion.MapRetrievedDataToDataView(retVal);

            // Add pagination

            return this.Page();
        }
    }
}

using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace C_C_Test.Pages
{
    public class ViewDatabaseModel : PageModel
    {
        private readonly IDataRepository dataRepository;

        private readonly ILogger logger;

        private readonly IConversion conversion;

        public ViewDatabaseModel(IDataRepository dataRepository, ILogger<ViewDatabaseModel> logger, IConversion conversion)
        {
            this.dataRepository = dataRepository;
            this.logger = logger;
            this.conversion = conversion;
        }
     
        public List<DataViewModel> ViewDatabase { get; set; } = null!;

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on ViewDatabaseModel");

            var retVal = await this.dataRepository.GetData();

            ViewDatabase = this.conversion.MapRetrievedDataToDataView(retVal);

            // Add pagination

            return this.Page();
        }
    }
}

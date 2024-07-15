using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.FileIO;
using C_C_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace C_C_Test.Pages
{
    public class ValidateDataModel : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;
        private readonly IFileParsing fileParsing;
        private readonly IDataRepository dataRepository;

        public ValidateDataModel(ILogger<UploadFileModel> logger, IFileParsing fileParsing, IDataRepository dataRepository)
        {
            this.logger = logger;
            this.fileParsing = fileParsing;
            this.dataRepository = dataRepository;
        }

        public ValidationViewModel ValidationView { get; set; }

        public List<string> RejectedRows { get; set; } = null!;

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on VerifyDataModel");

            ValidationView = new ValidationViewModel();
            RejectedRows = new List<string>();

            await this.fileParsing.ParseFile(ValidationView, RejectedRows);

            return this.Page();
        }
    }
}

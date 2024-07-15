using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.FileIO;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;
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
        private readonly IMediator mediator;

        public ValidateDataModel(ILogger<UploadFileModel> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
     
        }

        public ValidationViewModel ValidationView { get; set; }

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on VerifyDataModel");

            ValidationView = await this.mediator.Send(new ValidateDataQuery() { });

            return this.Page();
        }
    }
}

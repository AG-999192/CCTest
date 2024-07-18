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
    /// <summary>
    /// The ValidateDataModel implementation.
    /// </summary>
    public class ValidateDataModel : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// IMediator.
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor of ValidateDataModel
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ValidateDataModel(ILogger<ValidateDataModel> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        /// <summary>
        /// The view model.
        /// </summary>
        public ValidationViewModel ValidationView { get; set; }

        /// <summary>
        /// OnGet() method
        /// </summary>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> OnGet()
        {
            this.logger.LogInformation("Get method called on Validation View Model");

            ValidationView = await this.mediator.Send(new ValidateDataQuery() { });

            return this.Page();
        }
    }
}

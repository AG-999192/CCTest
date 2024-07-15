using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.Emuns;
using C_C_Test.FileIO;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C_C_Test.Pages
{
    /// <summary>
    /// Implamentation of the file upload page.
    /// </summary>
    public class UploadFileModel : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;
        /// <summary>
        /// Mediator
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// Constructor for UploadFileModel.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public UploadFileModel(ILogger<UploadFileModel> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        /// <summary>
        /// Status model.
        /// </summary>
        public DatabaseStatusModel DBStatusView { get; set; }

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on UploadFileModel");
            DBStatusView = await this.mediator.Send(new UpdateDatabaseQuery() { });

            return this.Page();
        }
    }
}

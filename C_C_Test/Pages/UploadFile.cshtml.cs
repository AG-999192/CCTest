using C_C_Test.DataAccess;
using C_C_Test.Dtos;
using C_C_Test.Emuns;
using C_C_Test.FileIO;
using C_C_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C_C_Test.Pages
{
    public class UploadFileModel : PageModel
    {
        /// <summary>
        /// Logger.
        /// </summary>
        private readonly ILogger logger;
        private readonly IFileParsing fileParsing;
        private readonly IDataRepository dataRepository;

        public UploadFileModel(ILogger<UploadFileModel> logger, IFileParsing fileParsing, IDataRepository dataRepository)
        {
            this.logger = logger;
            this.fileParsing = fileParsing;
            this.dataRepository = dataRepository;
        }

        public DatabaseStatusModel DBStatusView { get; set; }

        public List<string> RejectedRows { get; set; } = null!;

        public async Task<IActionResult> OnGet(int id)
        {
            // Add new page for DB update

            this.logger.LogDebug("Get method called on UploadFileModel with {Id}", id);
            List<ParsedDataDto> ParsedData;
            DBStatusView = new DatabaseStatusModel();
            var ValidationView = new ValidationViewModel();
            RejectedRows = new List<string>();

            ParsedData = await this.fileParsing.ParseFile(ValidationView, RejectedRows);

            var ret = await this.dataRepository.AddData(ParsedData[0]);
            DBStatusView.QueryStatus = ret.QueryStatus;
            DBStatusView.SuccessfulWrites = ret.SuccessfulWrites;

            return this.Page();
        }
    }
}

using C_C_Test.Common;
using C_C_Test.DataAccess;
using C_C_Test.Emuns;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace C_C_Test.Pages
{
    /// <summary>
    /// ViewDatabaseModel implementation.
    /// </summary>
    public class ViewPaginatedDatabaseModel : PageModel
    {
        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The Mediator.
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="mediator"></param>
        public ViewPaginatedDatabaseModel(ILogger<ViewDatabaseModel> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public List<DataViewModel> ViewDatabase { get; set; } = null!;

        /// <summary>
        /// Gets or sets index. Used to turn pages.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 30;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogInformation("Get method called on ViewPaginatedDatabaseModel");

            var ret = await this.mediator.Send(new GetPaginatedDataQuery() { PageIndex = CurrentPage, PageSize = PageSize });
            Count = ret.TotalSize;
            ViewDatabase = ret.PaginatedViewDatabase;

            return this.Page();
        }
    }
}

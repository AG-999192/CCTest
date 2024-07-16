using C_C_Test.Common;
using C_C_Test.Conversions;
using C_C_Test.DataAccess;
using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace C_C_Test.Pages
{
    /// <summary>
    /// ViewDatabaseModel implementation.
    /// </summary>
    public class ViewDatabaseModel : PageModel
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
        public ViewDatabaseModel(ILogger<ViewDatabaseModel> logger, IMediator mediator)
        {
            this.logger = logger;
            this.mediator = mediator;
        }

        public PaginatedList<DataViewModel> ViewDatabase { get; set; } = null!;

        /// <summary>
        /// Gets or sets index. Used to turn pages.
        /// </summary>
        [BindProperty(SupportsGet = true, Name = "p")]
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// OnGet()
        /// </summary>
        /// <returns>IActionResult</returns>
        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on ViewDatabaseModel");

            var ret =  await this.mediator.Send(new GetDataQuery() { });

            var vmsq = ret.AsQueryable();

            this.ViewDatabase = vmsq.ToPagedList(this.PageIndex, 50);

            // TODO Add pagination

            return this.Page();
        }
    }
}

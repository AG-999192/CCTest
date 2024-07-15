using C_C_Test.Common;
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
       
        private readonly ILogger logger;

        private readonly IMediator mediator;

        public ViewDatabaseModel(IDataRepository dataRepository, ILogger<ViewDatabaseModel> logger, IConversion conversion, IMediator mediator)
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

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string Sort { get; set; } = "garagenumber";

        [BindProperty(SupportsGet = true)]
        public SortDirection Direction { get; set; } = SortDirection.Asc;

        [BindProperty(SupportsGet = true, Name = "listtype")]
        public GarageListType ListType { get; set; }

        /// <summary>
        /// Gets link data fpr page.
        /// </summary>
        public Dictionary<string, string> LinkData =>
          new()
          {
              { "filter", this.Filter },
              { "p", this.ViewDatabase.CurrentPage.ToString() },
              { "sort", this.Sort },
              { "direction", this.Direction.ToString() },
              { "listtype", this.ListType.ToString() },
          };


       

        public async Task<IActionResult> OnGet()
        {
            this.logger.LogDebug("Get method called on ViewDatabaseModel");

            var ret =  await this.mediator.Send(new GetDataQuery() { });

            var vmsq = ret.AsQueryable();

            this.ViewDatabase = vmsq.ToPagedList(this.PageIndex, 50);

            // Add pagination

            return this.Page();
        }
    }

    public enum SortDirection
    {
        /// <summary>
        /// Ascending.
        /// </summary>
        Asc,

        /// <summary>
        /// Descending.
        /// </summary>
        Desc,
    }

    public enum GarageListType
    {
        /// <summary>
        /// Get all garages.
        /// </summary>
        All,

        /// <summary>
        /// Get garages that have readers that have outstanding table loads.
        /// </summary>
        HasReadersWithOutstandingTableLoads,

        /// <summary>
        /// Get garages that have readers that have no outstanding table loads.
        /// </summary>
        HasNoReadersWithOutstandingTableLoads,
    }
}

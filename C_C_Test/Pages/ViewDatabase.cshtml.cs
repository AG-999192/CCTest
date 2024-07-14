using C_C_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace C_C_Test.Pages
{
    public class ViewDatabaseModel : PageModel
    {
        [BindProperty]
        public int MPAN { get; set; }

        public string? MeterSerial { get; set; }

        [BindProperty]
        public string? DateOfInstallation { get; set; }

        public string? AddressLine { get; set; }

        public string? PostCode { get; set; }

        /// <summary>
        /// Gets or sets Data collection.
        /// </summary>
        public List<DataViewModel> ViewDatabase { get; set; } = null!;

        public IActionResult OnGet()
        {
            ViewDatabase = new List<DataViewModel> { new DataViewModel { MPAN = 1212212, AddressLine = "Addresss", DateOfInstallation = "DateOfInstallation", MeterSerial = "MeterSerial", PostCode = "postcode" } };

            return this.Page();
        }
    }
}

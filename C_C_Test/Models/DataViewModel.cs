using Microsoft.AspNetCore.Mvc;

namespace C_C_Test.Models
{
    /// <summary>
    /// Model to view Data
    /// </summary>
    public class DataViewModel
    {
        public decimal MPAN { get; set; }

        public string MeterSerial { get; set; } = string.Empty;

        public string DateOfInstallation { get; set; } = string.Empty;

        public string? AddressLine { get; set; }

        public string? PostCode { get; set; }
    }
}

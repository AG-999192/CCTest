namespace C_C_Test.Dtos
{
    /// <summary>
    /// The RetrievedDataDto implementation.
    /// </summary>
    public class RetrievedDataDto
    {
        public decimal MPAN { get; set; }

        public string MeterSerial { get; set; } = string.Empty;

        public string DateOfInstallation { get; set; } = string.Empty;

        public string? AddressLine { get; set; }

        public string? PostCode { get; set; }
    }
}

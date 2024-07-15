namespace C_C_Test.Models
{
    /// <summary>
    /// Model for validation of data.
    /// </summary>
    public class ValidationViewModel
    {
        public int SuccessfulRows { get; set; }

        public int RejectedRows { get; set; }

        public string ValidationStatus { get; set; } = string.Empty;

        public List<string> RejectedRowsList { get; set; } = null!;
    }
}

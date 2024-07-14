namespace C_C_Test.Models
{
    public class ValidationViewModel
    {
        public int SuccessfulRows { get; set; }

        public int RejectedRows { get; set; }

        public string ValidationStatus { get; set; } = string.Empty;
    }
}

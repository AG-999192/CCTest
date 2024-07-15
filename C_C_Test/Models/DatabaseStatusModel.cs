namespace C_C_Test.Models
{
    public class DatabaseStatusModel
    {
        public int SuccessfulWrites { get; set; }

        public int FailedWrites { get; set; }

        public string QueryStatus { get; set; } = string.Empty;
    }
}

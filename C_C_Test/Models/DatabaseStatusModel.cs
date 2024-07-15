namespace C_C_Test.Models
{
    /// <summary>
    /// Model for database.
    /// </summary>
    public class DatabaseStatusModel
    {
        public int SuccessfulWrites { get; set; }

        public int FailedWrites { get; set; }

        public string QueryStatus { get; set; } = string.Empty;
    }
}

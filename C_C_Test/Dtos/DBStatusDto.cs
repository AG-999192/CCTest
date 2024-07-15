namespace C_C_Test.Dtos
{
    public class DBStatusDto
    {
        public int SuccessfulWrites { get; set; }

        public int FailedWrites { get; set; }

        public string QueryStatus { get; set; } = string.Empty;
    }
}

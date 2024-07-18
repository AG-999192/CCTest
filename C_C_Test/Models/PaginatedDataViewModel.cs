namespace C_C_Test.Models
{
    public class PaginatedDataViewModel
    {
        public int TotalSize { get; set; }
        public List<DataViewModel> PaginatedViewDatabase { get; set; } = null!;
    }
}

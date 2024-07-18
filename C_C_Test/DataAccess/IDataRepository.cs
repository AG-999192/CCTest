using C_C_Test.Dtos;

namespace C_C_Test.DataAccess
{
    /// <summary>
    /// Interface for the data repository.
    /// </summary>
    public interface IDataRepository
    {
        Task AddData(List<ParsedDataDto> ParsedData, DBStatusDto status);

        Task<DBStatusDto> AddData(ParsedDataDto ParsedData);

        Task<List<RetrievedDataDto>> GetData();

        Task<DBStatusDto> AddDataBulk(List<ParsedDataDto> ParsedData, DBStatusDto status);
    }
}

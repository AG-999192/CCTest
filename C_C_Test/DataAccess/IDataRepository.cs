using C_C_Test.Dtos;

namespace C_C_Test.DataAccess
{
    public interface IDataRepository
    {
        Task AddData(List<ParsedDataDto> ParsedData, DBStatusDto status);

        Task<DBStatusDto> AddData(ParsedDataDto ParsedData);

        Task<List<RetrievedData>> GetData();
    }
}

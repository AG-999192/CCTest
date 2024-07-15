using C_C_Test.Dtos;

namespace C_C_Test.DataAccess
{
    public interface IDataRepository
    {
        Task<DBStatusDto> AddData(List<ParsedDataDto> ParsedData);

        Task<DBStatusDto> AddData(ParsedDataDto ParsedData);

        Task<List<RetrievedData>> GetData();
    }
}

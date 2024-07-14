using C_C_Test.Dtos;

namespace C_C_Test.DataAccess
{
    public interface IDataRepository
    {
        void AddData(List<ParsedDataDto> ParsedData);

        List<RetrievedData> GetData();
    }
}

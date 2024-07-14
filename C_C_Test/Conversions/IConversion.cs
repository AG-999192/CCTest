using C_C_Test.Dtos;
using C_C_Test.Models;

namespace C_C_Test.Conversions
{
    public interface IConversion
    {
        List<DataViewModel> MapRetrievedDataToDataView(List<RetrievedData> manifests);
    }
}

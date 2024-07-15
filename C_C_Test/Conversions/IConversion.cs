using C_C_Test.Dtos;
using C_C_Test.Models;

namespace C_C_Test.Conversions
{
    /// <summary>
    /// Interface for conversion.
    /// </summary>
    public interface IConversion
    {
        List<DataViewModel> MapRetrievedDataToDataView(List<RetrievedDataDto> manifests);
    }
}

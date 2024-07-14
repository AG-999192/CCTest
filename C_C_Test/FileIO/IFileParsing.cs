using C_C_Test.Dtos;
using C_C_Test.Models;
using System.Threading.Tasks;

namespace C_C_Test.FileIO
{
    public interface IFileParsing
    {
        public Task<List<ParsedDataDto>> ParseFile(ValidationViewModel validationViewModel, List<string> RejectedRows);
    }
}

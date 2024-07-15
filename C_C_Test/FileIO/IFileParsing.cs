using C_C_Test.Dtos;
using C_C_Test.Models;
using System.Threading.Tasks;

namespace C_C_Test.FileIO
{
    /// <summary>
    /// Interface for File Parsing.
    /// </summary>
    public interface IFileParsing
    {
        public Task<List<ParsedDataDto>> ParseFile();
        public Task<List<ParsedDataDto>> ParseFile(ValidationViewModel validationViewModel);
    }
}

using C_C_Test.Models;
using C_C_Test.Queries;
using MediatR;

namespace C_C_Test.Services
{
    public class GetDataQueryHandler : IRequestHandler<GetDataQuery, List<DataViewModel>>
    {
        public GetDataQueryHandler() { }

        public async Task<List<DataViewModel>> Handle(GetDataQuery request, CancellationToken cancellationToken)
        {
            
            return new List<DataViewModel>();
        }
    }
}

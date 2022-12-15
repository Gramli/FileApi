using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Http;

namespace File.Core.Queries
{
    internal class ConvertToQueryHandler : IConvertToQueryHandler
    {
        public Task<HttpDataResponse<FileDto>> HandleAsync(ConvertToQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

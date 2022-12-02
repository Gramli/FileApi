using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Http;

namespace File.Core.Queries
{
    internal class GetFilesInfoQueryHandler : IGetFilesInfoQueryHandler
    {
        public GetFilesInfoQueryHandler() 
        { 
        
        }

        public Task<HttpDataResponse<IEnumerable<FileDto>>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

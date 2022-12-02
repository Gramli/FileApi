using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Http;
using File.Domain.Queries;

namespace File.Core.Queries
{
    internal class DownloadFileQueryHandler : IDownloadFileQueryHandler
    {
        public DownloadFileQueryHandler() 
        { 
        
        }

        public Task<HttpDataResponse<FileDto>> HandleAsync(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

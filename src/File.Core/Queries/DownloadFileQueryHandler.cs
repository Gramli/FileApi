using File.Core.Abstractions;
using File.Domain.Abstractions;
using File.Domain.Http;
using File.Domain.Queries;

namespace File.Core.Queries
{
    internal class DownloadFileQueryHandler : IDownloadFileQueryHandler
    {
        public Task<HttpDataResponse<IFile>> HandleAsync(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

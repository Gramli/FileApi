using File.Domain.Abstractions;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IDownloadFileQueryHandler : IRequestHandler<IFile,DownloadFileQuery>
    {
    }
}

using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IDownloadFileQueryHandler : IRequestHandler<FileDto,DownloadFileQuery>
    {
    }
}

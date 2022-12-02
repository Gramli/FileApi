using File.Domain.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IFileQueriesRepository
    {
        FileDto GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken);
        IEnumerable<FileInfoDto> GetFilesInfo(CancellationToken cancellationToken);
    }
}

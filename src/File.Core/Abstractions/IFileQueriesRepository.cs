using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IFileQueriesRepository
    {
        Task<FileDto> GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken);
        Task<IEnumerable<FileInfoDto>> GetFilesInfo(CancellationToken cancellationToken);
    }
}

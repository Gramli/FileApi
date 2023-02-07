using File.Domain.Dtos;
using File.Domain.Queries;
using FluentResults;

namespace File.Core.Abstractions
{
    public interface IFileQueriesRepository
    {
        Task<Result<FileDto>> GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken);
        Task<IEnumerable<FileInfoDto>> GetFilesInfo(CancellationToken cancellationToken);
    }
}

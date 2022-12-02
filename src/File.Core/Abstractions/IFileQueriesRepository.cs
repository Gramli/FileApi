using File.Domain.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IFileQueriesRepository
    {
        IFile GetFile(DownloadFileQuery downloadFileQuery);
        IEnumerable<FileDto> GetFilesInfo();
    }
}

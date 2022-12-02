using File.Core.Abstractions;
using File.Domain.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Infrastructure.Database.Repositories
{
    internal class FileQueriesRepository : IFileQueriesRepository
    {
        public IFile GetFile(DownloadFileQuery downloadFileQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileDto> GetFilesInfo()
        {
            throw new NotImplementedException();
        }
    }
}

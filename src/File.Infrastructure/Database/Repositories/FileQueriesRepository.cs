using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;
using File.Infrastructure.Database.EFContext;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace File.Infrastructure.Database.Repositories
{
    internal class FileQueriesRepository : IFileQueriesRepository
    {
        private readonly FileContext _context;
        public FileQueriesRepository(FileContext fileContext)
        {
            _context = Guard.Against.Null(fileContext);
        }

        public async Task<FileDto> GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken)
        {
            var file = await _context.Files.FirstAsync(x => x.Id.Equals(downloadFileQuery.Id), cancellationToken);
            return file.Adapt<FileDto>();
        }

        public async Task<IEnumerable<FileInfoDto>> GetFilesInfo(CancellationToken cancellationToken)
        {
            return await _context.Files.ProjectToType<FileInfoDto>().ToListAsync(cancellationToken);
        }
    }
}

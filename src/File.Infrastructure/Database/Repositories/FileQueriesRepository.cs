using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;
using File.Infrastructure.Database.EFContext;
using File.Infrastructure.Resources;
using FluentResults;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace File.Infrastructure.Database.Repositories
{
    internal sealed class FileQueriesRepository : IFileQueriesRepository
    {
        private readonly FileContext _context;
        public FileQueriesRepository(FileContext fileContext)
        {
            _context = Guard.Against.Null(fileContext);
        }

        public async Task<Result<FileDto>> GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken)
        {
            var file = await _context.Files.FirstOrDefaultAsync(x => x.Id.Equals(downloadFileQuery.Id), cancellationToken);
            if(file is null)
            {
                return Result.Fail(string.Format(ErrorMessages.FileNotExists, downloadFileQuery.Id));
            }

            return Result.Ok(file.Adapt<FileDto>());
        }

        public async Task<IEnumerable<FileInfoDto>> GetFilesInfo(CancellationToken cancellationToken)
        {
            return await _context.Files.ProjectToType<FileInfoDto>().ToListAsync(cancellationToken);
        }
    }
}

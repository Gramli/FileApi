using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Queries;
using File.Infrastructure.Database.EFContext;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace File.Infrastructure.Database.Repositories
{
    internal class FileQueriesRepository : IFileQueriesRepository
    {
        private readonly IMapper _mapper;
        private readonly FileContext _context;
        public FileQueriesRepository(FileContext fileContext, IMapper mapper)
        {
            _context = Guard.Against.Null(fileContext);
            _mapper = Guard.Against.Null(mapper);
        }

        public FileDto GetFile(DownloadFileQuery downloadFileQuery, CancellationToken cancellationToken)
        {
            var file = _context.Files.FirstAsync(x => x.Id.Equals(downloadFileQuery.Id), cancellationToken);
            return _mapper.Map<FileDto>(file);
        }

        public IEnumerable<FileInfoDto> GetFilesInfo(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

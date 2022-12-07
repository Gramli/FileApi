using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Infrastructure.Database.EFContext;
using File.Infrastructure.Database.EFContext.Entities;
using FluentResults;
using Mapster;

namespace File.Infrastructure.Database.Repositories
{
    internal class FileCommandsRepository : IFileCommandsRepository
    {
        private readonly FileContext _context;
        public FileCommandsRepository(FileContext fileContext)
        {
            _context = Guard.Against.Null(fileContext);
        }
        public async Task<int> AddFileAsync(FileDto fileDto, CancellationToken cancellationToken)
        {
            var fileEntity = fileDto.Adapt<FileEntity>();
            await _context.Files.AddAsync(fileEntity);
            await _context.SaveChangesAsync(cancellationToken);
            return fileEntity.Id;
        }
    }
}

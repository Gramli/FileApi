using File.Domain.Abstractions;
using File.Domain.Dtos;
using FluentResults;

namespace File.Core.Abstractions
{
    public interface IFileConvertService
    {
        Task<Result<IFile>> ConvertTo(IFile sourceFile, string destinationFormat, CancellationToken cancellationToken);
        Task<Result<IFile>> ExportTo(FileDto sourceFile, string destinationFormat, CancellationToken cancellationToken);
    }
}

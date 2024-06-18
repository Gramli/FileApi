using File.Domain.Abstractions;
using File.Domain.Dtos;
using FluentResults;

namespace File.Core.Abstractions
{
    public interface IFileConvertService
    {
        Task<Result<IFileProxy>> ConvertTo(IFileProxy sourceFile, string destinationFormat, CancellationToken cancellationToken);
        Task<Result<IFileProxy>> ExportTo(FileDto sourceFile, string destinationFormat, CancellationToken cancellationToken);
    }
}

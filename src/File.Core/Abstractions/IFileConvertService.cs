using File.Domain.Abstractions;
using FluentResults;

namespace File.Core.Abstractions
{
    public interface IFileConvertService
    {
        Task<Result<IFile>> ConvertTo(IFile sourceFile, string destinationFormat);
    }
}

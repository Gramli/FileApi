using File.Domain.Abstractions;
using FluentResults;

namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverter
    {
        Task<Result<IFile>> Convert(byte[] sourceFileData, CancellationToken cancellationToken); 
    }
}

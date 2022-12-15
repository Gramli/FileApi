using File.Domain.Abstractions;
using FluentResults;

namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverter
    {
        Task<Result<IFile>> Convert(IFile sourceFile); 
    }
}

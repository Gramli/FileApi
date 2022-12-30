using File.Domain.Abstractions;
using FluentResults;

namespace File.Core.Abstractions
{
    internal interface IFileByOptionsValidator
    {
        Result<bool> Validate(IFile file);
    }
}

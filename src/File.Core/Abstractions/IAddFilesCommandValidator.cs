using File.Domain.Abstractions;
using File.Domain.Commands;
using FluentResults;
using Validot.Results;

namespace File.Core.Abstractions
{
    public interface IAddFilesCommandValidator
    {
        Result<bool> Validate(AddFilesCommand addFilesCommand);
    }
}

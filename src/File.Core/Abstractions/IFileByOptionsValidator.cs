using File.Domain.Abstractions;
using FluentResults;

namespace File.Core.Abstractions
{
    //TODO CONVERSION VALIDATION
    internal interface IFileByOptionsValidator
    {
        Result<bool> Validate(IFileProxy file);
        Result<bool> Validate(string extension);
        Result<bool> ValidateConversion(string sourceExtension, string destinationExtension);
    }
}

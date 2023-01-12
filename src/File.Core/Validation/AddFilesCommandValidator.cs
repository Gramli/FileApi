using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Commands;
using FluentResults;
using Validot;

namespace File.Core.Validation
{
    internal sealed class AddFilesCommandValidator : IAddFilesCommandValidator
    {
        private readonly IValidator<AddFilesCommand> _addFilesCommandValidator;
        private readonly IFileByOptionsValidator _fileByOptionsValidator;
        public AddFilesCommandValidator(IValidator<AddFilesCommand> addFilesCommandValidator, IFileByOptionsValidator fileByOptionsValidator)
        {
            _addFilesCommandValidator = Guard.Against.Null(addFilesCommandValidator);
            _fileByOptionsValidator = Guard.Against.Null(fileByOptionsValidator);
        }

        public Result<bool> Validate(AddFilesCommand addFilesCommand)
        {
            var validationResult = _addFilesCommandValidator.Validate(addFilesCommand);
            if (validationResult.AnyErrors)
            {
                return Result.Fail(validationResult.ToString());
            }

            foreach(var file in addFilesCommand.Files)
            {
                var fileValidationResult = _fileByOptionsValidator.Validate(file);

                if(fileValidationResult.IsFailed)
                {
                    return fileValidationResult;
                }
            }

            return Result.Ok(true);
        }
    }
}

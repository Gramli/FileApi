using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Commands;
using File.Domain.Options;
using FluentResults;
using Microsoft.Extensions.Options;
using Validot;

namespace File.Core.Validation
{
    internal class AddFilesCommandValidator : IAddFilesCommandValidator
    {
        private readonly IValidator<AddFilesCommand> _addFilesCommandValidator;
        private readonly IOptions<FilesOptions> _fileOptions;
        public AddFilesCommandValidator(IValidator<AddFilesCommand> addFilesCommandValidator, IOptions<FilesOptions> fileOptions)
        {
            _addFilesCommandValidator = Guard.Against.Null(addFilesCommandValidator);
            _fileOptions = Guard.Against.Null(fileOptions);
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
                var options = _fileOptions.Value.AllowedFiles.SingleOrDefault(x => x.ContentType.Equals(file.ContentType));
                if(options is null)
                {
                    return Result.Fail($"Unsuported format. File:{file.Name}, contentType: {file.ContentType}"); //TODO
                }

                if(file.Length > _fileOptions.Value.MaxFileLength) 
                {
                    return Result.Fail($"Maximum file size exceeded. File:{file.Name}"); //TODO
                }
            }

            return Result.Ok(true);
        }
    }
}

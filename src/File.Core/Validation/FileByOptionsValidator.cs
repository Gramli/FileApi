using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Core.Resources;
using File.Domain.Abstractions;
using File.Domain.Commands;
using File.Domain.Options;
using FluentResults;
using Microsoft.Extensions.Options;

namespace File.Core.Validation
{
    internal class FileByOptionsValidator : IFileByOptionsValidator
    {
        private readonly IOptions<FilesOptions> _fileOptions;
        public FileByOptionsValidator(IOptions<FilesOptions> fileOptions)
        {
            _fileOptions = Guard.Against.Null(fileOptions);
        }

        public Result<bool> Validate(IFile file)
        {
            var options = _fileOptions.Value.AllowedFiles.SingleOrDefault(x => x.ContentType.Equals(file.ContentType));
            if (options is null)
            {
                return Result.Fail(string.Format(ValidationErrorMessages.UnsupportedFormat, file.FileName, file.ContentType));
            }

            if (file.Length > _fileOptions.Value.MaxFileLength)
            {
                return Result.Fail(string.Format(ValidationErrorMessages.MaximalFileSize, file.FileName));
            }

            return Result.Ok(true);
        }

        public Result<bool> Validate(string extension)
        {
            var options = _fileOptions.Value.AllowedFiles.SingleOrDefault(x => x.Format.Equals(extension));
            if (options is null)
            {
                return Result.Fail(string.Format(ValidationErrorMessages.UnsupportedExtension, extension));
            }

            return Result.Ok(true);
        }
    }
}

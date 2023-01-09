using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Queries;
using FluentResults;
using Validot;

namespace File.Core.Validation
{
    internal class ExportFileQueryValidator : IExportFileQueryValidator
    {
        private readonly IValidator<ExportFileQuery> _exportFileQueryValidator;
        private readonly IFileByOptionsValidator _fileByOptionsValidator;

        public ExportFileQueryValidator(IValidator<ExportFileQuery> exportFileQueryValidator, IFileByOptionsValidator fileByOptionsValidator)
        {
            _exportFileQueryValidator = Guard.Against.Null(exportFileQueryValidator);
            _fileByOptionsValidator = Guard.Against.Null(fileByOptionsValidator);
        }

        public Result<bool> Validate(ExportFileQuery exportFileQuery)
        {
            var validationResult = _exportFileQueryValidator.Validate(exportFileQuery);
            if (validationResult.AnyErrors)
            {
                return Result.Fail(validationResult.ToString());
            }

            var fileValidationResult = _fileByOptionsValidator.Validate(exportFileQuery.Extension);

            if (fileValidationResult.IsFailed)
            {
                return fileValidationResult;
            }

            return Result.Ok(true);
        }
    }
}

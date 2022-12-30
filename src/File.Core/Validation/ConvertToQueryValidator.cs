using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Core.Queries;
using FluentResults;
using Validot;

namespace File.Core.Validation
{
    internal class ConvertToQueryValidator : IConvertToQueryValidator
    {
        private readonly IValidator<ConvertToQuery> _convertToQueryValidator;
        private readonly IFileByOptionsValidator _fileByOptionsValidator;
        public ConvertToQueryValidator(IValidator<ConvertToQuery> convertToQueryValidator, IFileByOptionsValidator fileByOptionsValidator)
        {
            _convertToQueryValidator = Guard.Against.Null(convertToQueryValidator);
            _fileByOptionsValidator = Guard.Against.Null(fileByOptionsValidator);
        }

        public Result<bool> Validate(ConvertToQuery convertToQuery)
        {
            var validationResult = _convertToQueryValidator.Validate(convertToQuery);
            if (validationResult.AnyErrors)
            {
                return Result.Fail(validationResult.ToString());
            }

            var fileValidationResult = _fileByOptionsValidator.Validate(convertToQuery.File);

            if (fileValidationResult.IsFailed)
            {
                return fileValidationResult;
            }

            return Result.Ok(true);
        }
    }
}

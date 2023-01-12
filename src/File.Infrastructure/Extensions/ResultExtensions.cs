using File.Infrastructure.Resources;
using FluentResults;

namespace File.Infrastructure.Extensions
{
    internal static class ResultExtensions
    {
        //TODO BETTER SOL?
        public static Result<string> OkIfNotNull(this string value) 
        {
            return string.IsNullOrEmpty(value) ? 
                Result.Fail<string>(ErrorMessages.ConversionFailed) : 
                Result.Ok(value);
        }
    }
}

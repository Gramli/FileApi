using File.Core.Queries;
using FluentResults;

namespace File.Core.Abstractions
{
    public interface IConvertToQueryValidator
    {
        Result<bool> Validate(ConvertToQuery convertToQuery);
    }
}

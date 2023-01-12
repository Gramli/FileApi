using FluentResults;

namespace File.Infrastructure.Abstractions
{
    internal interface IFileConverter
    {
        Task<Result<string>> Convert(string fileContent, CancellationToken cancellationToken); 
    }
}

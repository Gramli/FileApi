using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Abstractions;
using File.Domain.Dtos;
using File.Infrastructure.Abstractions;
using FluentResults;

namespace File.Infrastructure.FileConversions
{
    internal class FileConversionService : IFileConvertService
    {
        private readonly IFileConverterFactory _fileConverterFactory;

        public FileConversionService(IFileConverterFactory fileConverterFactory)
        {
            _fileConverterFactory = Guard.Against.Null(fileConverterFactory);
        }

        public Task<Result<IFile>> ConvertTo(IFile sourceFile, string destinationFormat)
        {
            throw new NotImplementedException();
        }
    }
}

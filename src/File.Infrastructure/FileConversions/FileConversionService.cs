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

        public async Task<Result<IFile>> ConvertTo(IFile sourceFile, string destinationFormat, CancellationToken cancellationToken)
        {
            var converter = _fileConverterFactory.Create(Path.GetExtension(sourceFile.FileName), destinationFormat);
            return await converter.Convert(await sourceFile.GetData(), cancellationToken);
        }

        public async Task<Result<IFile>> ExportTo(FileDto sourceFile, string destinationFormat, CancellationToken cancellationToken)
        {
            var converter = _fileConverterFactory.Create(Path.GetExtension(sourceFile.FileName), destinationFormat);
            return await converter.Convert(sourceFile.Data, cancellationToken);
        }
    }
}

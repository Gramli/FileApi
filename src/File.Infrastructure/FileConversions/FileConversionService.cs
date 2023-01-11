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
        private readonly IEncodingFactory _encodingFactory;

        public FileConversionService(IFileConverterFactory fileConverterFactory, IEncodingFactory encodingFactory)
        {
            _fileConverterFactory = Guard.Against.Null(fileConverterFactory);
            _encodingFactory = Guard.Against.Null(encodingFactory);
        }

        public async Task<Result<IFile>> ConvertTo(IFile sourceFile, string destinationExtension, CancellationToken cancellationToken)
        {
            var data = await sourceFile.GetData();
            return await ConvertFile(data, sourceFile.FileName, sourceFile.ContentType, destinationExtension, cancellationToken);
        }

        public async Task<Result<IFile>> ExportTo(FileDto sourceFile, string destinationExtension, CancellationToken cancellationToken)
        {
            return await ConvertFile(sourceFile.Data, sourceFile.FileName, sourceFile.ContentType, destinationExtension, cancellationToken);
        }

        private async Task<Result<IFile>> ConvertFile(byte[] data, string fileName, string contentType, string destinationExtension, CancellationToken cancellationToken)
        {
            var encoding = _encodingFactory.CreateEncoding(data);

            var converter = _fileConverterFactory.Create(Path.GetExtension(fileName).Substring(1), destinationExtension);
            var convertedContent = await converter.Convert(encoding.GetString(data), cancellationToken);

            var convertedData = encoding.GetBytes(convertedContent);
            var convertedFileName = $"{Path.GetFileNameWithoutExtension(fileName)}.{destinationExtension}";

            return Result.Ok<IFile>(new ConvertedFile(convertedFileName, contentType, convertedData));
        }
    }
}

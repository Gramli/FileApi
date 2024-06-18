using File.Domain.Abstractions;
using File.Domain.Dtos;

namespace File.Core.Extensions
{
    internal static class FileExtensions
    {
        public async static Task<FileDto> CreateFileDto(this IFileProxy file, CancellationToken cancellationToken)
        {
            return new FileDto
            {
                ContentType = file.ContentType,
                Data = await file.GetData(cancellationToken),
                FileName = Path.GetFileName(file.FileName),
                Length = file.Length
            };
        }
    }
}

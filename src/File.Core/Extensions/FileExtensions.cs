using File.Domain.Abstractions;
using File.Domain.Dtos;

namespace File.Core.Extensions
{
    internal static class FileExtensions
    {
        public async static Task<FileDto> CreateFileDto(this IFile file, CancellationToken cancellationToken)
        {
            var data = await file.GetData(cancellationToken);

            return new FileDto
            {
                ContentType = file.ContentType,
                Data = data,
                FileName = file.FileName,
                Name = file.FileName,
                Length = data.Length
            };
        }
    }
}

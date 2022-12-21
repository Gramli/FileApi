using Ardalis.GuardClauses;
using File.Domain.Abstractions;

namespace File.Infrastructure.FileConversions
{
    internal class ConvertedFile : IFile
    {
        public string FileName { get; }

        public string ContentType { get; }

        public long Length => _data.Length;

        private readonly byte[] _data;

        public ConvertedFile(string fileName, string contentType, byte[] data)
        {
            _data = Guard.Against.Null(data);
            FileName = Guard.Against.NullOrEmpty(fileName);
            ContentType = Guard.Against.NullOrEmpty(contentType);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            await target.WriteAsync(_data, cancellationToken);
        }

        public Task<byte[]> GetData(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_data);
        }
    }
}

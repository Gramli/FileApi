using Ardalis.GuardClauses;
using File.Domain.Abstractions;

namespace File.API.Files
{
    internal sealed class FormFileProxy : IFileProxy
    {
        private readonly IFormFile _formFile;
        public string ContentType => _formFile.ContentType;

        public long Length => _formFile.Length;

        public string FileName => _formFile.FileName;

        public FormFileProxy(IFormFile formFile)
        {
            _formFile = Guard.Against.Null(formFile);
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            return _formFile.CopyToAsync(target, cancellationToken);
        }

        public async Task<byte[]> GetData(CancellationToken cancellationToken = default)
        {
            using var stream = new MemoryStream();
            await CopyToAsync(stream, cancellationToken);
            return stream.ToArray();
        }
    }
}

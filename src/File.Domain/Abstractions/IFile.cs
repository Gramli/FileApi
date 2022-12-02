namespace File.Domain.Abstractions
{
    public interface IFile
    {
        string Name { get; }
        string FileName { get; }
        string ContentType { get; }
        long Length { get; }
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
        Task<byte[]> GetData(CancellationToken cancellationToken = default);
    }
}

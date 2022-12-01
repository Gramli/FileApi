namespace File.Domain.Abstractions
{
    public interface IFile
    {   
        string ContentType { get; }
        long Length { get; }
        string Name { get; }
        string FileName { get; }
        Task CopyToAsync(Stream target, CancellationToken cancellationToken = default);
        Task<byte[]> GetData(CancellationToken cancellationToken = default);
    }
}

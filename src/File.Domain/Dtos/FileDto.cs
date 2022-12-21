namespace File.Domain.Dtos
{
    public class FileDto
    {
        public string FileName { get; init; } = string.Empty;

        public string ContentType { get; init; } = string.Empty;

        public long Length { get; init; }

        public byte[] Data { get; init; } = new byte[0];
    }
}

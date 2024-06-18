namespace File.Domain.Dtos
{
    public abstract class BaseFileDto
    {
        public string FileName { get; init; } = string.Empty;

        public string ContentType { get; init; } = string.Empty;

        public long Length { get; init; }
    }
}

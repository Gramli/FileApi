namespace File.Domain.Dtos
{
    public sealed class FileDto
    {
        public string Name { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
    }
}

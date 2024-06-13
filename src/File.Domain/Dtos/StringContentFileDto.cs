namespace File.Domain.Dtos
{
    public sealed class StringContentFileDto : BaseFileDto
    {
        public string Data { get; init; } = string.Empty;
    }
}

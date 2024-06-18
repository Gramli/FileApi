namespace File.Domain.Dtos
{
    public class FileDto : BaseFileDto
    {
        public byte[] Data { get; init; } = [];
    }
}

namespace File.Infrastructure.Database.EFContext.Entities
{
    internal class FileEntity
    {
        public int Id { get; set; }
        public string FileName { get; init; } = string.Empty;
        public string ContentType { get; init; } = string.Empty;
        public byte[] Data { get; set; } = new byte[0];
    }
}

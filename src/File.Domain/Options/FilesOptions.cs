namespace File.Domain.Options
{
    public sealed class FilesOptions
    {
        public const string Files= "Files";

        public uint MaxFileLength { get; set; }

        public AllowedFile[] AllowedFiles { get; set; } = new AllowedFile[0];
    }
}

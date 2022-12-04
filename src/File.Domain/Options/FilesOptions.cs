namespace File.Domain.Options
{
    public class FilesOptions
    {
        public uint MaxFileLength { get; set; }

        public AllowedFile[] AllowedFiles { get; set; } = new AllowedFile[0];
    }
}

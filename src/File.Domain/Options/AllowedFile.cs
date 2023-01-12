namespace File.Domain.Options
{
    public sealed class AllowedFile
    {
        public string Format { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public string[] CanBeExportedTo { get; set; } = new string[0];
    }
}

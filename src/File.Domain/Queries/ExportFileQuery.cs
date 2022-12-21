namespace File.Domain.Queries
{
    public sealed class ExportFileQuery
    {
        public int Id { get; init; }
        public string Extension { get; init; } = string.Empty;
    }
}

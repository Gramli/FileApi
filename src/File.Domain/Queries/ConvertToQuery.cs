using File.Domain.Abstractions;

namespace File.Core.Queries
{
    public sealed class ConvertToQuery
    {
        public IFile File { get; init; }

        public string FormatToExport { get; init; } = string.Empty;

        public ConvertToQuery(IFile file, string formatToExport)
        {
            File = file;
            FormatToExport = formatToExport;
        }
    }
}

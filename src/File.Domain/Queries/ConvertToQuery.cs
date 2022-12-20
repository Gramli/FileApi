using File.Domain.Abstractions;

namespace File.Core.Queries
{
    public sealed class ConvertToQuery
    {
        public IFile File { get; init; }

        public string FormatToConvert{ get; init; } = string.Empty;

        public ConvertToQuery(IFile file, string formatToConvert)
        {
            File = file;
            FormatToConvert = formatToConvert;
        }
    }
}

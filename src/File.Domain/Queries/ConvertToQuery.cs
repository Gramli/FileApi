using File.Domain.Abstractions;

namespace File.Core.Queries
{
    public sealed class ConvertToQuery
    {
        public IFileProxy File { get; init; }

        public string ExtensionToConvert{ get; init; } = string.Empty;

        public ConvertToQuery(IFileProxy file, string extensionToConvert)
        {
            File = file;
            ExtensionToConvert = extensionToConvert;
        }
    }
}

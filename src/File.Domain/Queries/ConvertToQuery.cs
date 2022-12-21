using File.Domain.Abstractions;

namespace File.Core.Queries
{
    public sealed class ConvertToQuery
    {
        public IFile File { get; init; }

        public string ExtensionToConvert{ get; init; } = string.Empty;

        public ConvertToQuery(IFile file, string extensionToConvert)
        {
            File = file;
            ExtensionToConvert = extensionToConvert;
        }
    }
}

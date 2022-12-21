using File.Infrastructure.Abstractions;
using LunarLabs.Parser.JSON;
using LunarLabs.Parser.XML;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class XmlToJsonFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var root = XMLReader.ReadFromString(fileContent);
            cancellationToken.ThrowIfCancellationRequested();
            var jsonContent = JSONWriter.WriteToString(root);
            return Task.FromResult(jsonContent);
        }
    }
}

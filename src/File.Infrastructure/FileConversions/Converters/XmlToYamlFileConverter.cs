using File.Infrastructure.Abstractions;
using LunarLabs.Parser.XML;
using LunarLabs.Parser.YAML;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class XmlToYamlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var root = XMLReader.ReadFromString(fileContent);
            cancellationToken.ThrowIfCancellationRequested();
            var jsonContent = YAMLWriter.WriteToString(root);
            return Task.FromResult(jsonContent);
        }
    }
}

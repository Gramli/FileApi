using File.Infrastructure.Abstractions;
using LunarLabs.Parser.JSON;
using LunarLabs.Parser.YAML;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class YamlToJsonFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var root = YAMLReader.ReadFromString(fileContent);
            cancellationToken.ThrowIfCancellationRequested();
            var jsonContent = JSONWriter.WriteToString(root);
            return Task.FromResult(jsonContent);
        }
    }
}

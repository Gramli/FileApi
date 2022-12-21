using File.Infrastructure.Abstractions;
using LunarLabs.Parser.JSON;
using LunarLabs.Parser.YAML;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class JsonToYamlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var root = JSONReader.ReadFromString(fileContent);
            cancellationToken.ThrowIfCancellationRequested();
            var jsonContent = YAMLWriter.WriteToString(root);
            return Task.FromResult(jsonContent);
        }
    }
}

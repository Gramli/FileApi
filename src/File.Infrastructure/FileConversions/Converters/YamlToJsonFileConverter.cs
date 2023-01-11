using ChoETL;
using File.Infrastructure.Abstractions;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class YamlToJsonFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var stringReader = new StringReader(fileContent);
            using var yamlReader = new ChoYamlReader(stringReader);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder);
            yamlWriter.Write(yamlReader);
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

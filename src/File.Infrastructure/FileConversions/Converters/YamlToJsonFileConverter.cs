using ChoETL;
using File.Infrastructure.Abstractions;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class YamlToJsonFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var yamlReader = ChoYamlReader.LoadText(fileContent);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder);
            yamlWriter.Write(yamlReader);
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

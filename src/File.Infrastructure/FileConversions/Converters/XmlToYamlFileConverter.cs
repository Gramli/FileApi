using ChoETL;
using File.Infrastructure.Abstractions;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class XmlToYamlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var xmlCoReader = ChoXmlReader.LoadText(fileContent);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder);
            yamlWriter.Write(xmlCoReader);
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

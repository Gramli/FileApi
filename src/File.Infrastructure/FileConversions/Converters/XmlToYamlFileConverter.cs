using ChoETL;
using File.Infrastructure.Abstractions;
using System.Text;
using System.Xml;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class XmlToYamlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var textReader = new StringReader(fileContent);
            using var xmlReader = XmlReader.Create(textReader);
            using var xmlCoReader = new ChoXmlReader(xmlReader);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder);
            yamlWriter.Write(xmlCoReader);
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

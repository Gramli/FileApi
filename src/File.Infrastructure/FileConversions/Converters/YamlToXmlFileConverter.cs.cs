using ChoETL;
using File.Infrastructure.Abstractions;
using SharpYaml.Model;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class YamlToXmlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var stringReader = new StringReader(fileContent);
            using var yamlReader = new ChoYamlReader(stringReader);
            var stringBuilder = new StringBuilder();
            using var xmlWriter = new ChoXmlWriter();
            xmlWriter.Write(yamlReader);
            var result = stringBuilder.ToString();
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

using ChoETL;
using File.Infrastructure.Abstractions;
using Newtonsoft.Json;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class JsonToYamlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var textReader = new StringReader(fileContent);
            using var jsonTextReader = new JsonTextReader(textReader);
            using var jsonCoReader = new ChoJSONReader(jsonTextReader);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder).SingleDocument();
            yamlWriter.Write(jsonCoReader);
            return Task.FromResult(stringBuilder.ToString());
        }
    }
}

using ChoETL;
using File.Infrastructure.Abstractions;
using File.Infrastructure.Extensions;
using FluentResults;
using System.Text;

namespace File.Infrastructure.FileConversions.Converters
{
    internal sealed class YamlToJsonFileConverter : IFileConverter
    {
        public Task<Result<string>> Convert(string fileContent, CancellationToken cancellationToken)
        {
            using var yamlReader = ChoYamlReader.LoadText(fileContent);
            var stringBuilder = new StringBuilder();
            using var yamlWriter = new ChoYamlWriter(stringBuilder);
            yamlWriter.Write(yamlReader);
            var jsonContent = stringBuilder.ToString();
            return Task.FromResult(jsonContent.OkIfNotNull());
        }
    }
}

using File.Infrastructure.Abstractions;
using File.Infrastructure.Extensions;
using FluentResults;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace File.Infrastructure.FileConversions.Converters
{
    internal sealed class YamlToJsonFileConverter : IFileConverter
    {
        public Task<Result<string>> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var deserializer = new Deserializer();
            var yamlObject = deserializer.Deserialize(fileContent);

            var jsonSerializer = new JsonSerializer();
            using var writer = new StringWriter();
            jsonSerializer.Serialize(writer, yamlObject);

            return Task.FromResult(writer.ToString().OkIfNotNull());
        }
    }
}

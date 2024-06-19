using File.Infrastructure.Abstractions;
using File.Infrastructure.Extensions;
using FluentResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Dynamic;

namespace File.Infrastructure.FileConversions.Converters
{
    internal sealed class JsonToYamlFileConverter : IFileConverter
    {
        public Task<Result<string>> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var expConverter = new ExpandoObjectConverter();
            var deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(fileContent, expConverter);

            var serializer = new YamlDotNet.Serialization.Serializer();
            var yamlContent = serializer.Serialize(deserializedObject);
            return Task.FromResult(yamlContent.OkIfNotNull());
        }
    }
}

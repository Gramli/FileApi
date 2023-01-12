using File.Infrastructure.Abstractions;
using File.Infrastructure.Resources;
using FluentResults;
using Newtonsoft.Json;

namespace File.Infrastructure.FileConversions.Converters
{
    internal sealed class JsonToXmlFileConverter : IFileConverter
    {
        private static readonly string _rootElement = "rootElement";
        public Task<Result<string>> Convert(string jsonString, CancellationToken cancellationToken)
        {
            var doc = JsonConvert.DeserializeXmlNode(jsonString, _rootElement);
            if(doc is null)
            {
                return Task.FromResult(Result.Fail<string>(ErrorMessages.ConversionFailed));
            }
            return Task.FromResult(Result.Ok(doc.OuterXml));
        }
    }
}

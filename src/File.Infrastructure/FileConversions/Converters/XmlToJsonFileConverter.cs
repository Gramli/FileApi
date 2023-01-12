using File.Infrastructure.Abstractions;
using File.Infrastructure.Extensions;
using FluentResults;
using Newtonsoft.Json;
using System.Xml;

namespace File.Infrastructure.FileConversions.Converters
{
    internal sealed class XmlToJsonFileConverter : IFileConverter
    {
        public Task<Result<string>> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var doc = new XmlDocument();
            doc.LoadXml(fileContent);
            var jsonText = JsonConvert.SerializeXmlNode(doc);
            return Task.FromResult(jsonText.OkIfNotNull());
        }
    }
}

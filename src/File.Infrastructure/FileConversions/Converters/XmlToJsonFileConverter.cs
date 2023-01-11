using File.Infrastructure.Abstractions;
using Newtonsoft.Json;
using System.Xml;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class XmlToJsonFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var doc = new XmlDocument();
            doc.LoadXml(fileContent);
            var jsonText = JsonConvert.SerializeXmlNode(doc);
            return Task.FromResult(jsonText);
        }
    }
}

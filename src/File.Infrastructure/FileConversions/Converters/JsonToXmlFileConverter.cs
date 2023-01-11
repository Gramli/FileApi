using File.Infrastructure.Abstractions;
using Newtonsoft.Json;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class JsonToXmlFileConverter : IFileConverter
    {
        public Task<string> Convert(string jsonString, CancellationToken cancellationToken)
        {
            var doc = JsonConvert.DeserializeXmlNode(jsonString);
            if(doc is null)
            {
                throw new ArgumentException(nameof(jsonString));
            }
            return Task.FromResult(doc.OuterXml);
        }
    }
}

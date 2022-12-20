using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;

namespace File.Infrastructure.FileConversions
{
    internal class FileConverterFactory : IFileConverterFactory
    {
        private readonly Dictionary<(string,string), IFileConverter> _converters = new Dictionary<(string, string), IFileConverter>
        {
            {("json","xml") ,new JsonToXmlFileConverter() },
            {("json","yml") ,new JsonToYamlFileConverter() },
            {("yml","xml") ,new YamlToXmlFileConverter() },
            {("yml","json") ,new YamlToJsonFileConverter() },
            {("xml","json") ,new XmlToJsonFileConverter() },
            {("xml","yml") ,new XmlToYamlFileConverter() }
        };

        public IFileConverter Create(string sourceFormat, string destinationFormat)
        {
            if(!_converters.TryGetValue((sourceFormat, destinationFormat), out var fileConverter))
            {
                throw new NotSupportedException($"Not supported conversion: source: {sourceFormat}, destination: {destinationFormat}");
            }

            return fileConverter;
        }
    }
}

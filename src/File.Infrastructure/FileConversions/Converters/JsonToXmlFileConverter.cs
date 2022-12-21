using File.Infrastructure.Abstractions;
using LunarLabs.Parser.JSON;
using LunarLabs.Parser.XML;

namespace File.Infrastructure.FileConversions.Converters
{
    internal class JsonToXmlFileConverter : IFileConverter
    {
        public Task<string> Convert(string fileContent, CancellationToken cancellationToken)
        {
            var root = JSONReader.ReadFromString(fileContent);
            cancellationToken.ThrowIfCancellationRequested();
            var jsonContent = XMLWriter.WriteToString(root);
            return Task.FromResult(jsonContent);
        }
    }
}

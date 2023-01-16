using File.Infrastructure.FileConversions.Converters;
using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class ValidConverters : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "json", "xml", typeof(JsonToXmlFileConverter) };
            yield return new object[] { "json", "yaml", typeof(JsonToYamlFileConverter) };
            yield return new object[] { "yaml", "json", typeof(YamlToJsonFileConverter) };
            yield return new object[] { "xml", "json", typeof(XmlToJsonFileConverter) };
            yield return new object[] { "xml", "yaml", typeof(XmlToYamlFileConverter) };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

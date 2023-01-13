using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using File.Infrastructure.UnitTests.Assets;
using Newtonsoft.Json;
using System.Xml;

namespace File.Infrastructure.UnitTests.FileConversions.Converters
{
    public class XmlToJsonFileConverterTests
    {
        private readonly IFileConverter _uut;

        public XmlToJsonFileConverterTests()
        {
            _uut = new XmlToJsonFileConverter();
        }

        [Fact]
        public async Task Empty_Xml()
        {
            //Arrange
            //Act
            var result = await _uut.Convert("<root />", CancellationToken.None);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Theory]
        [ClassData(typeof(InvalidXmlData))]
        public async Task Invalid_Xml(string invalidXml)
        {
            //Act & Assert
            await Assert.ThrowsAsync<XmlException>(() => _uut.Convert(invalidXml, CancellationToken.None));
        }
    }
}

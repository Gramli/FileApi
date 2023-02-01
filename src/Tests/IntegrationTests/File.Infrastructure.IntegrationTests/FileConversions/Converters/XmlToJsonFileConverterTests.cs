using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using File.Infrastructure.IntegrationTests.Extensions;

namespace File.Infrastructure.IntegrationTests.FileConversions.Converters
{
    public class XmlToJsonFileConverterTests
    {
        private readonly IFileConverter _uut;

        public XmlToJsonFileConverterTests()
        {
            _uut = new XmlToJsonFileConverter();
        }

        [Fact]
        public async Task Convert_Success()
        {
            //Arrange
            using var fileStream = new FileStream("Assets/new.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fileStream);
            //Act
            var result = await _uut.Convert(reader.ReadToEnd(), CancellationToken.None);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
            AssertExtensions.EqualFileContent("Assets/root.json", result.Value);
        }
    }
}

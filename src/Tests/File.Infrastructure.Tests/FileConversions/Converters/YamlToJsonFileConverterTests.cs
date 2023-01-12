using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using System.Globalization;

namespace File.Infrastructure.UnitTests.FileConversions.Converters
{
    public class YamlToJsonFileConverterTests
    {
        private readonly IFileConverter _uut;

        public YamlToJsonFileConverterTests()
        {
            _uut = new YamlToJsonFileConverter();
        }

        [Fact]
        public async Task Convert_Success()
        {
            //Arrange
            using var fileStream = new FileStream("Assets/new.yaml", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fileStream);
            //Act
            var result = await _uut.Convert(reader.ReadToEnd(), CancellationToken.None);
            //Assert
            Assert.NotEmpty(result);
        }
    }
}

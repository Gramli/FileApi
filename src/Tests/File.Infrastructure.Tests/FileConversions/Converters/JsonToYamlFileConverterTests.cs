using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using System.Globalization;

namespace File.Infrastructure.Tests.FileConversions.Converters
{
    public class JsonToYamlFileConverterTests
    {
        private readonly IFileConverter _uut;

        public JsonToYamlFileConverterTests()
        {
            _uut = new JsonToYamlFileConverter();
        }

        [Fact]
        public async Task Convert_Success()
        {
            //Arrange
            using var fileStream = new FileStream("Assets/new.json", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fileStream);
            //Act
            var result = await _uut.Convert(reader.ReadToEnd(), CancellationToken.None);
            //Assert
            Assert.NotEmpty(result);
            Assert.Equal(0, string.Compare(result,
                System.IO.File.ReadAllText("Assets/new.yaml"),
                CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols));
        }
    }
}

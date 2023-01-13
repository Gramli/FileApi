using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using File.Infrastructure.UnitTests.Assets;
using Newtonsoft.Json;

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
        public async Task Empty_Json()
        {
            //Arrange
            var emptyJson = "{}";
            //Act
            var result = await _uut.Convert(emptyJson, CancellationToken.None);
            //Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [ClassData(typeof(InvalidJsonData))]
        public async Task Invalid_Json(string invalidJson)
        {
            //Act & Assert
            await Assert.ThrowsAsync<JsonReaderException>(() => _uut.Convert(invalidJson, CancellationToken.None));
        }
    }
}

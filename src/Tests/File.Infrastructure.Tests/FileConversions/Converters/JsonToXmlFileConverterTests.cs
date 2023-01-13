using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using File.Infrastructure.UnitTests.Assets;
using Newtonsoft.Json;

namespace File.Infrastructure.UnitTests.FileConversions.Converters
{
    public class JsonToXmlFileConverterTests
    {
        private readonly IFileConverter _uut;

        public JsonToXmlFileConverterTests()
        {
            _uut = new JsonToXmlFileConverter();
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
            Assert.Equal("<rootElement />", result.Value);
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

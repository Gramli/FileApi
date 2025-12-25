using File.API.SystemTests.Extensions;

namespace File.API.SystemTests.Tests
{
    public class ConvertTests : SystemTestsBase
    {
        [Fact]
        public async Task ConvertFileAsync_ToXml()
        {
            //Arrange
            using var content = new MultipartFormDataContent().AddFileFromAssets("new.json");

            //Act
            using var response = await _httpClient.PostAsync("v1/file/convert?formatToExport=xml", content);
            using var stream = await response.Content.ReadAsStreamAsync();

            //Assert
            Assert.True(stream.Length > 0);
        }
    }
}

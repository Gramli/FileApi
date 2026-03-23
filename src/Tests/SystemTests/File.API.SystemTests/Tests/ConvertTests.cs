using File.API.SystemTests.Extensions;
using Xunit.Abstractions;

namespace File.API.SystemTests.Tests
{
    public class ConvertTests : SystemTestsBase
    {
        private readonly ITestOutputHelper _output;

        public ConvertTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task ConvertFileAsync_ToXml()
        {
            //Arrange
            using var content = new MultipartFormDataContent().AddFileFromAssets("new.json");
            _output.WriteLine("File uploaded successfully, starting conversion.. Content: {0}", content.Headers.ToString());
            //Act
            using var response = await _httpClient.PostAsync("v1/file/convert?formatToExport=xml", content);
            _output.WriteLine("Conversion response received. Status Code: {0}, Headers: {1}", response.StatusCode, response.Headers.ToString());
            using var stream = await response.Content.ReadAsStreamAsync();
            _output.WriteLine("Stream length after conversion: {0} bytes", stream.Length);
            //Assert
            Assert.True(stream.Length > 0);
        }
    }
}

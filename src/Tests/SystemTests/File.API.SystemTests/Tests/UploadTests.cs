using File.API.SystemTests.Extensions;

namespace File.API.SystemTests.Tests
{
    public class UploadTests : SystemTestsBase
    {
        [Fact]
        public async Task UploadFileAsync()
        {
            //Arrange
            //Act
            var result = await _httpClient.UploadAssetsFile("new.json");

            //Assert
            var resultData = await result.GetResponseData<bool>();
            Assert.True(resultData);
        }
    }
}

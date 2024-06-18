using File.API.SystemTests.Extensions;
using SmallApiToolkit.Core.Response;

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
            var resultData = await result.GetResponseData<DataResponse<bool>>();
            Assert.True(resultData.Data);
        }
    }
}

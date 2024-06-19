using File.API.SystemTests.Extensions;
using File.Domain.Dtos;
using SmallApiToolkit.Core.Response;

namespace File.API.SystemTests.Tests
{
    public class GetTests : SystemTestsBase
    {
        [Fact]
        public async Task GetFileInfoAsync()
        {
            //Arrange
            (await _httpClient.UploadAssetsFile("new.json"))
                .EnsureSuccessStatusCode();

            //Act
            var result = await _httpClient.GetAsync("file/v1/files-info");

            //Assert
            var resultData = await result.GetResponseData<DataResponse<IEnumerable<FileInfoDto>>>();
            Assert.NotNull(resultData);
            Assert.NotEmpty(resultData.Data);
        }
    }
}

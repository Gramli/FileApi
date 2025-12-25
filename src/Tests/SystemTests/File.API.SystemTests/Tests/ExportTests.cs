using File.API.SystemTests.Extensions;
using File.Domain.Dtos;
using File.Domain.Queries;
using Newtonsoft.Json;
using SmallApiToolkit.Core.Response;
using System.Text;

namespace File.API.SystemTests.Tests
{
    public class ExportTests : SystemTestsBase
    {
        [Fact]
        public async Task ExportFileAsync_ToXml()
        {
            //Arrange
            using var uploadResponse = (await _httpClient.UploadAssetsFile("new.json"))
                .EnsureSuccessStatusCode();

            using var fileInfo = await _httpClient.GetAsync("v1/file");
            var fileToDownload = (await fileInfo.GetResponseData<DataResponse<IEnumerable<FileInfoDto>>>())?.Data?.First();

            if (fileToDownload is null)
            {
                Assert.Fail("Downloaded file is empty.");
            }

            //Act
            using var response = await _httpClient.GetAsync($"v1/file/{fileToDownload.Id}/export?extension=xml");
            using var stream = await response.Content.ReadAsStreamAsync();

            //Assert
            Assert.True(stream.Length > 0);
        }
    }
}

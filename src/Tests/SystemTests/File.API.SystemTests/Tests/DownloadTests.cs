using ChoETL;
using File.API.SystemTests.Extensions;
using File.Domain.Dtos;
using File.Domain.Http;
using System.Net.Http.Json;

namespace File.API.SystemTests.Tests
{
    public class DownloadTests : SystemTestsBase
    {
        [Fact]
        public async Task DownloadFileAsync()
        {
            //Arrange
            using var uploadResponse = (await _httpClient.UploadAssetsFile("new.json"))
                .EnsureSuccessStatusCode();

            using var fileInfo = await _httpClient.GetAsync("file/v1/files-info");
            var fileToDownload = (await fileInfo.GetResponseData<DataResponse<IEnumerable<FileInfoDto>>>())?.Data?.First();

            if(fileToDownload is null)
            {
                Assert.Fail("Downloaded file is empty.");
            }

            //Act
            using var response = await _httpClient.GetAsync($"file/v1/download/?id={fileToDownload.Id}");
            using var stream = await response.Content.ReadAsStreamAsync();

            //Assert
            Assert.True(stream.Length > 0);
        }

        [Fact]
        public async Task DownloadJsonFileAsync()
        {
            //Arrange
            using var uploadResponse = (await _httpClient.UploadAssetsFile("new.json"))
                .EnsureSuccessStatusCode();

            using var fileInfo = await _httpClient.GetAsync("file/v1/files-info");
            var fileToDownload = (await fileInfo.GetResponseData<DataResponse<IEnumerable<FileInfoDto>>>())?.Data?.First();

            if (fileToDownload is null)
            {
                Assert.Fail("Downloaded file is empty.");
            }

            //Act
            using var response = await _httpClient.GetAsync($"file/v1/downloadAsJson/?id={fileToDownload.Id}");
            var responseFile = await response.Content.ReadFromJsonAsync<DataResponse<StringContentFileDto>>();

            //Assert
            Assert.True(responseFile!.Data!.Data.Length > 0);
            Assert.NotEmpty(responseFile!.Data!.FileName);
        }
    }
}

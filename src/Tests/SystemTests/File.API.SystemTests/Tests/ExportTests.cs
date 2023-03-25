using File.API.SystemTests.Extensions;
using File.Domain.Dtos;
using File.Domain.Http;
using File.Domain.Queries;
using Newtonsoft.Json;
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

            using var fileInfo = await _httpClient.GetAsync("file/v1/files-info");
            var fileToDownload = (await fileInfo.GetResponseData<DataResponse<IEnumerable<FileInfoDto>>>())?.Data?.First();

            if (fileToDownload is null)
            {
                Assert.Fail("Downloaded file is empty.");
            }

            var body = JsonConvert.SerializeObject(new ExportFileQuery
            {
                Id = fileToDownload.Id,
                Extension = "xml"
            });
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            //Act
            using var response = await _httpClient.PostAsync($"file/v1/export", content);
            using var stream = await response.Content.ReadAsStreamAsync();

            //Assert
            Assert.True(stream.Length > 0);
        }
    }
}

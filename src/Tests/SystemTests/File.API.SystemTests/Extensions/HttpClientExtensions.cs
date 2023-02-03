namespace File.API.SystemTests.Extensions
{
    internal static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> UploadAssetsFile(this HttpClient httpClient, string fileName)
        {
            using var content = new MultipartFormDataContent().AddFileFromAssets(fileName);
            return await httpClient.PostAsync("file/v1/upload", content);
        }
    }
}

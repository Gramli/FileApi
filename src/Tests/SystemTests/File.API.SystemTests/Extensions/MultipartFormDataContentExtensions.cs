namespace File.API.SystemTests.Extensions
{
    public static class MultipartFormDataContentExtensions
    {
        public static MultipartFormDataContent AddFileFromAssets(this MultipartFormDataContent content, string fileName)
        {
            var stream = new FileStream($"Assets/{fileName}", FileMode.Open, FileAccess.Read, FileShare.Read);
            var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Add(streamContent, "file", fileName);
            return content;
        }
    }
}

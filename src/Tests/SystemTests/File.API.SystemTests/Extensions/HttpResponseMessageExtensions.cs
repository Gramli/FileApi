using Newtonsoft.Json;

namespace File.API.SystemTests.Extensions
{
    internal static class HttpResponseMessageExtensions
    {
        public static async Task<T> GetResponseData<T>(this HttpResponseMessage httpResponseMessage)
        {
            httpResponseMessage.EnsureSuccessStatusCode();
            var resultString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(resultString);
        }
    }
}

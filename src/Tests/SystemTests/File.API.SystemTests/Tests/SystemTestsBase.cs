using Microsoft.AspNetCore.Mvc.Testing;

namespace File.API.SystemTests.Tests
{
    public abstract class SystemTestsBase
    {
        protected readonly HttpClient _httpClient;

        protected SystemTestsBase()
        {
            var application = new WebApplicationFactory<Program>();
            _httpClient = application.CreateClient();
        }
    }
}

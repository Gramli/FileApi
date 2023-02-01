using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace File.API.SystemTests
{
    public abstract class SystemTestsBase
    {
        protected readonly HttpClient _httpClient;

        public SystemTestsBase()
        {
            var application = new WebApplicationFactory<Program>();
            _httpClient = application.CreateClient();
        }
    }
}

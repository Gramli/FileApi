using Microsoft.AspNetCore.StaticFiles;

namespace File.API.Extensions
{
    internal static class FileExtensionContentTypeExtensions
    {
        internal static void AddCustomContentTypes(this WebApplication webApplication)
        {
            var provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".yaml"] = "text/yaml";

            webApplication.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
        }
    }
}

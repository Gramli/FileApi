using Microsoft.Extensions.DependencyInjection;

namespace File.Core.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection)
        {
            return serviceCollection;
        }
    }
}

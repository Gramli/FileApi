using File.Core.Abstractions;
using File.Infrastructure.Abstractions;
using File.Infrastructure.Database.EFContext;
using File.Infrastructure.Database.Repositories;
using File.Infrastructure.FileConversions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace File.Infrastructure.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            return serviceCollection
                .AddDatabase();
        }

        private static IServiceCollection AddConversion(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IEncodingFactory, EncodingFactory>()
                .AddSingleton<IFileConverterFactory, FileConverterFactory>()
                .AddScoped<IFileConvertService, FileConversionService>();
        }

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<FileContext>(opt => opt.UseInMemoryDatabase("Files"))
                .AddScoped<IFileCommandsRepository, FileCommandsRepository>()
                .AddScoped<IFileQueriesRepository, FileQueriesRepository>();
        }
    }
}

using File.Core.Abstractions;
using File.Infrastructure.Database.EFContext;
using File.Infrastructure.Database.Repositories;
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

        private static IServiceCollection AddDatabase(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddDbContext<FileContext>(opt => opt.UseInMemoryDatabase("Files"))
                .AddScoped<IFileCommandsRepository, FileCommandsRepository>()
                .AddScoped<IFileQueriesRepository, FileQueriesRepository>();
        }
    }
}

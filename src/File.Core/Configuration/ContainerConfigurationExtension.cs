using File.Core.Abstractions;
using File.Core.Commands;
using File.Core.Extensions;
using File.Core.Validation;
using File.Domain.Commands;
using File.Domain.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Validot;

namespace File.Core.Configuration
{
    public static class ContainerConfigurationExtension
    {
        public static IServiceCollection AddCore(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<FilesOptions>(configuration.GetSection(FilesOptions.Files));

            return serviceCollection
                .AddCommandHandlers()
                .AddValidation();
        }

        private static IServiceCollection AddCommandHandlers(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IAddFilesCommandHandler, AddFilesCommandHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IAddFilesCommandValidator, AddFilesCommandValidator>()
                .AddValidotSingleton<IValidator<AddFilesCommand>, AddFileCommandSpecificationHolder, AddFilesCommand>();
        }
    }
}

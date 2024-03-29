﻿using File.Core.Abstractions;
using File.Core.Commands;
using File.Core.Extensions;
using File.Core.Queries;
using File.Core.Validation;
using File.Domain.Commands;
using File.Domain.Options;
using File.Domain.Queries;
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
                .AddScoped<IAddFilesCommandHandler, AddFilesCommandHandler>()
                .AddScoped<IGetFilesInfoQueryHandler, GetFilesInfoQueryHandler>()
                .AddScoped<IDownloadFileQueryHandler, DownloadFileQueryHandler>()
                .AddScoped<IExportFileQueryHandler, ExportFileQueryHandler>()
                .AddScoped<IConvertToQueryHandler, ConvertToQueryHandler>();
        }

        private static IServiceCollection AddValidation(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddScoped<IAddFilesCommandValidator, AddFilesCommandValidator>()
                .AddScoped<IConvertToQueryValidator, ConvertToQueryValidator>()
                .AddScoped<IExportFileQueryValidator, ExportFileQueryValidator>()
                .AddScoped<IFileByOptionsValidator, FileByOptionsValidator>()
                .AddValidotSingleton<IValidator<AddFilesCommand>, AddFileCommandSpecificationHolder, AddFilesCommand>()
                .AddValidotSingleton<IValidator<DownloadFileQuery>, DownloadFileQuerySpecificationHolder, DownloadFileQuery>()
                .AddValidotSingleton<IValidator<ConvertToQuery>, ConvertToQuerySpecificationHolder, ConvertToQuery>()
                .AddValidotSingleton<IValidator<ExportFileQuery>, ExportFileQuerySpecificationHolder, ExportFileQuery>();
        }
    }
}

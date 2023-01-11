using File.API.Extensions;
using File.API.Files;
using File.Core.Abstractions;
using File.Core.Queries;
using File.Domain.Commands;
using File.Domain.Dtos;
using File.Domain.Http;
using File.Domain.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace File.API.EndpointBuilders
{
    public static class FileEndpointsBuilder
    {
        public static IEndpointRouteBuilder BuildWeatherEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            return endpointRouteBuilder
                .BuildUploadEndpoints()
                .BuildDownloadEndpoints()
                .BuildGetEndpoints()
                .BuildParseEndpoints()
                .BuildExportEndpoints();
        }

        private static IEndpointRouteBuilder BuildUploadEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("file/upload",
                async (IFormFileCollection files, [FromServices] IAddFilesCommandHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new AddFilesCommand(files.Select(file => new FormFileProxy(file))), cancellationToken))
                        .Produces<bool>()
                        .WithName("AddFiles")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildDownloadEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("file/download/{1}",
                async (int id, [FromServices] IDownloadFileQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(new DownloadFileQuery(id), cancellationToken))
                        .Produces<FileContentHttpResult>()
                        .WithName("DownloadFile")
                        .WithTags("Get");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildGetEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("file/files-info",
                async ([FromServices] IGetFilesInfoQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .Produces<IEnumerable<FileInfoDto>>()
                        .WithName("GetFilesInfo")
                        .WithTags("Get");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildParseEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("file/export",
                async ([AsParameters]ExportFileQuery parseFileQuery,[FromServices] IExportFileQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(parseFileQuery, cancellationToken))
                        .Produces<IEnumerable<FileInfoDto>>()
                        .WithName("Export")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildExportEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("file/convert",
                async (IFormFile file, string formatToExport, [FromServices] IConvertToQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(new ConvertToQuery(new FormFileProxy(file), formatToExport), cancellationToken))
                        .Produces<IEnumerable<FileInfoDto>>()
                        .WithName("ConvertTo")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }
    }
}

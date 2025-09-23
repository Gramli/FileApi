using File.API.Extensions;
using File.API.Files;
using File.Core.Abstractions;
using File.Core.Queries;
using File.Domain.Commands;
using File.Domain.Dtos;
using File.Domain.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Extensions;

namespace File.API.EndpointBuilders
{
    public static class FileEndpointsBuilder
    {
        public static IEndpointRouteBuilder BuildFileEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            return endpointRouteBuilder
                .MapVersionGroup(1)
                .MapGroup("files")
                .BuildUploadEndpoints()
                .BuildDownloadEndpoints()
                .BuildGetEndpoints()
                .BuildParseEndpoints()
                .BuildExportEndpoints();
        }

        private static IEndpointRouteBuilder BuildUploadEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("/upload",
                async (IFormFile file, [FromServices] IAddFilesCommandHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(new AddFilesCommand([new FormFileProxy(file)]), cancellationToken))
                        .DisableAntiforgery()
                        .ProducesDataResponse<bool>()
                        .WithName("AddFiles")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildDownloadEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/{id}/download",
                async (int id, [FromServices] IDownloadFileQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(new DownloadFileQuery(id), cancellationToken))
                        .DisableAntiforgery()
                        .Produces<FileContentHttpResult>()
                        .WithName("DownloadFile")
                        .WithTags("Get");

            endpointRouteBuilder.MapGet("/{id}/download/json",
                async (int id, [FromServices] IDownloadFileQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetJsonFileAsync(new DownloadFileQuery(id), cancellationToken))
                        .DisableAntiforgery()
                        .ProducesDataResponse<StringContentFileDto>()
                        .WithName("DownloadFileAsJson")
                        .WithTags("Get");

            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildGetEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/",
                async ([FromServices] IGetFilesInfoQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(EmptyRequest.Instance, cancellationToken))
                        .ProducesDataResponse<IEnumerable<FileInfoDto>>()
                        .WithName("GetFilesInfo")
                        .WithTags("Get");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildParseEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("{id}/export",
                async (int id, [FromQuery]string extension,[FromServices] IExportFileQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(new ExportFileQuery
                    {
                        Id = id,
                        Extension = extension
                    }, cancellationToken))
                        .DisableAntiforgery()
                        .ProducesDataResponse<FileContentHttpResult>()
                        .WithName("Export")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }

        private static IEndpointRouteBuilder BuildExportEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapPost("/convert",
                async (IFormFile file, [FromForm]string formatToConvert, [FromServices] IConvertToQueryHandler handler, CancellationToken cancellationToken) =>
                    await handler.GetFileAsync(new ConvertToQuery(new FormFileProxy(file), formatToConvert), cancellationToken))
                        .DisableAntiforgery()
                        .ProducesDataResponse<FileContentHttpResult>()
                        .WithName("ConvertTo")
                        .WithTags("Post");
            return endpointRouteBuilder;
        }
    }
}

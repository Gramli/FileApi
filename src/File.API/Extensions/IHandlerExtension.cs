using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Http;
using System.Buffers.Text;

namespace File.API.Extensions
{
    internal static class IHandlerExtension
    {
        internal static async Task<IResult> SendAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        }

        internal static async Task<IResult> GetFileAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken) where TResponse : FileDto
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            if(response.Data is not null)
            {
                return Results.File(response.Data.Data, response.Data.ContentType, response.Data.FileName);
            }
            return Results.NotFound();
        }

        internal static async Task<IResult> GetJsonFileAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken) where TResponse : FileDto
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            if (response.Data is not null)
            {
                return Results.Json(new DataResponse<StringContentFileDto>
                {
                    Errors = response.Errors,
                    Data = new StringContentFileDto
                    {
                        Data = Convert.ToBase64String(response.Data.Data),
                        ContentType = response.Data.ContentType,
                        FileName = response.Data.FileName,
                        Length = response.Data.Length
                    },
                }, statusCode: (int)response.StatusCode);
            }
            return Results.NotFound();
        }
    }
}

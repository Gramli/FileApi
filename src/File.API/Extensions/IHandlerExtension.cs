using File.Core.Abstractions;
using File.Domain.Dtos;

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
    }
}

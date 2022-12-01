using File.Core.Abstractions;
using File.Domain.Abstractions;

namespace File.API.Extensions
{
    internal static class IHandlerExtension
    {
        internal static async Task<IResult> SendAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            return Results.Json(response.Data, statusCode: (int)response.StatusCode);
        }

        internal static async Task<IResult> GetFileAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken) where TResponse : IFile
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            if(response.Data is not null)
            {
                return Results.File(await response.Data.GetData(), response.Data.ContentType, response.Data.Name);
            }
            return Results.NotFound();
        }
    }
}

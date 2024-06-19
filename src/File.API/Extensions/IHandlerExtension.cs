using File.Domain.Dtos;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;

namespace File.API.Extensions
{
    internal static class IHandlerExtension
    {
        internal static async Task<IResult> GetJsonFileAsync<TResponse, TRequest>(this IHttpRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken) where TResponse : FileDto
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

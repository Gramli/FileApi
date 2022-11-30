using File.Domain.Http;

namespace File.Core.Abstractions
{
    public interface IRequestHandler<TResponse, in TRequest> 
    {
        Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}

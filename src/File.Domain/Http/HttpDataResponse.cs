using System.Net;

namespace File.Domain.Http
{
    public sealed class HttpDataResponse<T> : DataResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }

    }
}

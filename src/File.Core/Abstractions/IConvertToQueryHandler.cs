using File.Core.Queries;
using File.Domain.Dtos;
using SmallApiToolkit.Core.RequestHandlers;

namespace File.Core.Abstractions
{
    public interface IConvertToQueryHandler : IHttpRequestHandler<FileDto, ConvertToQuery>
    {
    }
}

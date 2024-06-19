using File.Domain.Dtos;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;

namespace File.Core.Abstractions
{
    public interface IGetFilesInfoQueryHandler : IHttpRequestHandler<IEnumerable<FileInfoDto>, EmptyRequest>
    {
    }
}

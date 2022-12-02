using File.Domain.Dtos;
using File.Domain.Http;

namespace File.Core.Abstractions
{
    public interface IGetFilesInfoQueryHandler : IRequestHandler<IEnumerable<FileInfoDto>, EmptyRequest>
    {
    }
}

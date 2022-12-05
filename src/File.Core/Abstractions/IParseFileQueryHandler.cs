using File.Domain.Dtos;
using File.Domain.Queries;

namespace File.Core.Abstractions
{
    public interface IParseFileQueryHandler : IRequestHandler<FileDto, ParseFileQuery>
    {
    }
}

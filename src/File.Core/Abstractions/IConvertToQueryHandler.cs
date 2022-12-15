using File.Core.Queries;
using File.Domain.Dtos;

namespace File.Core.Abstractions
{
    public interface IConvertToQueryHandler : IRequestHandler<FileDto, ConvertToQuery>
    {
    }
}

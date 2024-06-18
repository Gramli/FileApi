using File.Domain.Dtos;
using File.Domain.Queries;
using SmallApiToolkit.Core.RequestHandlers;

namespace File.Core.Abstractions
{
    public interface IExportFileQueryHandler : IHttpRequestHandler<FileDto, ExportFileQuery>
    {
    }
}

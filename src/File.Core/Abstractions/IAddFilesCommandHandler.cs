using File.Domain.Commands;
using SmallApiToolkit.Core.RequestHandlers;

namespace File.Core.Abstractions
{
    public interface IAddFilesCommandHandler : IHttpRequestHandler<bool, AddFilesCommand>
    {
    }
}

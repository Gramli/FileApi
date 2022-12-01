using File.Domain.Commands;

namespace File.Core.Abstractions
{
    public interface IAddFilesCommandHandler : IRequestHandler<bool, AddFilesCommand>
    {
    }
}

using File.Domain.Abstractions;

namespace File.Domain.Commands
{
    public sealed class AddFilesCommand
    {
        public IEnumerable<IFileProxy> Files { get; init; } = [];

        public AddFilesCommand(IEnumerable<IFileProxy> files)
        {
            Files = files;
        }
    }
}

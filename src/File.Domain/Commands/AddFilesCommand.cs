using File.Domain.Abstractions;

namespace File.Domain.Commands
{
    public sealed class AddFilesCommand
    {
        public IEnumerable<IFile> Files { get; init; } = [];

        public AddFilesCommand(IEnumerable<IFile> files)
        {
            Files = files;
        }
    }
}

using File.Domain.Abstractions;

namespace File.Core.Abstractions
{
    public interface IFileCommandsRepository
    {
        Task<int> AddFileAsync(IFile file);
    }
}

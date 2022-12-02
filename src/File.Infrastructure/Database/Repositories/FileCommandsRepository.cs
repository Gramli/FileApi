using File.Core.Abstractions;
using File.Domain.Abstractions;

namespace File.Infrastructure.Database.Repositories
{
    internal class FileCommandsRepository : IFileCommandsRepository
    {
        public Task<int> AddFileAsync(IFile file)
        {
            throw new NotImplementedException();
        }
    }
}

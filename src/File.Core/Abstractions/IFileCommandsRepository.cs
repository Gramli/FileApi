using File.Domain.Abstractions;
using File.Domain.Dtos;

namespace File.Core.Abstractions
{
    public interface IFileCommandsRepository
    {
        Task<int> AddFileAsync(FileDto fileDto, CancellationToken cancellationToken);
    }
}

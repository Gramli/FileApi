using SmallApiToolkit.Core.Abstractions;

namespace File.Domain.Dtos
{
    public class FileDto : BaseFileDto, IFile
    {
        public byte[] Data { get; init; } = [];
    }
}

using File.Core.Abstractions;
using File.Domain.Commands;
using File.Domain.Http;
using Validot;

namespace File.Core.Commands
{
    internal sealed class AddFilesCommandHandler : IAddFilesCommandHandler
    {
        private readonly IValidator<AddFilesCommand> _addFilesCommandValidator;

        public AddFilesCommandHandler()
        {

        }

        public Task<HttpDataResponse<bool>> HandleAsync(AddFilesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

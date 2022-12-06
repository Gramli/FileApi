using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Commands;
using File.Domain.Extensions;
using File.Domain.Http;
using Validot;

namespace File.Core.Commands
{
    internal sealed class AddFilesCommandHandler : IAddFilesCommandHandler
    {
        private readonly IAddFilesCommandValidator _addFilesCommandValidator;

        public AddFilesCommandHandler(IAddFilesCommandValidator addFilesCommandValidator)
        {
            _addFilesCommandValidator = Guard.Against.Null(addFilesCommandValidator);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(AddFilesCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _addFilesCommandValidator.Validate(request);
            if(validationResult.IsFailed)
            {
                return HttpDataResponses.AsBadRequest<bool>(validationResult.Errors.ToErrorMessages());
            }

            throw new NotImplementedException();
        }
    }
}

using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Core.Extensions;
using File.Core.Resources;
using File.Domain.Commands;
using File.Domain.Extensions;
using File.Domain.Logging;
using Microsoft.Extensions.Logging;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.Response;

namespace File.Core.Commands
{
    internal sealed class AddFilesCommandHandler : IAddFilesCommandHandler
    {
        private readonly IAddFilesCommandValidator _addFilesCommandValidator;
        private readonly IFileCommandsRepository _fileCommandsRepository;
        private readonly ILogger<IAddFilesCommandHandler> _logger;

        public AddFilesCommandHandler(
            IAddFilesCommandValidator addFilesCommandValidator, 
            IFileCommandsRepository fileCommandsRepository, 
            ILogger<IAddFilesCommandHandler> logger)
        {
            _addFilesCommandValidator = Guard.Against.Null(addFilesCommandValidator);
            _fileCommandsRepository = Guard.Against.Null(fileCommandsRepository);
            _logger = Guard.Against.Null(logger);
        }

        public async Task<HttpDataResponse<bool>> HandleAsync(AddFilesCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _addFilesCommandValidator.Validate(request);
            if(validationResult.IsFailed)
            {
                return HttpDataResponses.AsBadRequest<bool>(validationResult.Errors.ToErrorMessages());
            }

            var errorMessages = new List<string>();

            await request.Files.ForEachAsync(async file =>
            {
                try
                {
                    var fileDto = await file.CreateFileDto(cancellationToken);
                    var addResult = await _fileCommandsRepository.AddFileAsync(fileDto, cancellationToken);
                    if(addResult < 0)
                    {
                        var message = string.Format(ErrorMessages.SaveFileFailed, file.FileName);
                        _logger.LogError(LogEvents.AddFileStreamError, message);
                        errorMessages.Add(message);
                    }
                }
                catch(IOException ioException)
                {
                    var message = string.Format(ErrorMessages.ReadFileStreamFailed, file.FileName);
                    _logger.LogError(LogEvents.AddFileStreamError, ioException, message);
                    errorMessages.Add(message);
                }
            });

            return HttpDataResponses.AsOK(true, errorMessages);
        }
    }
}

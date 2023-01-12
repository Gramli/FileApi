using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Extensions;
using File.Domain.Http;
using File.Domain.Logging;
using File.Domain.Queries;
using Microsoft.Extensions.Logging;
using Validot;

namespace File.Core.Queries
{
    internal sealed class DownloadFileQueryHandler : IDownloadFileQueryHandler
    {
        private readonly IValidator<DownloadFileQuery> _downloadFileQueryValidator;
        private readonly ILogger<IDownloadFileQueryHandler> _logger;
        private readonly IFileQueriesRepository _fileQueriesRepository;
        public DownloadFileQueryHandler(
            IValidator<DownloadFileQuery> downloadFileQueryValidator, 
            ILogger<IDownloadFileQueryHandler> logger,
            IFileQueriesRepository fileQueriesRepository) 
        {
            _downloadFileQueryValidator = Guard.Against.Null(downloadFileQueryValidator);
            _logger = Guard.Against.Null(logger);
            _fileQueriesRepository = Guard.Against.Null(fileQueriesRepository);
        }

        public async Task<HttpDataResponse<FileDto>> HandleAsync(DownloadFileQuery request, CancellationToken cancellationToken)
        {
            var validationResult = _downloadFileQueryValidator.Validate(request);
            if(validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.GetFileValidationError, validationResult.ToString());
                HttpDataResponses.AsBadRequest<FileDto>(validationResult.ToString());
            }

            var file = await _fileQueriesRepository.GetFile(request, cancellationToken);
            return HttpDataResponses.AsOK(file);
        }
    }
}

using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Extensions;
using File.Domain.Http;
using File.Domain.Logging;
using File.Domain.Queries;
using Mapster;
using Microsoft.Extensions.Logging;
using Validot;

namespace File.Core.Queries
{
    internal class ExportFileQueryHandler : IExportFileQueryHandler
    {
        private readonly IValidator<ExportFileQuery> _exportFileQueryValidator;
        private readonly IFileQueriesRepository _fileQueriesRepository;
        private readonly ILogger<IExportFileQueryHandler> _logger;
        public async Task<HttpDataResponse<FileDto>> HandleAsync(ExportFileQuery request, CancellationToken cancellationToken)
        {
            var validationResult = _exportFileQueryValidator.Validate(request);
            if (validationResult.AnyErrors)
            {
                _logger.LogError(LogEvents.ExportFileValidationError, validationResult.ToString());
                HttpDataResponses.AsBadRequest<FileDto>(validationResult.ToString());
            }

            var file = await _fileQueriesRepository.GetFile(request.Adapt<DownloadFileQuery>(), cancellationToken);

            throw new NotImplementedException();
        }
    }
}

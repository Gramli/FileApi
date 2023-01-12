using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Core.Extensions;
using File.Core.Resources;
using File.Domain.Dtos;
using File.Domain.Extensions;
using File.Domain.Http;
using File.Domain.Logging;
using File.Domain.Queries;
using Mapster;
using Microsoft.Extensions.Logging;

namespace File.Core.Queries
{
    internal class ExportFileQueryHandler : IExportFileQueryHandler
    {
        private readonly IExportFileQueryValidator _exportFileQueryValidator;
        private readonly IFileQueriesRepository _fileQueriesRepository;
        private readonly ILogger<IExportFileQueryHandler> _logger;
        private readonly IFileConvertService _fileConvertService;
        private readonly IFileByOptionsValidator _fileByOptionsValidator;

        public ExportFileQueryHandler(
            IExportFileQueryValidator exportFileQueryValidator, 
            IFileQueriesRepository fileQueriesRepository, 
            ILogger<IExportFileQueryHandler> logger, 
            IFileConvertService fileConvertService,
            IFileByOptionsValidator fileByOptionsValidator)
        {
            _exportFileQueryValidator = Guard.Against.Null(exportFileQueryValidator);
            _fileQueriesRepository = Guard.Against.Null(fileQueriesRepository);
            _logger = Guard.Against.Null(logger);
            _fileConvertService = Guard.Against.Null(fileConvertService);
            _fileByOptionsValidator = Guard.Against.Null(fileByOptionsValidator);
        }

        public async Task<HttpDataResponse<FileDto>> HandleAsync(ExportFileQuery request, CancellationToken cancellationToken)
        {
            var validationResult = _exportFileQueryValidator.Validate(request);
            if (validationResult.IsFailed)
            {
                _logger.LogError(LogEvents.ExportFileValidationError, validationResult.ToString());
                HttpDataResponses.AsBadRequest<FileDto>(validationResult.ToString());
            }

            var file = await _fileQueriesRepository.GetFile(request.Adapt<DownloadFileQuery>(), cancellationToken);

            var conversionValidationResult = _fileByOptionsValidator.ValidateConversion(file.FileName.GetFileExtension(), request.Extension);
            if (conversionValidationResult.IsFailed)
            {
                _logger.LogError(LogEvents.ExportFileGeneralError, conversionValidationResult.Errors.JoinToMessage());
                return HttpDataResponses.AsBadRequest<FileDto>(string.Format(ErrorMessages.ExportFileFailed, request.Id, request.Extension));
            }

            var exportResult = await _fileConvertService.ExportTo(file, request.Extension, cancellationToken);

            if(exportResult.IsFailed)
            {
                _logger.LogError(LogEvents.ExportFileGeneralError, exportResult.Errors.JoinToMessage());
                return HttpDataResponses.AsBadRequest<FileDto>(string.Format(ErrorMessages.ExportFileFailed, request.Id, request.Extension));
            }

            return HttpDataResponses.AsOK(await exportResult.Value.CreateFileDto(cancellationToken));
        }
    }
}

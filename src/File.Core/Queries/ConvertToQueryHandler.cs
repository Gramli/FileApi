using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Core.Extensions;
using File.Core.Resources;
using File.Domain.Dtos;
using File.Domain.Extensions;
using File.Domain.Http;
using File.Domain.Logging;
using Microsoft.Extensions.Logging;

namespace File.Core.Queries
{
    internal class ConvertToQueryHandler : IConvertToQueryHandler
    {
        private readonly ILogger<IConvertToQueryHandler> _logger;
        private readonly IFileConvertService _fileConvertService;

        public ConvertToQueryHandler(ILogger<IConvertToQueryHandler> logger, IFileConvertService fileConvertService)
        {
            _logger = Guard.Against.Null(logger);
            _fileConvertService = Guard.Against.Null(fileConvertService);
        }

        public async Task<HttpDataResponse<FileDto>> HandleAsync(ConvertToQuery request, CancellationToken cancellationToken)
        {
            //TODO ADD VALIDATION
            var convertResult = await _fileConvertService.ConvertTo(request.File, request.ExtensionToConvert, cancellationToken);

            if(convertResult.IsFailed)
            {
                _logger.LogError(LogEvents.ConvertFileGeneralError, convertResult.Errors.JoinToMessage());
                return HttpDataResponses.AsBadRequest<FileDto>(string.Format(ErrorMessages.ConvertFileFailed, request.File.FileName, request.ExtensionToConvert));
            }

            return HttpDataResponses.AsOK(await convertResult.Value.CreateFileDto(cancellationToken));
        }
    }
}

using Ardalis.GuardClauses;
using File.Core.Abstractions;
using File.Domain.Dtos;
using File.Domain.Extensions;
using File.Domain.Http;

namespace File.Core.Queries
{
    internal class GetFilesInfoQueryHandler : IGetFilesInfoQueryHandler
    {
        private readonly IFileQueriesRepository _fileQueriesRepository;
        public GetFilesInfoQueryHandler(IFileQueriesRepository fileQueriesRepository) 
        { 
            _fileQueriesRepository = Guard.Against.Null(fileQueriesRepository);
        }

        public async Task<HttpDataResponse<IEnumerable<FileInfoDto>>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var filesInfo = await _fileQueriesRepository.GetFilesInfo(cancellationToken);
            return HttpDataResponses.AsOK(filesInfo);
        }
    }
}

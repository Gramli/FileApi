using File.Core.Abstractions;
using File.Core.Queries;
using File.Domain.Queries;
using Microsoft.Extensions.Logging;
using Moq;
using Validot;

namespace File.Core.UnitTests.Queries
{
    public class DownloadFileQueryHandlerTests
    {
        private readonly Mock<IValidator<DownloadFileQuery>> _downloadFileQueryValidatorMock;
        private readonly Mock<ILogger<IDownloadFileQueryHandler>> _loggerMock;
        private readonly Mock<IFileQueriesRepository> _fileQueriesRepositoryMock;

        private readonly IDownloadFileQueryHandler _uut;

        public DownloadFileQueryHandlerTests()
        {
            _downloadFileQueryValidatorMock = new Mock<IValidator<DownloadFileQuery>>();
            _loggerMock = new Mock<ILogger<IDownloadFileQueryHandler>>();
            _fileQueriesRepositoryMock = new Mock<IFileQueriesRepository>();

            _uut = new DownloadFileQueryHandler(_downloadFileQueryValidatorMock.Object, _loggerMock.Object, _fileQueriesRepositoryMock.Object);
        }
    }
}

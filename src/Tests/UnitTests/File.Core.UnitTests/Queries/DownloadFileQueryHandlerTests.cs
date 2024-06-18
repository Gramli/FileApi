using File.Core.Abstractions;
using File.Core.Queries;
using File.Core.Resources;
using File.Domain.Dtos;
using File.Domain.Logging;
using File.Domain.Queries;
using File.UnitTests.Common.Extensions;
using System.Net;
using Validot;
using Validot.Results;

namespace File.Core.UnitTests.Queries
{
    public class DownloadFileQueryHandlerTests
    {
        private readonly Mock<IValidator<DownloadFileQuery>> _downloadFileQueryValidatorMock;
        private readonly Mock<ILogger<IDownloadFileQueryHandler>> _loggerMock;
        private readonly Mock<IFileQueriesRepository> _fileQueriesRepositoryMock;
        private readonly Mock<IValidationResult> _validationResult;

        private readonly IDownloadFileQueryHandler _uut;

        public DownloadFileQueryHandlerTests()
        {
            _downloadFileQueryValidatorMock = new Mock<IValidator<DownloadFileQuery>>();
            _loggerMock = new Mock<ILogger<IDownloadFileQueryHandler>>();
            _fileQueriesRepositoryMock = new Mock<IFileQueriesRepository>();

            _validationResult = new Mock<IValidationResult>();

            _uut = new DownloadFileQueryHandler(_downloadFileQueryValidatorMock.Object, _loggerMock.Object, _fileQueriesRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidationFailed()
        {
            //Arrange
            var query = new DownloadFileQuery(1);

            _downloadFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<DownloadFileQuery>(), It.IsAny<bool>())).Returns(_validationResult.Object);
            _validationResult.SetupGet(x=>x.AnyErrors).Returns(true);
            _validationResult.Setup(x => x.ToString()).Returns(string.Empty);

            //Act
            var result = await _uut.HandleAsync(query, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.Errors);
            Assert.Equal(ValidationErrorMessages.InvalidRequest, result.Errors.First());
            _downloadFileQueryValidatorMock.Verify(x => x.Validate(It.Is<DownloadFileQuery>(y=>y.Equals(query)), It.IsAny<bool>()), Times.Once);
            _validationResult.VerifyGet(x => x.AnyErrors, Times.Once);
            _validationResult.Verify(x => x.ToString(), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.GetFileValidationError, Times.Once());
        }

        [Fact]
        public async Task GetFileFailed()
        {
            //Arrange
            var query = new DownloadFileQuery(1);

            _downloadFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<DownloadFileQuery>(), It.IsAny<bool>())).Returns(_validationResult.Object);
            _validationResult.SetupGet(x => x.AnyErrors).Returns(false);

            _fileQueriesRepositoryMock.Setup(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(string.Empty));

            //Act
            var result = await _uut.HandleAsync(query, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Null(result.Data);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.FileNotExist, result.Errors.First());
            _downloadFileQueryValidatorMock.Verify(x => x.Validate(It.Is<DownloadFileQuery>(y => y.Equals(query)), It.IsAny<bool>()), Times.Once);
            _validationResult.VerifyGet(x => x.AnyErrors, Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.GetFileDatabaseError, Times.Once());
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.Is<DownloadFileQuery>(y => y.Equals(query)), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var query = new DownloadFileQuery(1);
            var resultDto = new FileDto();

            _downloadFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<DownloadFileQuery>(), It.IsAny<bool>())).Returns(_validationResult.Object);
            _validationResult.SetupGet(x => x.AnyErrors).Returns(false);

            _fileQueriesRepositoryMock.Setup(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(resultDto));

            //Act
            var result = await _uut.HandleAsync(query, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(resultDto, result.Data);
            Assert.Empty(result.Errors);
            _downloadFileQueryValidatorMock.Verify(x => x.Validate(It.Is<DownloadFileQuery>(y => y.Equals(query)), It.IsAny<bool>()), Times.Once);
            _validationResult.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.Is<DownloadFileQuery>(y => y.Equals(query)), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

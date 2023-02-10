using File.Core.Abstractions;
using File.Core.Queries;
using File.Core.Resources;
using File.Core.UnitTests.Assets;
using File.Domain.Dtos;
using File.Domain.Logging;
using File.Domain.Queries;
using File.UnitTests.Common.Extensions;
using FluentResults;
using System.Net;

namespace File.Core.UnitTests.Queries
{
    public class ExportFileQueryHandlerTests
    {
        private readonly Mock<IExportFileQueryValidator> _exportFileQueryValidatorMock;
        private readonly Mock<IFileQueriesRepository> _fileQueriesRepositoryMock;
        private readonly Mock<ILogger<IExportFileQueryHandler>> _loggerMock;
        private readonly Mock<IFileConvertService> _fileConvertServiceMock;
        private readonly Mock<IFileByOptionsValidator> _fileByOptionsValidatorMock;

        private readonly IExportFileQueryHandler _uut;

        public ExportFileQueryHandlerTests()
        {
            _exportFileQueryValidatorMock = new Mock<IExportFileQueryValidator>();
            _fileQueriesRepositoryMock = new Mock<IFileQueriesRepository>();
            _loggerMock = new Mock<ILogger<IExportFileQueryHandler>>();
            _fileConvertServiceMock = new Mock<IFileConvertService>();
            _fileByOptionsValidatorMock = new Mock<IFileByOptionsValidator>();

            _uut = new ExportFileQueryHandler(
                _exportFileQueryValidatorMock.Object,
                _fileQueriesRepositoryMock.Object,
                _loggerMock.Object,
                _fileConvertServiceMock.Object,
                _fileByOptionsValidatorMock.Object);
        }

        [Fact]
        public async Task ValidateRequest_Failed()
        {
            //Arrange
            var request = new ExportFileQuery();

            var failedMessage = "failedMessage";
            _exportFileQueryValidatorMock.Setup(x=>x.Validate(It.IsAny<ExportFileQuery>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Contains(failedMessage, result.Errors.First());
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ExportFileValidationError, Times.Once());
            _exportFileQueryValidatorMock.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request))), Times.Once);
        }

        [Fact]
        public async Task GetFile_Failed()
        {
            //Arrange
            var request = new ExportFileQuery();

            var failedMessage = "failedMessage";
            _exportFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ExportFileQuery>())).Returns(Result.Ok(true));
            _fileQueriesRepositoryMock.Setup(x=>x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failedMessage));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ErrorMessages.ExportFileFailed, request.Id, request.Extension), result.Errors.First());
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.GetFileDatabaseError, failedMessage, Times.Once());
            _exportFileQueryValidatorMock.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request))), Times.Once);
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ValidateConversion_Failed()
        {
            //Arrange
            var request = new ExportFileQuery
            {
                Extension = "xml"
            };
            var sourceExtension = "json";
            var fileDto = new FileDto 
            { 
                FileName = $"json.{sourceExtension}" 
            };

            var failedMessage = "failedMessage";
            _exportFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ExportFileQuery>())).Returns(Result.Ok(true));
            _fileQueriesRepositoryMock.Setup(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(fileDto));
            _fileByOptionsValidatorMock.Setup(x=>x.ValidateConversion(It.IsAny<string>(), It.IsAny<string>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ErrorMessages.ExportFileFailed, request.Id, request.Extension), result.Errors.First());
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ExportFileGeneralError, failedMessage, Times.Once());
            _exportFileQueryValidatorMock.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request))), Times.Once);
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.ValidateConversion(It.Is<string>(y=>y.Equals(sourceExtension)), It.Is<string>(y => y.Equals(request.Extension))), Times.Once);
        }

        [Fact]
        public async Task ExportTo_Failed()
        {
            //Arrange
            var request = new ExportFileQuery
            {
                Extension = "xml"
            };
            var sourceExtension = "json";
            var fileDto = new FileDto
            {
                FileName = $"json.{sourceExtension}"
            };

            var failedMessage = "failedMessage";
            _exportFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ExportFileQuery>())).Returns(Result.Ok(true));
            _fileQueriesRepositoryMock.Setup(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(fileDto));
            _fileByOptionsValidatorMock.Setup(x => x.ValidateConversion(It.IsAny<string>(), It.IsAny<string>())).Returns(Result.Ok(true));
            _fileConvertServiceMock.Setup(x=>x.ExportTo(It.IsAny<FileDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failedMessage));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ErrorMessages.ExportFileFailed, request.Id, request.Extension), result.Errors.First());
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ExportFileGeneralError, failedMessage, Times.Once());
            _exportFileQueryValidatorMock.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request))), Times.Once);
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.ValidateConversion(It.Is<string>(y => y.Equals(sourceExtension)), It.Is<string>(y => y.Equals(request.Extension))), Times.Once);
            _fileConvertServiceMock.Verify(x => x.ExportTo(It.Is<FileDto>(y => y.Equals(fileDto)), It.Is<string>(y => y.Equals(request.Extension)), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var request = new ExportFileQuery
            {
                Extension = "xml"
            };
            var sourceExtension = "json";
            var fileDto = new FileDto
            {
                FileName = $"json.{sourceExtension}"
            };

            var resultFileData = new byte[10];
            var resultFileMock = FileMockFactory.CreateMock(resultFileData, "application/json", "resultFileName");

            _exportFileQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ExportFileQuery>())).Returns(Result.Ok(true));
            _fileQueriesRepositoryMock.Setup(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok(fileDto));
            _fileByOptionsValidatorMock.Setup(x => x.ValidateConversion(It.IsAny<string>(), It.IsAny<string>())).Returns(Result.Ok(true));
            _fileConvertServiceMock.Setup(x => x.ExportTo(It.IsAny<FileDto>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(resultFileMock.Object));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            Assert.Equal(resultFileData.Length, result.Data.Length);
            _exportFileQueryValidatorMock.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request))), Times.Once);
            _fileQueriesRepositoryMock.Verify(x => x.GetFile(It.IsAny<DownloadFileQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.ValidateConversion(It.Is<string>(y => y.Equals(sourceExtension)), It.Is<string>(y => y.Equals(request.Extension))), Times.Once);
            _fileConvertServiceMock.Verify(x => x.ExportTo(It.Is<FileDto>(y => y.Equals(fileDto)), It.Is<string>(y => y.Equals(request.Extension)), It.IsAny<CancellationToken>()));
        }
    }
}

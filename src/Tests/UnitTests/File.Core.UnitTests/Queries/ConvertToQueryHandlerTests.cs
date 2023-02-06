using File.Core.Abstractions;
using File.Core.Queries;
using File.Core.Resources;
using File.Domain.Abstractions;
using File.Domain.Logging;
using File.UnitTests.Common.Extensions;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace File.Core.UnitTests.Queries
{
    public class ConvertToQueryHandlerTests
    {
        private readonly Mock<ILogger<IConvertToQueryHandler>> _loggerMock;
        private readonly Mock<IConvertToQueryValidator> _convertToQueryValidatorMock;
        private readonly Mock<IFileConvertService> _fileConvertServiceMock;

        private readonly Mock<IFile> _fileMock;

        private readonly IConvertToQueryHandler _uut;

        public ConvertToQueryHandlerTests()
        {
            _loggerMock = new Mock<ILogger<IConvertToQueryHandler>>();
            _convertToQueryValidatorMock = new Mock<IConvertToQueryValidator>();
            _fileConvertServiceMock = new Mock<IFileConvertService>();

            _fileMock = new Mock<IFile>();

            _uut = new ConvertToQueryHandler(_loggerMock.Object, _fileConvertServiceMock.Object, _convertToQueryValidatorMock.Object);
        }

        [Fact]
        public async Task RequestValidation_Failed()
        {
            //Arrange
            var failedMessage = nameof(RequestValidation_Failed);
            _convertToQueryValidatorMock.Setup(x=>x.Validate(It.IsAny<ConvertToQuery>())).Returns(Result.Fail(failedMessage));

            var request = new ConvertToQuery(_fileMock.Object, string.Empty);

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors.First(), failedMessage);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request))), Times.Once);
        }

        [Fact]
        public async Task FileConvert_Failed()
        {
            //Arrange
            var fileName = nameof(FileConvert_Failed);
            var format = "json";
            var request = new ConvertToQuery(_fileMock.Object, format);
            var failedMessage = nameof(FileConvert_Failed);
            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>())).Returns(Result.Ok(true));
            _fileConvertServiceMock.Setup(x => x.ConvertTo(It.IsAny<IFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failedMessage));
            _fileMock.SetupGet(x => x.FileName).Returns(fileName);

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(result.Errors.First(), string.Format(ErrorMessages.ConvertFileFailed, fileName, format));
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request))), Times.Once);
            _fileConvertServiceMock.Verify(x => x.ConvertTo(It.IsAny<IFile>(), It.Is<string>(y=>y.Equals(format)), It.IsAny<CancellationToken>()), Times.Once);
            _loggerMock.VerifyLog(LogLevel.Error, LogEvents.ConvertFileGeneralError, Times.Once());
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var resultFileData = new byte[10];

            var resultFileMock = new Mock<IFile>();
            resultFileMock.SetupGet(x=>x.ContentType).Returns("application/json");
            resultFileMock.SetupGet(x => x.FileName).Returns("resultFileName");
            resultFileMock.SetupGet(x => x.Length).Returns(resultFileData.Length);
            resultFileMock.Setup(x => x.GetData(It.IsAny<CancellationToken>())).ReturnsAsync(resultFileData);

            var format = "xml";
            var request = new ConvertToQuery(_fileMock.Object, format);
            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>())).Returns(Result.Ok(true));
            _fileConvertServiceMock.Setup(x => x.ConvertTo(It.IsAny<IFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Ok(resultFileMock.Object));
            _fileMock.SetupGet(x => x.FileName).Returns(nameof(FileConvert_Failed));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request))), Times.Once);
            _fileConvertServiceMock.Verify(x => x.ConvertTo(It.IsAny<IFile>(), It.Is<string>(y => y.Equals(format)), It.IsAny<CancellationToken>()), Times.Once);
            resultFileMock.VerifyGet(x => x.ContentType, Times.Once);
            resultFileMock.VerifyGet(x => x.FileName, Times.Once);
            resultFileMock.VerifyGet(x => x.Length, Times.Once);
            resultFileMock.Verify(x => x.GetData(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

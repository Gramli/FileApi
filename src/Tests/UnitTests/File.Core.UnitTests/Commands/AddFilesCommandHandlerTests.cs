using File.Core.Abstractions;
using File.Core.Commands;
using File.Domain.Abstractions;
using File.Domain.Commands;
using File.Domain.Dtos;
using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;

namespace File.Core.UnitTests.Commands
{
    public class AddFilesCommandHandlerTests
    {
        private readonly Mock<IAddFilesCommandValidator> _addFilesCommandValidatorMock;
        private readonly Mock<IFileCommandsRepository> _fileCommandsRepositoryMock;
        private readonly Mock<ILogger<IAddFilesCommandHandler>> _loggerMock;

        private readonly IAddFilesCommandHandler _uut;

        public AddFilesCommandHandlerTests()
        {
            _addFilesCommandValidatorMock = new Mock<IAddFilesCommandValidator>();
            _fileCommandsRepositoryMock = new Mock<IFileCommandsRepository>();
            _loggerMock = new Mock<ILogger<IAddFilesCommandHandler>>();

            _uut = new AddFilesCommandHandler(_addFilesCommandValidatorMock.Object, _fileCommandsRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Validation_Failed()
        {
            //Arrange
            var validationFailedMessage = "validationFailedMessage";
            var request = new AddFilesCommand(Enumerable.Empty<IFile>());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Fail<bool>(validationFailedMessage));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(validationFailedMessage, result.Errors.Single());
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y=>y.Equals(request))), Times.Once);
        }

        [Fact]
        public async Task AddFileAsync_IOException_All_Failed()
        {
            //Arrange
            var iFileMockOne = new Mock<IFile>();
            var iFileMockTwo = new Mock<IFile>();

            var request = new AddFilesCommand(new List<IFile>()
            {
                iFileMockOne.Object, 
                iFileMockTwo.Object
            });

            iFileMockOne.Setup(x => x.GetData(CancellationToken.None)).ThrowsAsync(new IOException());
            iFileMockTwo.Setup(x => x.GetData(CancellationToken.None)).ThrowsAsync(new IOException());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Ok<bool>(true));

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, result.Errors.Count);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(request))), Times.Once);
            iFileMockOne.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            iFileMockTwo.Verify(x => x.GetData(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task AddFileAsync_IOException_One_Failed()
        {
            //Arrange
            var iFileMockOne = new Mock<IFile>();
            var iFileMockTwo = new Mock<IFile>();

            var request = new AddFilesCommand(new List<IFile>()
            {
                iFileMockOne.Object,
                iFileMockTwo.Object
            });

            iFileMockOne.Setup(x => x.GetData(CancellationToken.None)).ThrowsAsync(new IOException());
            iFileMockTwo.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Ok<bool>(true));

            _fileCommandsRepositoryMock.Setup(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None)).ReturnsAsync(1);
            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, result.Errors.Count);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(request))), Times.Once);
            iFileMockOne.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            iFileMockTwo.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            _fileCommandsRepositoryMock.Verify(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task AddFileAsync_All_Failed()
        {
            //Arrange
            var iFileMockOne = new Mock<IFile>();
            var iFileMockTwo = new Mock<IFile>();

            var request = new AddFilesCommand(new List<IFile>()
            {
                iFileMockOne.Object,
                iFileMockTwo.Object
            });

            iFileMockOne.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());
            iFileMockTwo.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Ok<bool>(true));

            _fileCommandsRepositoryMock.Setup(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None)).ReturnsAsync(-1);
            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(2, result.Errors.Count);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(request))), Times.Once);
            iFileMockOne.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            iFileMockTwo.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            _fileCommandsRepositoryMock.Verify(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None), Times.Exactly(2));
        }

        [Fact]
        public async Task AddFileAsync_One_Failed()
        {
            //Arrange
            var fileName = "fileName";

            var iFileMockOne = new Mock<IFile>();
            var iFileMockTwo = new Mock<IFile>();

            var request = new AddFilesCommand(new List<IFile>()
            {
                iFileMockOne.Object,
                iFileMockTwo.Object
            });

            iFileMockOne.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());
            iFileMockOne.SetupGet(x => x.FileName).Returns(fileName);
            iFileMockTwo.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Ok<bool>(true));

            _fileCommandsRepositoryMock.Setup(x => x.AddFileAsync(It.Is<FileDto>(y => string.IsNullOrEmpty(y.FileName)), CancellationToken.None)).ReturnsAsync(-1);
            _fileCommandsRepositoryMock.Setup(x => x.AddFileAsync(It.Is<FileDto>(y => !string.IsNullOrEmpty(y.FileName) && y.FileName.Equals(fileName)), CancellationToken.None)).ReturnsAsync(1);

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(1, result.Errors.Count);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(request))), Times.Once);
            iFileMockOne.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            iFileMockTwo.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            _fileCommandsRepositoryMock.Verify(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None), Times.Exactly(2));
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            var iFileMockOne = new Mock<IFile>();
            var iFileMockTwo = new Mock<IFile>();

            var request = new AddFilesCommand(new List<IFile>()
            {
                iFileMockOne.Object,
                iFileMockTwo.Object
            });

            iFileMockOne.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());
            iFileMockTwo.Setup(x => x.GetData(CancellationToken.None)).ReturnsAsync(Array.Empty<byte>());

            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>())).Returns(Result.Ok<bool>(true));

            _fileCommandsRepositoryMock.Setup(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None)).ReturnsAsync(2);

            //Act
            var result = await _uut.HandleAsync(request, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Empty(result.Errors);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(request))), Times.Once);
            iFileMockOne.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            iFileMockTwo.Verify(x => x.GetData(CancellationToken.None), Times.Once);
            _fileCommandsRepositoryMock.Verify(x => x.AddFileAsync(It.IsAny<FileDto>(), CancellationToken.None), Times.Exactly(2));
        }
    }
}

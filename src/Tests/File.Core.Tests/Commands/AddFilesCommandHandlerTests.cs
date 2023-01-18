using File.Core.Abstractions;
using File.Core.Commands;
using Microsoft.Extensions.Logging;
using Moq;

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
        public void Validation_Failed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void AddFileAsync_All_Failed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void AddFileAsync_One_Failed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void AddFileAsync_IOException_One_Failed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void Success()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}

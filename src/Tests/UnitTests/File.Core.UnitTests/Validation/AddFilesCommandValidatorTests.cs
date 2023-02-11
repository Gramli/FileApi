using File.Core.Abstractions;
using File.Core.UnitTests.Assets;
using File.Core.Validation;
using File.Domain.Abstractions;
using File.Domain.Commands;
using Validot;
using Validot.Results;

namespace File.Core.UnitTests.Validation
{
    public class AddFilesCommandValidatorTests
    {
        private readonly Mock<IValidator<AddFilesCommand>> _addFilesCommandValidatorMock;
        private readonly Mock<IFileByOptionsValidator> _fileByOptionsValidatorMock;
        private readonly Mock<IValidationResult> _validationResultMock;

        private readonly IAddFilesCommandValidator _uut;

        public AddFilesCommandValidatorTests()
        {
            _addFilesCommandValidatorMock = new Mock<IValidator<AddFilesCommand>>();
            _fileByOptionsValidatorMock = new Mock<IFileByOptionsValidator>();
            _validationResultMock = new Mock<IValidationResult>();

            _uut = new AddFilesCommandValidator(_addFilesCommandValidatorMock.Object, _fileByOptionsValidatorMock.Object);
        }

        [Fact]
        public void CommandValidation_Failed()
        {
            //Arrange
            var command = new AddFilesCommand(Enumerable.Empty<IFile>());

            _validationResultMock.SetupGet(x=>x.AnyErrors).Returns(true);
            _addFilesCommandValidatorMock.Setup(x=>x.Validate(It.IsAny<AddFilesCommand>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);

            //Act
            var result = _uut.Validate(command);

            //Assert
            Assert.True(result.IsFailed);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y=>y.Equals(command)), It.IsAny<bool>()), Times.Once);
        }

        [Fact]
        public void AllFilesValidation_Failed()
        {
            //Arrange
            var fileMockOne = FileMockFactory.CreateMock(new byte[10], "application/json", "resultFileOneName");
            var fileMockTwo = FileMockFactory.CreateMock(new byte[10], "application/json", "resultFileTwoName");

            var command = new AddFilesCommand(new List<IFile>()
            {
                fileMockOne.Object, 
                fileMockTwo.Object
            });
            var failedMessage = "failedMessage";

            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.IsAny<IFile>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = _uut.Validate(command);

            //Assert
            Assert.True(result.IsFailed);
            Assert.True(result.IsFailed);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(command)), It.IsAny<bool>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFile>()), Times.Once);
        }

        [Fact]
        public void OneFilesValidation_Failed()
        {
            //Arrange
            var fileNameOne = "fileNameOne";
            var fileNameTwo = "fileNameTwo";

            var fileMockOne = FileMockFactory.CreateMock(new byte[10], "application/json", fileNameOne);
            var fileMockTwo = FileMockFactory.CreateMock(new byte[10], "application/json", fileNameTwo);

            var command = new AddFilesCommand(new List<IFile>()
            {
                fileMockOne.Object,
                fileMockTwo.Object
            });
            var failedMessage = "failedMessage";

            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.Is<IFile>(y=>y.FileName.Equals(fileNameTwo)))).Returns(Result.Fail(failedMessage));
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.Is<IFile>(y =>y.FileName.Equals(fileNameOne)))).Returns(Result.Ok(true));

            //Act
            var result = _uut.Validate(command);

            //Assert
            Assert.True(result.IsFailed);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(command)), It.IsAny<bool>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFile>()), Times.Exactly(2));
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var fileMockOne = FileMockFactory.CreateMock(new byte[10], "application/json", "fileNameOne");
            var fileMockTwo = FileMockFactory.CreateMock(new byte[10], "application/json", "fileNameTwo");

            var command = new AddFilesCommand(new List<IFile>()
            {
                fileMockOne.Object,
                fileMockTwo.Object
            });

            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _addFilesCommandValidatorMock.Setup(x => x.Validate(It.IsAny<AddFilesCommand>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.IsAny<IFile>())).Returns(Result.Ok(true));

            //Act
            var result = _uut.Validate(command);

            //Assert
            Assert.True(result.IsSuccess);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _addFilesCommandValidatorMock.Verify(x => x.Validate(It.Is<AddFilesCommand>(y => y.Equals(command)), It.IsAny<bool>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFile>()), Times.Exactly(2));
        }
    }
}

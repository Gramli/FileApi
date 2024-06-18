using File.Core.Abstractions;
using File.Core.Queries;
using File.Core.UnitTests.Assets;
using File.Core.Validation;
using File.Domain.Abstractions;
using Validot;
using Validot.Results;

namespace File.Core.UnitTests.Validation
{
    public class ConvertToQueryValidatorTests
    {
        private readonly Mock<IValidator<ConvertToQuery>> _convertToQueryValidatorMock;
        private readonly Mock<IFileByOptionsValidator> _fileByOptionsValidatorMock;

        private readonly Mock<IValidationResult> _validationResultMock;

        private readonly IConvertToQueryValidator _uut;

        public ConvertToQueryValidatorTests()
        {
            _convertToQueryValidatorMock = new Mock<IValidator<ConvertToQuery>>();
            _fileByOptionsValidatorMock = new Mock<IFileByOptionsValidator>();

            _validationResultMock = new Mock<IValidationResult>();

            _uut = new ConvertToQueryValidator(_convertToQueryValidatorMock.Object, _fileByOptionsValidatorMock.Object);
        }

        [Fact]
        public void QueryValidation_Failed()
        {
            //Arrange
            var request = new ConvertToQuery(new Mock<IFileProxy>().Object, string.Empty);

            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x=>x.AnyErrors).Returns(true);

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsFailed);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y=>y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
        }

        [Fact]
        public void FileValidation_Failed()
        {
            //Arrange
            var request = new ConvertToQuery(new Mock<IFileProxy>().Object, string.Empty);

            var failedMessage = "failedMessage";

            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.IsAny<IFileProxy>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(failedMessage, result.Errors.First().Message);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFileProxy>()), Times.Once);
        }

        [Fact]
        public void ConversionValidation_Failed()
        {
            //Arrange
            var sourceExtension = "json";
            var destinationExtension = "xml";
            var fileMockOne = FileMockFactory.CreateMock(new byte[10], "application/json", $"resultFileOneName.{sourceExtension}");
            var request = new ConvertToQuery(fileMockOne.Object, destinationExtension);

            var failedMessage = "failedMessage";

            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.IsAny<IFileProxy>())).Returns(Result.Ok(true));
            _fileByOptionsValidatorMock.Setup(x => x.ValidateConversion(It.IsAny<string>(), It.IsAny<string>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(failedMessage, result.Errors.First().Message);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFileProxy>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.ValidateConversion(It.Is<string>(y => y.Equals(sourceExtension)), It.Is<string>(y => y.Equals(destinationExtension))), Times.Once);
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var sourceExtension = "json";
            var destinationExtension = "xml";
            var fileMockOne = FileMockFactory.CreateMock(new byte[10], "application/json", $"resultFileOneName.{sourceExtension}");
            var request = new ConvertToQuery(fileMockOne.Object, destinationExtension);

            _convertToQueryValidatorMock.Setup(x => x.Validate(It.IsAny<ConvertToQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _fileByOptionsValidatorMock.Setup(x => x.Validate(It.IsAny<IFileProxy>())).Returns(Result.Ok(true));
            _fileByOptionsValidatorMock.Setup(x => x.ValidateConversion(It.IsAny<string>(), It.IsAny<string>())).Returns(Result.Ok(true));

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            _convertToQueryValidatorMock.Verify(x => x.Validate(It.Is<ConvertToQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.Validate(It.IsAny<IFileProxy>()), Times.Once);
            _fileByOptionsValidatorMock.Verify(x => x.ValidateConversion(It.Is<string>(y=>y.Equals(sourceExtension)), It.Is<string>(y=>y.Equals(destinationExtension))), Times.Once);
        }
    }
}

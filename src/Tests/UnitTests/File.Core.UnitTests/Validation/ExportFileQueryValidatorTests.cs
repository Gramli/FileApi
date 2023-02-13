using File.Core.Abstractions;
using File.Core.Validation;
using File.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validot.Results;
using Validot;

namespace File.Core.UnitTests.Validation
{
    public class ExportFileQueryValidatorTests
    {
        private readonly Mock<IValidator<ExportFileQuery>> _exportFileQueryValidator;
        private readonly Mock<IFileByOptionsValidator> _fileByOptionsValidator;

        private readonly Mock<IValidationResult> _validationResultMock;

        private readonly IExportFileQueryValidator _uut;

        public ExportFileQueryValidatorTests()
        {
            _exportFileQueryValidator = new Mock<IValidator<ExportFileQuery>>();
            _fileByOptionsValidator = new Mock<IFileByOptionsValidator>();

            _validationResultMock = new Mock<IValidationResult>();

            _uut = new ExportFileQueryValidator(_exportFileQueryValidator.Object, _fileByOptionsValidator.Object);
        }

        [Fact]
        public void QueryValidation_Failed()
        {
            //Arrange
            var request = new ExportFileQuery();

            _exportFileQueryValidator.Setup(x => x.Validate(It.IsAny<ExportFileQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(true);

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsFailed);
            _exportFileQueryValidator.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
        }

        [Fact]
        public void ExtensionValidation_Failed()
        {
            //Arrange
            var request = new ExportFileQuery()
            {
                Extension = "ex"
            };

            var failedMessage = "failedMessage";

            _exportFileQueryValidator.Setup(x => x.Validate(It.IsAny<ExportFileQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _fileByOptionsValidator.Setup(x => x.Validate(It.IsAny<string>())).Returns(Result.Fail(failedMessage));

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(failedMessage, result.Errors.First().Message);
            _exportFileQueryValidator.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileByOptionsValidator.Verify(x => x.Validate(It.Is<string>(y => y.Equals(request.Extension))), Times.Once);
        }

        [Fact]
        public void Success()
        {
            //Arrange
            var request = new ExportFileQuery()
            {
                Extension = "ex"
            };

            _exportFileQueryValidator.Setup(x => x.Validate(It.IsAny<ExportFileQuery>(), It.IsAny<bool>())).Returns(_validationResultMock.Object);
            _validationResultMock.SetupGet(x => x.AnyErrors).Returns(false);
            _fileByOptionsValidator.Setup(x => x.Validate(It.IsAny<string>())).Returns(Result.Ok(true));

            //Act
            var result = _uut.Validate(request);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Errors);
            _exportFileQueryValidator.Verify(x => x.Validate(It.Is<ExportFileQuery>(y => y.Equals(request)), It.IsAny<bool>()), Times.Once);
            _validationResultMock.VerifyGet(x => x.AnyErrors, Times.Once);
            _fileByOptionsValidator.Verify(x => x.Validate(It.Is<string>(y => y.Equals(request.Extension))), Times.Once);
        }
    }
}

using File.Core.Abstractions;
using File.Core.Resources;
using File.Core.UnitTests.Assets;
using File.Core.Validation;
using File.Domain.Options;
using Microsoft.Extensions.Options;

namespace File.Core.UnitTests.Validation
{
    public class FileByOptionsValidatorTests
    {
        private readonly IFileByOptionsValidator _uut;

        public FileByOptionsValidatorTests()
        {
            var fileOptions = Options.Create(new FilesOptions
            {
                MaxFileLength = 10,
                AllowedFiles = new []
                {
                    new AllowedFile 
                    { 
                        CanBeExportedTo = new []{"xml", "csv"},
                        ContentType = "application/json",
                        Format = "json",
                    }
                }
            });

            _uut = new FileByOptionsValidator(fileOptions);
        }

        [Fact]
        public void ValidateFile_NotAllowedContentType()
        {
            //Arrange
            var fileName = "fileName";
            var contentType = "contentType";
            var fileMock = FileMockFactory.CreateMock(new byte[0], contentType, fileName);

            //Act
            var result = _uut.Validate(fileMock.Object);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.UnsupportedFormat, fileName, contentType), result.Errors.First().Message);
        }

        [Fact]
        public void ValidateFile_MaxLengthExceed()
        {
            //Arrange
            var fileName = "fileName";
            var contentType = "application/json";
            var fileMock = FileMockFactory.CreateMock(new byte[12], contentType, fileName);

            //Act
            var result = _uut.Validate(fileMock.Object);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.MaximalFileSize, fileName), result.Errors.First().Message);
        }

        [Fact]
        public void ValidateFile_EmptyFile()
        {
            //Arrange
            var fileName = "fileName";
            var contentType = "application/json";
            var fileMock = FileMockFactory.CreateMock(new byte[0], contentType, fileName);

            //Act
            var result = _uut.Validate(fileMock.Object);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.FileIsEmpty, fileName), result.Errors.First().Message);
        }

        [Fact]
        public void ValidateExtension_NotAllowedExtension()
        {
            //Arrange
            var extension = "ss";

            //Act
            var result = _uut.Validate(extension);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.UnsupportedExtension, extension), result.Errors.First().Message);
        }

        [Fact]
        public void ValidateConversion_NotAllowedSourceExtension()
        {
            //Arrange
            var sourceExtension = "sourceExtension";

            //Act
            var result = _uut.ValidateConversion(sourceExtension, string.Empty);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.UnsupportedExtension, sourceExtension), result.Errors.First().Message);
        }

        [Fact]
        public void ValidateConversion_NotAllowedConversion()
        {
            //Arrange
            var sourceExtension = "json";
            var destExtension = "yaml";

            //Act
            var result = _uut.ValidateConversion(sourceExtension, destExtension);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Single(result.Errors);
            Assert.Equal(string.Format(ValidationErrorMessages.UnsuportedConversion, sourceExtension, destExtension), result.Errors.First().Message);
        }
    }
}

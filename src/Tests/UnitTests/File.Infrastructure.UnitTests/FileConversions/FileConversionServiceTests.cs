using File.Core.Abstractions;
using File.Domain.Abstractions;
using File.Domain.Dtos;
using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions;
using FluentResults;
using Moq;
using System.Text;

namespace File.Infrastructure.UnitTests.FileConversions
{
    public class FileConversionServiceTests
    {
        private readonly Mock<IFileConverterFactory> _fileConverterFactoryMock;
        private readonly Mock<IEncodingFactory> _encodingFactoryMock;

        private readonly IFileConvertService _uut;
        public FileConversionServiceTests()
        {
            _fileConverterFactoryMock = new Mock<IFileConverterFactory>();
            _encodingFactoryMock = new Mock<IEncodingFactory>();

            _uut = new FileConversionService(_fileConverterFactoryMock.Object, _encodingFactoryMock.Object);
        }

        [Fact]
        public async Task ConvertTo_Failed()
        {
            //Arrange
            var extension = "tst";
            var file = new Mock<IFile>();
            file.Setup(x => x.GetData(It.IsAny<CancellationToken>())).ReturnsAsync(Array.Empty<byte>());
            file.SetupGet(x => x.FileName).Returns($"sone.{extension}");

            _encodingFactoryMock.Setup(x => x.CreateEncoding(It.IsAny<byte[]>())).Returns(Encoding.Default);

            var converter = new Mock<IFileConverter>();
            var conversionFailedMessage = "failed message";
            converter.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(conversionFailedMessage));
            _fileConverterFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(converter.Object);
            
            //Act
            var result = await _uut.ConvertTo(file.Object, string.Empty, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Equal(conversionFailedMessage, result.Errors.First().Message);
            _encodingFactoryMock.Verify(x => x.CreateEncoding(It.Is<byte[]>(y=>y.Length == 0)), Times.Once);
            _fileConverterFactoryMock.Verify(x => x.Create(It.Is<string>(y=>y.Equals(extension)), It.Is<string>(y=>string.IsNullOrEmpty(y))), Times.Once);
            converter.Verify(x => x.Convert(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task ExportTo_Failed()
        {
            //Arrange
            var extension = "ss";
            var file = new FileDto
            {
                FileName = $"sss.{extension}"
            };

            _encodingFactoryMock.Setup(x => x.CreateEncoding(It.IsAny<byte[]>())).Returns(Encoding.Default);

            var converter = new Mock<IFileConverter>();
            var conversionFailedMessage = "failed message";
            converter.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(conversionFailedMessage));
            _fileConverterFactoryMock.Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>())).Returns(converter.Object);

            //Act
            var result = await _uut.ExportTo(file, string.Empty, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailed);
            Assert.Equal(conversionFailedMessage, result.Errors.First().Message);
            _encodingFactoryMock.Verify(x => x.CreateEncoding(It.Is<byte[]>(y => y.Length == 0)), Times.Once);
            _fileConverterFactoryMock.Verify(x => x.Create(It.Is<string>(y => y.Equals(extension)), It.Is<string>(y => string.IsNullOrEmpty(y))), Times.Once);
            converter.Verify(x => x.Convert(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

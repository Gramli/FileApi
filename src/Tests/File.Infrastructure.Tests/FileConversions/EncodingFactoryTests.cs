using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions;
using File.Infrastructure.UnitTests.Assets;
using System.Text;

namespace File.Infrastructure.UnitTests.FileConversions
{
    public class EncodingFactoryTests
    {
        private readonly IEncodingFactory _uut;

        public EncodingFactoryTests()
        {
            _uut = new EncodingFactory();
        }

        [Theory]
        [ClassData(typeof(InvalidByteData))]
        public void Invalid_Data(byte[] data)
        {
            //Arrange
            //Act&Assert
            Assert.Throws<ArgumentException>(()=> _uut.CreateEncoding(data));
        }

        [Theory]
        [ClassData(typeof(UknownByteData))]
        public void DefaultEncoding(byte[] data)
        {
            //Arrange
            //Act
            var encoding = _uut.CreateEncoding(data);
            //Assert
            Assert.Equal(Encoding.ASCII, encoding);
        }
    }
}

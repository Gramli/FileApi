using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions;
using File.Infrastructure.UnitTests.Assets;

namespace File.Infrastructure.UnitTests.FileConversions
{
    public class FileConverterFactoryTests
    {
        private readonly IFileConverterFactory _uut;

        public FileConverterFactoryTests()
        {
            _uut = new FileConverterFactory();
        }


        [Theory]
        [ClassData(typeof(ValidConverters))]
        public void Create_Converter(string from, string to, Type desiredConverter)
        {
            //Arrange

            //Act
            var result = _uut.Create(from, to);

            //Assert
            Assert.NotNull(result);
            Assert.IsType(desiredConverter, result);
        }

        [Theory]
        [ClassData(typeof(InvalidConverters))]
        public void Create_Converter_Failed(string from, string to)
        {
            //Act&Assert
            Assert.Throws<NotSupportedException>(() =>  _uut.Create(from, to));
        }
    }
}

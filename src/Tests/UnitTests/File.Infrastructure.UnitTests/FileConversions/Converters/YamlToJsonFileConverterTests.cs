using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using File.Infrastructure.UnitTests.Assets;
using YamlDotNet.Core;

namespace File.Infrastructure.UnitTests.FileConversions.Converters
{
    public class YamlToJsonFileConverterTests
    {
        private readonly IFileConverter _uut;

        public YamlToJsonFileConverterTests()
        {
            _uut = new YamlToJsonFileConverter();
        }

        [Fact]
        public async Task Empty_Yaml()
        {
            //Arrange
            //Act
            var result = await _uut.Convert("", CancellationToken.None);
            //Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value);
        }

        [Theory]
        [ClassData(typeof(InvalidYamlData))]
        public async Task Invalid_Yaml(string invalidYaml)
        {
            //Act & Assert
            await Assert.ThrowsAsync<SyntaxErrorException>(() => _uut.Convert(invalidYaml, CancellationToken.None));
        }
    }
}

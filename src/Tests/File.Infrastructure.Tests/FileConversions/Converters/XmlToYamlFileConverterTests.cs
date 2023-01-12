using File.Infrastructure.Abstractions;
using File.Infrastructure.FileConversions.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File.Infrastructure.UnitTests.FileConversions.Converters
{
    public class XmlToYamlFileConverterTests
    {
        private readonly IFileConverter _uut;

        public XmlToYamlFileConverterTests()
        {
            _uut = new XmlToYamlFileConverter();
        }

        [Fact]
        public async Task Convert_Success()
        {
            //Arrange
            using var fileStream = new FileStream("Assets/new.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new StreamReader(fileStream);
            //Act
            var result = await _uut.Convert(reader.ReadToEnd(), CancellationToken.None);
            //Assert
            Assert.NotEmpty(result);
        }
    }
}

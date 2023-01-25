using File.Core.Abstractions;
using File.Core.Queries;
using Microsoft.Extensions.Logging;
using Moq;

namespace File.Core.UnitTests.Queries
{
    public class ConvertToQueryHandlerTests
    {
        private readonly Mock<ILogger<IConvertToQueryHandler>> _loggerMock;
        private readonly Mock<IConvertToQueryValidator> _convertToQueryValidatorMock;
        private readonly Mock<IFileConvertService> _fileConvertServiceMock;

        private readonly IConvertToQueryHandler _uut;

        public ConvertToQueryHandlerTests()
        {
            _loggerMock = new Mock<ILogger<IConvertToQueryHandler>>();
            _convertToQueryValidatorMock = new Mock<IConvertToQueryValidator>();
            _fileConvertServiceMock = new Mock<IFileConvertService>();

            _uut = new ConvertToQueryHandler(_loggerMock.Object, _fileConvertServiceMock.Object, _convertToQueryValidatorMock.Object);
        }
    }
}

using Microsoft.Extensions.Logging;
using Moq;

namespace File.UnitTests.Common.Extensions
{
    public static class MoqLoggerExtensions
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel logLevel, EventId eventId, string message, Times times)
        {
            loggerMock.Verify(
               x => x.Log(
                   It.Is<LogLevel>(y => y.Equals(logLevel)),
                   It.Is<EventId>(y => y.Equals(eventId)),
                   It.Is<It.IsAnyType>((o, _) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
               times);
        }

        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel logLevel, EventId eventId, Times times)
        {
            loggerMock.Verify(
               x => x.Log(
                   It.Is<LogLevel>(y => y.Equals(logLevel)),
                   It.Is<EventId>(y => y.Equals(eventId)),
                   It.IsAny<It.IsAnyType>(),
                   It.IsAny<Exception>(),
                   It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
               times);
        }
    }
}

using File.Domain.Abstractions;

namespace File.Core.UnitTests.Assets
{
    internal static class FileMockFactory
    {
        public static Mock<IFileProxy> CreateMock(byte[] resultFileData, string contentType, string fileName)
        {
            var resultFileMock = new Mock<IFileProxy>();
            resultFileMock.SetupGet(x => x.ContentType).Returns(contentType);
            resultFileMock.SetupGet(x => x.FileName).Returns(fileName);
            resultFileMock.SetupGet(x => x.Length).Returns(resultFileData.Length);
            resultFileMock.Setup(x => x.GetData(It.IsAny<CancellationToken>())).ReturnsAsync(resultFileData);
            return resultFileMock;
        }
    }
}

namespace File.Infrastructure.UnitTests.Assets
{
    internal static class InvalidJsonData
    {
        public static TheoryData<string> Values => new()
        {
            { "root: {}" },
            { @"\{root: {}}" },
        };
    }
}

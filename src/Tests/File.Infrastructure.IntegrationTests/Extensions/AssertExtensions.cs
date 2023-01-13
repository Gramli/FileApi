using System.Globalization;

namespace File.Infrastructure.IntegrationTests.Extensions
{
    internal static class AssertExtensions
    {
        public static void EqualFileContent(string expected, string actual)
        {
            Assert.Equal(0, string.Compare(actual,
                System.IO.File.ReadAllText(expected),
                CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols));
        }
    }
}

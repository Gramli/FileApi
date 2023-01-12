using System.Globalization;

namespace File.Infrastructure.UnitTests.Extensions
{
    internal static class AssertExtensions
    {
        public static void EqualFileContent(string expected, string actual)
        {
            Assert.Equal(0, string.Compare(expected,
                System.IO.File.ReadAllText(actual),
                CultureInfo.CurrentCulture, CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols));
        }
    }
}

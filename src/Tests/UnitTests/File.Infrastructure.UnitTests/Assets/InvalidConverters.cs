using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class InvalidConverters : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "json", "csv" };
            yield return new object[] { "json", "txt" };
            yield return new object[] { "xml", "csv" };
            yield return new object[] { "xml", "txt" };
            yield return new object[] { "yaml", "txt" };
            yield return new object[] { "yaml", "csv" };
            yield return new object[] { "yaml", "xml" };
            yield return new object[] { "csv", "txt" };
            yield return new object[] { "csv", "yaml" };
            yield return new object[] { "csv", "json" };
            yield return new object[] { "csv", "xml" };
            yield return new object[] { "txt", "csv" };
            yield return new object[] { "txt", "yaml" };
            yield return new object[] { "txt", "json" };
            yield return new object[] { "txt", "xml" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

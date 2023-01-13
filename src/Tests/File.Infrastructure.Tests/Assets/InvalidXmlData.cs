using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class InvalidXmlData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "</>" };
            yield return new object[] { "<root/><root1/>" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

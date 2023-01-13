using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class InvalidJsonData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "root: {}" };
            yield return new object[] { @"\{root: {}}" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

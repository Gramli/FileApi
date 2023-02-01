using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class UknownByteData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new byte[] { 0xff, 0xbb, 0,0 } };
            yield return new object[] { new byte[] { 0xef, 0xbb, 0, 0 } };
            yield return new object[] { new byte[] { 0, 0, 0, 0 } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

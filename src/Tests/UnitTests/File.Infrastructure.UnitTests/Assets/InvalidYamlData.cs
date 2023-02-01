using System.Collections;

namespace File.Infrastructure.UnitTests.Assets
{
    internal class InvalidYamlData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "addresses: [172.16.60.10/8]\r\ngateway4:172.16.11.1}" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

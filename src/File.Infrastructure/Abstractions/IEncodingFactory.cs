using System.Text;

namespace File.Infrastructure.Abstractions
{
    internal interface IEncodingFactory
    {
        Encoding CreateEncoding(byte[] data);
    }
}

using File.Infrastructure.Abstractions;
using System.Text;

namespace File.Infrastructure.FileConversions
{
    internal sealed class EncodingFactory : IEncodingFactory
    {
        private static readonly Dictionary<byte[], Encoding> _encodingMap = new Dictionary<byte[], Encoding>
        {
            {new byte[]{ 0xef, 0xbb, 0xbf }, Encoding.UTF8 },
            {new byte[]{ 0xff, 0xfe, 0, 0 }, Encoding.UTF32 },
            {new byte[]{ 0xff, 0xfe }, Encoding.Unicode },
            {new byte[]{ 0xfe, 0xff }, Encoding.BigEndianUnicode },
            {new byte[]{ 0, 0, 0xfe, 0xff}, new UTF32Encoding(true, true) }
        };

        public Encoding CreateEncoding(byte[] data)
        {
            var firstBytes = new ReadOnlySpan<byte>(data, 0, 4);
            foreach(var encoder in _encodingMap)
            {
                if(firstBytes.SequenceEqual(encoder.Key))
                {
                    return encoder.Value;
                }
            }

            return Encoding.ASCII;
        }
    }
}

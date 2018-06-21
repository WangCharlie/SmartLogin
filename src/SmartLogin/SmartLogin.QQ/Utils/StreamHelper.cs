using System.IO;

namespace SmartLogin.QQ.Utils
{
    internal static class StreamHelper
    {
        public static byte[] ToBytes(this Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}

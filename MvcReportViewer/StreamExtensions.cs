using System.IO;

namespace MvcReportViewer
{
    internal static class StreamExtensions
    {
        public static Stream ToMemoryStream(this byte[] data)
        {
            return new MemoryStream(data);
        }

        public static byte[] ToByteArray(this Stream source)
        {
            var buffer = new byte[16 * 1024];
            using (var memoryStream = new MemoryStream())
            {
                int read;
                while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    memoryStream.Write(buffer, 0, read);
                }

                return memoryStream.ToArray();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MvcReportViewer
{
    public static class StreamExtensions
    {
        private static Stream ToStream(this byte[] data)
        {
            return new MemoryStream(data);
        }

        private static byte[] ToByteArray(this Stream source)
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

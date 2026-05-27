using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class FileHelper
    {
        public static string ConvertStreamToBase64(this Stream input)
        {
            Byte[] bytes = input.CovertStreamToByte();
            return Convert.ToBase64String(bytes);
        }

        public static Byte[] CovertStreamToByte(this Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}

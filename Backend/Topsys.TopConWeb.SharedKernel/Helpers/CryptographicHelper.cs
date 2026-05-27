using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Topsys.TopConWeb.SharedKernel.Helpers
{
    public static class CryptographicHelper
    {

        //private static byte[] HashHMAC(byte[] key, byte[] message)
        //{
        //    var hash = new HMACSHA256(key);
        //    return hash.ComputeHash(message);
        //}

        public static string HmacSha256Digest(this string message, string secret)
        {
            var encoding = new UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA256 cryptographer = new HMACSHA256(keyBytes);

            byte[] bytes = cryptographer.ComputeHash(messageBytes);

            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}

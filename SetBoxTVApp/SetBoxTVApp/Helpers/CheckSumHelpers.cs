using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SetBoxTVApp.Helpers
{
    public static class CheckSumHelpers
    {
        /// <summary>
        /// MD5 CheckSum File
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5HashFile(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "42102f065ee333ba93e57fb329ff060c";

            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(input))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }

        public static string MD5HashString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "42102f065ee333ba93e57fb329ff060c";

            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }

        /// <summary>
        /// CheckMD5Hash
        /// </summary>
        /// <param name="MD5input1">MD5 String</param>
        /// <param name="MD5input2">MD5 String</param>
        /// <returns></returns>
        public static bool CheckMD5Hash(string MD5input1, string MD5input2)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(MD5input1, MD5input2) == 0;
        }
    }
}

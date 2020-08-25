using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SetBoxWebUI.Helpers
{
    /// <summary>
    /// Cripto
    /// </summary>
    public static class CriptoHelpers
    {
        //Method using to Encode, you can use internal, public, private...
        /// <summary>
        /// Method using to Encode
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Method using to Decode, you can use internal, public, private...
        /// <summary>
        /// Method using to Decode
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// MD5 CheckSum File
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5HashFile(string input)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(input))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
                }
            }
        }

        /// <summary>
        /// MD5HashStream
        /// </summary>
        /// <param name="input">Stream</param>
        /// <returns></returns>
        public static string MD5HashStream(Stream input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(input);
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }

        /// <summary>
        /// MD5HashBytes
        /// </summary>
        /// <param name="input">byte[]</param>
        /// <returns></returns>
        public static string MD5HashBytes(byte[] input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(input);
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }

        /// <summary>
        /// MD5 Hash String
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string MD5HashString(string input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }

        /// <summary>
        /// CheckMD5Hash
        /// </summary>
        /// <param name="input1">MD5 String</param>
        /// <param name="input2">MD5 String</param>
        /// <returns></returns>
        public static bool CheckMD5Hash(string input1, string input2)
        {
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(input1, input2) == 0;
        }
    }
}

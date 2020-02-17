using System;
using System.Security.Cryptography;
using System.Text;

namespace SetBoxTV.VideoPlayer.Helpers
{
    public static class CriptoHelpers
    {
        //Method using to Encode, you can use internal, public, private...
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Method using to Decode, you can use internal, public, private...
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string MD5HashString(string input)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }
    }
}

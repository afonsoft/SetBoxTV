using System;
using System.Security.Cryptography;
using System.Text;

namespace SetBoxTVApp.Helpers
{
    public static class CriptoHelpers
    {
        //Method using to Encode, you can use internal, public, private...
        public static string Base64Encode(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return "QUZPTlNPRlQ=";

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //Method using to Decode, you can use internal, public, private...
        public static string Base64Decode(string base64EncodedData)
        {
            if (string.IsNullOrEmpty(base64EncodedData))
                return "";

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
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
    }
}

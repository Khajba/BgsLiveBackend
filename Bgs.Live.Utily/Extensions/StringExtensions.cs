using System;
using System.Security.Cryptography;
using System.Text;

namespace Bgs.Utility.Extensions
{
    public static class StringExtensions
    {
        public static string ToSHA256(this string value, string salt = null)
        {
            string saltString = string.Empty;
            if (!string.IsNullOrEmpty(salt))
                saltString = CreateSalt(salt);

            var saltAndPwd = string.Concat(value, saltString);
            var encoder = new UTF8Encoding();
            var sha256hasher = new SHA256Managed();
            var hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
            var hashedPwd = string.Concat(ByteArrayToString(hashedDataBytes), saltString);
            return hashedPwd;
        }

        private static string ByteArrayToString(byte[] inputArray)
        {
            var output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }

        private static string CreateSalt(string salt)
        {
            byte[] userBytes;
            string saltString;
            userBytes = Encoding.ASCII.GetBytes(salt);
            var XORED = 0x00;

            foreach (int x in userBytes)
                XORED = XORED ^ x;

            var rand = new Random(Convert.ToInt32(XORED));
            saltString = rand.Next().ToString();
            saltString += rand.Next().ToString();
            saltString += rand.Next().ToString();
            saltString += rand.Next().ToString();
            return saltString;
        }
    }
}

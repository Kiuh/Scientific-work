using CryptoNet;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class StringExtensions
    {
        public static string GetHash(this string input)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] hashedBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                _ = sBuilder.Append(hashedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static string GetEncrypted(this string value, ICryptoNet rsa)
        {
            byte[] encrypted = rsa.EncryptFromString(value);
            return Convert.ToBase64String(encrypted);
        }
    }
}

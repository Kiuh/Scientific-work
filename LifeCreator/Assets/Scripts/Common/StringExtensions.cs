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
            SHA256Managed sha256 = new();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Encoding.UTF8.GetString(hashedBytes);
        }

        public static string GetEncrypted(this string value, ICryptoNet rsa)
        {
            byte[] encrypted = rsa.EncryptFromString(value);
            return Convert.ToBase64String(encrypted);
        }
    }
}

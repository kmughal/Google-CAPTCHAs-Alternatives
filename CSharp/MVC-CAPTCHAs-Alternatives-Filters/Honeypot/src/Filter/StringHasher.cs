namespace Honeypot.Filter
{

    using System;
    using System.Security.Cryptography;
    using System.IO;
    using System.Text;

    public static class StringHasher
    {
        const string SAND_KEY = "b14ca5898a4e4133bbce2ea2315a1916";

        public static string EncryptString(string plainInput)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(SAND_KEY);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new MemoryStream();
            using CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
            {
                streamWriter.Write(plainInput);
            }

            array = memoryStream.ToArray();
            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(SAND_KEY);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var memoryStream = new MemoryStream(buffer);

            using var cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);

            using var streamReader = new StreamReader((Stream)cryptoStream);
            return streamReader.ReadToEnd();
        }
    }
}
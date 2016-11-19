using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    class Program
    {
        private static string explicitTextFileName = "Plik.txt";
        private static string keywordTextFileName = "Keyword.txt";
        static byte[] bytes;

        static void Main(string[] args)
        {
            try
            {
                string explicitText = GetTextFromFile(explicitTextFileName);
                bytes = ASCIIEncoding.ASCII.GetBytes(GetTextFromFile(keywordTextFileName));
                string encodedText = Encrypt(explicitText);
                Console.WriteLine(encodedText);
                Console.WriteLine(Decrypt(encodedText));
            }
            catch (Exception ex)
            {
                Console.WriteLine("DES needs 64bit keyword");
            }
            finally
            {
                Console.ReadLine();
            }
        }
        
        private static string Encrypt(string originalString)
        {
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }

        private static string Decrypt(string cryptedString)
        {
            if (String.IsNullOrEmpty(cryptedString))
            {
                throw new ArgumentNullException(@"The string cannot be null.");
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);

            return reader.ReadToEnd();
        }

        private static string GetTextFromFile(string filename)
        {
            if (File.Exists(filename))
            {
                return File.ReadAllText(filename).ToLower();
            }

            return String.Empty;
        }

    }
}

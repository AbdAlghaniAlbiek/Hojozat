using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HojozatServer.Services.Security
{
    public class AESCryptography
    {

        private static string AesIV256 = @"jpawro01280*&da-";  //16 chars = 128 bits

        private static string AesKey256 = @"347npawhpc*^@pad!#iad4358*/%5*9+";  //32 chars = 256 bits


        public static string Encrypt(string decrypted)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert string to byte array
            byte[] src = Encoding.Unicode.GetBytes(decrypted);

            // encryption
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(src, 0, src.Length);

                // Convert byte array to Base64 strings
                return Convert.ToBase64String(dest);
            }
        }

        public static string Decrypt(string encrypted)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.IV = Encoding.UTF8.GetBytes(AesIV256);
            aes.Key = Encoding.UTF8.GetBytes(AesKey256);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Convert Base64 strings to byte array
            byte[] src = System.Convert.FromBase64String(encrypted);

            // decryption
            using (ICryptoTransform decrypt = aes.CreateDecryptor())
            {
                byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                return Encoding.Unicode.GetString(dest);
            }
        }

    }
}

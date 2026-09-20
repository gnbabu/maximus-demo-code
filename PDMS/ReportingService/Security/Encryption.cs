using ReportingService.Common.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Services.Security
{
    public class Encryption
    {
        public static byte[] CreateSalt(int size)
        {
            // generate a cryptographic random number
            using (var provider = RandomNumberGenerator.Create())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);

                return buff;
            }
        }

        public static Base64String AESEncrypt(Base64String plainTextBytes, Base64String key)
        {
            if (key.DecodedBytes == null || key.DecodedBytes.Length == 0) throw new ArgumentOutOfRangeException("key cannot be null or empty");
            var cipherText = new Base64String(AESEncrypt(plainTextBytes.DecodedBytes, key.DecodedBytes));
            return cipherText;
        }

        public static Base64String AESEncrypt(string plainText, byte[] key)
        {
            if (key == null || key.Length == 0) throw new ArgumentOutOfRangeException("key cannot be null or empty");
            if (plainText == null) plainText = string.Empty;
            var cipherText = new Base64String(AESEncrypt(Encoding.UTF8.GetBytes(plainText), key));
            return cipherText;
        }

        /// <summary>
        /// AES Encryption using a randomly generated Initialization Vector (IV).
        /// IV is prepended to the cipherText.
        /// </summary>
        /// <param name="plainTextBytes"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] AESEncrypt(byte[] plainTextBytes, byte[] key)
        {
            using (var aes = Aes.Create())
            {
                var blkSize = aes.BlockSize / 8;
                var iv = CreateSalt(blkSize);
                return AESEncrypt(plainTextBytes, key, iv, true);
            }
        }

        public static byte[] AESEncrypt(byte[] plainTextBytes, byte[] key, byte[] iv, bool prependIV = true)
        {
            if (key == null || key.Length == 0)
                throw new ArgumentOutOfRangeException("key cannot be null or empty");

            if (iv == null || iv.Length == 0)
                throw new ArgumentOutOfRangeException("Initialization Vector (IV) cannot be null or empty");

            byte[] cipherBytes;

            using (var aes = Aes.Create())
            {
                var blkSize = aes.BlockSize / 8;

                if (iv.Length != blkSize)
                    throw new ArgumentOutOfRangeException("Initialization Vector (IV) length must be equal to the block size: " + blkSize + " bytes");

                var cryptoEngine = aes.CreateEncryptor(key, iv);

                using (var msEncrypt = new MemoryStream())
                {
                    if (prependIV)
                    {
                        msEncrypt.Write(iv, 0, iv.Length);
                    }

                    using (var csEncrypt = new CryptoStream(msEncrypt, cryptoEngine, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock(); // ✅ Important for correct encryption
                    }

                    cipherBytes = msEncrypt.ToArray();
                }
            }

            return cipherBytes;
        }

        public static Base64String AESDecrypt(Base64String cipherText, Base64String key)
        {
            if (key.DecodedBytes == null || key.DecodedBytes.Length == 0) throw new ArgumentOutOfRangeException("key cannot be null or empty");
            var plainText = AESDecrypt(cipherText.DecodedBytes, key.DecodedBytes);
            return plainText;
        }

        /// <summary>
        /// AES Decrypt. Initialization Vector (IV) is expected to be prepended to the cipherText.
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Base64String AESDecrypt(byte[] cipherText, byte[] key)
        {

            using (var aes = Aes.Create())
            {
                var blkSize = aes.BlockSize / 8;
                return AESDecrypt(cipherText.AsSpan(blkSize).ToArray(), key, cipherText.AsSpan(0, blkSize).ToArray());
            }
        }

        public static Base64String AESDecrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
          
            if (key == null || key.Length == 0) throw new ArgumentOutOfRangeException("key cannot be null or empty");
            if (iv == null || iv.Length == 0) throw new ArgumentOutOfRangeException("Initialization Vector (IV) cannot be null or empty");

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipherText))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return new Base64String(Encoding.UTF8.GetBytes(sr.ReadToEnd()));
                }
            }
        }
    }
}

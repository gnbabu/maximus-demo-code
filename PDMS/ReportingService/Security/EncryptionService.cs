using ReportingService.Common.Text;
using ReportingService.Services.Helper;
using System;
using System.Security.Cryptography;
using System.Text;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Services.Security
{
    /// <summary>
    /// Encryption service
    /// </summary>
    public class EncryptionService : IEncryptionService
    {
        private readonly ISecretsMgrRepository _secretsRepo;
        private readonly string _keySecretName;
        public const string DefaultKeyName = "DefaultEncryptionKey";

        public EncryptionService(ISecretsMgrRepository secretsRepo) : this(secretsRepo, DefaultKeyName) { }

        public EncryptionService(ISecretsMgrRepository secretsRepo, string keySecretName)
        {
            _secretsRepo = secretsRepo;
            _keySecretName = keySecretName;
        }

        private string EncryptionKey => _secretsRepo.GetSecretString(_keySecretName);

        private string AesEncryptTextToMemory(string data, byte[] key)
        {
            Base64String cipherText = Encryption.AESEncrypt(data, key);
            return cipherText.Value;
        }

        private string AesDecryptTextFromMemory(byte[] data, byte[] key)
        {
            Base64String plainText = Encryption.AESDecrypt(data, key);
            return plainText.DecodedString;
        }

        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <returns>Salt key</returns>
        public virtual string CreateSaltKey(int size)
        {
            using (var provider = RandomNumberGenerator.Create())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);

                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat)
        {
            return HashHelper.CreateHash(Encoding.UTF8.GetBytes(string.Concat(password, saltkey)), passwordFormat);
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = EncryptionKey;

            return AesEncryptTextToMemory(plainText, KeyStringToBytes(encryptionPrivateKey));
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            if (string.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = EncryptionKey;

            byte[] buffer = Convert.FromBase64String(cipherText);
            return AesDecryptTextFromMemory(buffer, KeyStringToBytes(encryptionPrivateKey));
        }

        private byte[] KeyStringToBytes(string key)
        {
            return Encoding.ASCII.GetBytes(key.Substring(0, 16));
        }
    }
}
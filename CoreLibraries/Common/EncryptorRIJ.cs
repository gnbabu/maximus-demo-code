using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Configuration;
using System.IO;
using System.Web.Configuration;
using MAXIMUS.Core.Libraries;

namespace Corp.Core.Libraries
{
    public class EncryptorRIJ
    {
        private RijndaelManaged _rijndaelEncryptor;

        private RijndaelManaged RijndaelEncryptor
        {
            get
            {
                if (_rijndaelEncryptor == null)
                {
                    _rijndaelEncryptor = new RijndaelManaged();
                    _rijndaelEncryptor.Key = StringToHex(AppSettings.Get("RijndaelKey"));
                    _rijndaelEncryptor.IV = StringToHex(AppSettings.Get("RijndaelIV"));
                }

                return _rijndaelEncryptor;
            }
        }

        public byte[] EncryptRijndael(byte[] data)
        {
            return Encrypt(data, RijndaelEncryptor);
        }



        public byte[] EncryptAES(byte[] data)
        {
            AesCryptoServiceProvider newAes = null;

            newAes = new AesCryptoServiceProvider();
            newAes.Key = StringToHex(WebConfigurationManager.AppSettings["AESKey"]);
            newAes.Mode = CipherMode.ECB;

            return Encrypt(data, newAes);
        }

        public byte[] Encrypt(byte[] data, SymmetricAlgorithm algorithm)
        {
            byte[] encryptedData;

            try
            {

                using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
                {
                    // Create the streams used for encryption. 
                    using (MemoryStream msFile = new MemoryStream(data))
                    {
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    byte[] bytearrayinput = new byte[msFile.Length];
                                    msFile.Read(bytearrayinput, 0, bytearrayinput.Length);
                                    csEncrypt.Write(bytearrayinput, 0, bytearrayinput.Length);
                                    //Write all data to the stream.

                                }
                                encryptedData = msEncrypt.ToArray();
                            }
                        }
                    }
                }
                return encryptedData;
            }
            catch
            {
                /// Handle exception
                return null;
            }
        }

        public byte[] DecryptAES(byte[] encryptedData)
        {
            AesCryptoServiceProvider newAes = new AesCryptoServiceProvider();
            newAes.Key = StringToHex(WebConfigurationManager.AppSettings["AESKey"]);
            newAes.Mode = CipherMode.ECB;

            return Decrypt(encryptedData, newAes);
        }

        public byte[] DecryptRijndael(byte[] encryptedData)
        {
            return Decrypt(encryptedData, RijndaelEncryptor);
        }

        public byte[] Decrypt(byte[] encryptedData, SymmetricAlgorithm algorithm)
        {
            byte[] data;

            try
            {
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
                {
                    // Create the streams used for encryption. 
                    using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (MemoryStream msOutput = new MemoryStream())
                            {
                                using (BinaryWriter swDecrypt = new BinaryWriter(msOutput))
                                {

                                    byte[] buffer = new byte[encryptedData.Length];
                                    var read = csDecrypt.Read(buffer, 0, buffer.Length);
                                    swDecrypt.Write(buffer, 0, read);

                                    swDecrypt.Flush();
                                    csDecrypt.Flush();
                                }
                                data = msOutput.ToArray();
                            }
                        }
                    }
                }
                return data;
            }
            catch
            {
                /// Handle exception
                return null;
            }
        }

        public byte[] StringToHex(string str)
        {
            int NumberChars = str.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            }
            return bytes;
        }
    }
}

//using Org.BouncyCastle.Bcpg.OpenPgp;
//using Org.BouncyCastle.Crypto;
//using Org.BouncyCastle.Security;
//using Org.BouncyCastle.Utilities.IO;
//using Org.BouncyCastle.Bcpg;
using Starksoft.Cryptography.OpenPGP;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     Author(s):      jfetters
    ///     Date:           2012.02.29
    ///     Name:           PgpDecrypt
    ///     Description:    Provides the ability to decrypt a PGP encrypted file
    ///     Additional Information:     
    ///         Bouncy Castle OpenPGP library wrapper
    ///         http://www.bouncycastle.org/csharp/
    ///         http://www.bouncycastle.org/docs/
    ///         From: http://rafayal.blogspot.com/2009/06/pgp-decryption-with-c.html
    /// </summary>
    public class PgpDecrypt
    {

        #region "Constructors"

        /// <summary>
        ///     The default parameterless constructor. Generates a new GUID for the Logging threadId
        /// </summary>
        public PgpDecrypt()
        {
            // generate a new thread id GUID
            this.ThreadId = Guid.NewGuid();
        }

        /// <summary>
        ///     The parameterized constructor which provides the Logging threadId GUID.
        /// </summary>
        /// <param name="threadId">The GUID which is used for tying all log entires across all classes.</param>
        public PgpDecrypt(Guid threadId)
        {
            this.ThreadId = threadId;
        }

#endregion
#region "Logging Objects"

        private int logCnt = 0;
        private Guid m_threadId;
        private Guid ThreadId
        {
            get
            {
                return this.m_threadId;
            }
            set
            {
                this.m_threadId = value;
            }
        }

 #endregion

        /// <summary>
        ///     Decrypts a PGP encrypted file
        /// </summary>
        /// <param name="encryptedFile">The location and name of the encrypted file</param>
        /// <param name="outputPath">the location where the method should write the decrypted file</param>
        /// <returns>The location and name of the decrypted file</returns>
        public string DecryptFile(FileInfo encryptedFile, string outputPath)
        {

            Logging log = new Logging(this.ThreadId, "Decrypting File");
            string outputFileNameFullPath = outputPath + encryptedFile.Name.Replace(AppSettings.Get("PGPFileExtension"), string.Empty);
            try
            {

                log.CreateLogEntry("Initializing GPG and encrypted/decrypted files", +logCnt);

                // fire up gpg and file objects
                GnuPG gpg = new GnuPG();
                FileStream encryptedStream = File.Open(encryptedFile.FullName, FileMode.Open);
                FileStream decryptedStream = File.Open(outputFileNameFullPath, FileMode.OpenOrCreate);
                gpg.Passphrase = AppSettings.Get(CryptoConstants.Passphrase);

                log.CreateLogEntry("Attempting to decrypt " + encryptedFile.FullName, +logCnt);

                gpg.Decrypt(encryptedStream, decryptedStream);

                log.CreateLogEntry("PGP decryption called and processing, system will pause to allow decryption to complete", +logCnt);
                
                System.Threading.Thread.Sleep(Convert.ToInt16(AppSettings.Get("PGPSleepInMilliseconds")));

                log.CreateLogEntry("Pause complete, file decrypted " + outputFileNameFullPath, +logCnt);
 
                encryptedStream.Close();
                decryptedStream.Close();

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PgpDecrypt.DecryptFile failed. Reason: " + ex.Message, Logging.LogPriority.Error, +logCnt);
                throw ex;
            }
            // return outputFileName;
            return outputFileNameFullPath;
        }
}
}

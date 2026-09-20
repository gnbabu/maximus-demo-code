using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.IO;


namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     Author(s):      jfetters
    ///     Date:           2012.02.29
    ///     Name:           PGPKeys
    ///     Description:    Nested class to get information on out PGP Keys
    ///     Additional Information:
    ///         From: http://rafayal.blogspot.com/2009/06/pgp-encryption-with-c.html
    /// </summary>
    public class PGPKeys
    {
        /// <summary>
        /// long representing the key ID we want to target.
        /// </summary>
        private long _keyID;
        /// <summary>
        /// string to represent the path to the private key file.
        /// </summary>
        private string _privKeyPath;
        /// <summary>
        /// string to represent the path to the public key file.
        /// </summary>
        private string _pubKeyPath;
        /// <summary>
        /// string to represent the passphrase used with PGP.
        /// </summary>
        private string _password;
        /// <summary>
        ///     The location and name of the public key
        /// </summary>
        public PgpPublicKey PGPPublicKey { get; private set; }
        /// <summary>
        ///     The location and name of the private key
        /// </summary>
        public PgpPrivateKey PGPPrivateKey { get; private set; }
        /// <summary>
        ///     The location and name of the secret key
        /// </summary>
        public PgpSecretKey PGPSecretKey { get; private set; }
        /// <summary>
        /// Constructor to manage variables for us.
        /// </summary>
        /// <param name="pubKeyPath">string representing the public key path</param>
        /// <param name="privKeyPath">string representing the private key path</param>
        /// <param name="password">string representing the passphrase</param>
        /// <param name="keyID">long representing the key ID we want to use</param>
        public PGPKeys(string pubKeyPath, string privKeyPath, string password, long keyID)
        {
            if (!File.Exists(pubKeyPath))
            {
                throw new ArgumentNullException("Could not find the public key at " + pubKeyPath);
            }
            else
            {
                _pubKeyPath = pubKeyPath;
            }
            if (!File.Exists(privKeyPath))
            {
                throw new ArgumentNullException("Could not find the private key at " + privKeyPath);
            }
            else
            {
                _privKeyPath = privKeyPath;
            }
            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("The password must not be null");
            }
            else
            {
                _password = password;
            }
            if (keyID == 0)
            {
                throw new ArgumentNullException("The key ID must not be null");
            }
            else
            {
                _keyID = keyID;
            }
            PGPPublicKey = getPublicKey(_pubKeyPath);
            PGPSecretKey = getSecretKey(_privKeyPath);
            PGPPrivateKey = getPrivateKey(_password);
        }
        /// <summary>
        /// private method to get the public key value.
        /// </summary>
        /// <param name="_pubKeyPath">string representing the public key path</param>
        /// <returns>a PGPPublicKey</returns>
        private PgpPublicKey getPublicKey(string _pubKeyPath)
        {
            PgpPublicKey pubKey;
            using (Stream keyin = File.OpenRead(_pubKeyPath))
            using (Stream s = PgpUtilities.GetDecoderStream(keyin))
            {
                PgpPublicKeyRingBundle pubKeyBundle = new PgpPublicKeyRingBundle(s);
                pubKey = pubKeyBundle.GetPublicKey(_keyID);
                if (pubKey == null)
                    throw new Exception("The public key value is null!");
            }
            return pubKey;
        }
        /// <summary>
        /// private method to get the secret key value.
        /// </summary>
        /// <param name="_privKeyPath">string representing the private key path</param>
        /// <returns>a PGPSecretKey</returns>
        private PgpSecretKey getSecretKey(string _privKeyPath)
        {
            PgpSecretKey secKey;
            using (Stream keyin = File.OpenRead(_privKeyPath))
            using (Stream s = PgpUtilities.GetDecoderStream(keyin))
            {
                PgpSecretKeyRingBundle secKeyBundle = new PgpSecretKeyRingBundle(s);
                secKey = secKeyBundle.GetSecretKey(_keyID);
                if (secKey == null)
                    throw new Exception("The secret key value is null!");
            }
            return secKey;
        }
        /// <summary>
        /// private method to get the private key value.
        /// </summary>
        /// <returns>a PGPPrivateKey</returns>
        private PgpPrivateKey getPrivateKey(string _password)
        {
            PgpPrivateKey privKey = PGPSecretKey.ExtractPrivateKey(_password.ToCharArray());
            if (privKey == null)
            {
                return null;
            }
            else
            {
                return privKey;
            }
        }

    }
}

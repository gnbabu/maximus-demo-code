using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAXIMUS.Core.Libraries
{
    /// <summary>
    ///     Author(s):      jfetters
    ///     Date:           2012.03.02
    ///     Name:           CryptoConstants
    ///     Description:    Provides project-wide items which are used by the classes of the Cryptography project.
    /// </summary>
    internal class CryptoConstants
    {
        /// <remarks>
        ///     Private constructor to prevent instantiation of, or inheritance
        ///     from, this class.
        /// </remarks>
        private CryptoConstants()
        {
            // no action
        }

        internal const string Passphrase = "PGPPhrase";
        internal const string PrivateKey = "PGPPvtKey";
        internal const string PublicKey = "PGPPubKey";
        internal const string KeyId = "PGPKeyId";
        internal const string CommandPath = "PGPCommandPath";
        internal const string CommandString = "PGPCommandString";
    }
}

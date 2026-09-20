using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Data = System.Collections.Generic.KeyValuePair<string, string>;

namespace Corp.Core.Libraries
{
    public class CertUtil
    {
        private X509Store _store;

        public CertUtil(StoreName store = StoreName.My, StoreLocation storeLocation = StoreLocation.LocalMachine)
        {
            _store = new X509Store(store, storeLocation);
        }

        public List<Data> GetCertNames()
        {
            var results = new List<Data>();

            try
            {
                _store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = _store.Certificates;
                try
                {
                    foreach (X509Certificate2 cert in certificatesInStore)
                    {
                        if (cert.HasPrivateKey && !String.IsNullOrEmpty(cert.SubjectName?.Name))
                            results.Add(new Data(cert.Thumbprint, cert.SubjectName?.Name?.Replace("CN=", "")));
                        //results.Add(cert.SubjectName?.Name?.Replace("CN=", ""));
                    }
                }
                catch { }  // Don't add bad certs
            }
            finally
            {
                _store.Close();
            }

            return results;
        }

        public X509Certificate2 GetCertificateByName(string certname)
        {
            //certname = Regex.Replace(certname, @"[^\da-fA-F]", string.Empty).ToUpper();
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            try
            {
                var clientCerts = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, certname, false);
                if (clientCerts.Count == 0)
                {
                    return null;
                }

                return clientCerts[0];
            }
            finally
            {
                store.Close();
            }
        }

        public X509Certificate2 GetCertificateByThumbprintNew(string thumbprint)
        {
            // strip any non-hexadecimal values and make uppercase
            thumbprint = Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            try
            {
                store.Open(OpenFlags.ReadOnly);

                var certCollection = store.Certificates;
                var signingCert = certCollection.Find(X509FindType.FindByThumbprint, thumbprint, false);
                if (signingCert.Count == 0)
                {
                    throw new FileNotFoundException(string.Format("Cert with thumbprint: '{0}' not found in local machine cert store.", thumbprint));
                }

                return signingCert[0];
            }
            finally
            {
                store.Close();
            }
        }
    }
}

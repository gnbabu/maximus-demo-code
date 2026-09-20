using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace WCFClientUtil
{
    public class CertUtil
    {
        private X509Store _store;

        public CertUtil(StoreName store = StoreName.My,  StoreLocation storeLocation = StoreLocation.CurrentUser)
        {
            _store = new X509Store(store, storeLocation);
        }

        public List<string> GetCertNames()
        {
            var results = new List<string>();

            try
            {
                _store.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = _store.Certificates;
                try
                {
                    foreach (X509Certificate2 cert in certificatesInStore)
                    {
                        if (cert.HasPrivateKey && !String.IsNullOrEmpty(cert.SubjectName?.Name))
                            results.Add(cert.SubjectName?.Name?.Replace("CN=", ""));
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
            _store.Open(OpenFlags.ReadOnly);

            try
            {
                var clientCerts = _store.Certificates.Find(X509FindType.FindBySubjectName, certname, false);
                if (clientCerts.Count == 0)
                {
                    return null;
                }

                return clientCerts[0];
            }
            finally
            {
                _store.Close();
            }
        }
    }
}

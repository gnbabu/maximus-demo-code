using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;

namespace WebApiClient.Infrastructure
{
    internal static class HttpClientServiceBase
    {        

        public static X509Certificate2 GetCertificateByName(string certname)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly);

            var clientCerts = store.Certificates.Find(X509FindType.FindBySubjectName, certname, false);

            store.Close();

            if (clientCerts.Count == 0)
            {
                return null;
            }

            return clientCerts[0];
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Security.Cryptography.X509Certificates;

namespace WCFClientUtil
{
    public class WcfAuthorizedClient<TClient, TInterface>
        where TClient : class
        where TInterface : class
    {
        public static TClient GetClient(string endPoint, string userName, string password, string certName)
        {
            var certUtil = new CertUtil();
            X509Certificate2 cert = certUtil.GetCertificateByName(certName);

            return ConfigureClient(endPoint, userName, password, cert);
        }

        public static TClient GetClient(string endPoint, string userName, string password, X509Certificate2 cert)
        {
            return ConfigureClient(endPoint, userName, password, cert);
        }


        private static TClient ConfigureClient(string endPoint, string userName, string password, X509Certificate2 cert)
        {
            var myBinding = new WSHttpBinding();
            myBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            myBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            myBinding.Security.Message.EstablishSecurityContext = false;

            EndpointAddress ea = new EndpointAddress(endPoint);

            TClient result = Activator.CreateInstance<TClient>();
            ClientBase<TInterface> client = result as ClientBase<TInterface>;

            client.Endpoint.Address = ea;
            client.Endpoint.Binding = myBinding;
            client.ClientCredentials.ClientCertificate.Certificate = cert;
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;

            return result;
        }
    }
}

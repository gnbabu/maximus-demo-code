using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WebServiceClient.PDMSWebApi;

namespace WebServiceClient
{
    public class UserInfoWebService : IDisposable
    {
        private UserInfoClient client;
        public UserInfoWebService(string address)
        {
            var binding = new BasicHttpsBinding();
            binding.Security.Mode = BasicHttpsSecurityMode.Transport;

            EndpointAddress ea = new EndpointAddress(address);

            client = new UserInfoClient(binding, ea);
        }

        public UserInfoWebService(string address, string userName, string password)
        {
            var myBinding = new WSHttpBinding();
            myBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            myBinding.Security.Transport.ClientCredentialType =
               HttpClientCredentialType.None;
            myBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            myBinding.Security.Message.EstablishSecurityContext = false;

            EndpointAddress ea = new EndpointAddress(address);

            client = new UserInfoClient(myBinding, ea);

            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
        }

        public UserInfoWebService(X509Certificate2 cert, string address, string userName, string password)
        {
            //client = new UserInfoClient();

            //var myBinding = new WSHttpBinding();
            //myBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
            //myBinding.Security.Transport.ClientCredentialType =
            //   HttpClientCredentialType.None;
            //myBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            //myBinding.Security.Message.EstablishSecurityContext = false;

            var myBinding = new BasicHttpsBinding();
            myBinding.Security.Mode = BasicHttpsSecurityMode.TransportWithMessageCredential;
            myBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;

            myBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            EndpointAddress ea = new EndpointAddress(address);

            client = new UserInfoClient(myBinding, ea);

            client.ClientCredentials.ClientCertificate.Certificate = cert;
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;
        }

        public string GetUserInfo(string userName, string password)
        {
            var result = client.GetUserInfo(userName, password);

            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public string EchoSoapRequest()
        {
            var result = client.EchoSoapRequest(1);
            return result.ToString();
        }

        public void Dispose()
        {
            try
            {
                client?.Close();
            }
            catch { }
        }

        public void SetCertificate(X509Certificate2 clientCert)
        {


            client.ClientCredentials.ClientCertificate.Certificate = clientCert;
        }

        public void SetCertificateByName(string certName)
        {
            client.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.My, X509FindType.FindBySubjectName, certName);
        }
    }
}

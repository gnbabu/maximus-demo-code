using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFClientUtil;
using WebServiceClient.AttachmentService; 

namespace WebServiceClient
{
    public class DocumentClient : IDisposable
    {
        private AttachmentServiceClient client;

        public DocumentClient(string endPoint, string userName, string password, X509Certificate2 cert)
        {
            client = WcfAuthorizedClient<AttachmentServiceClient, AttachmentService.AttachmentService>
                .GetClient(endPoint, userName, password, cert);

        }

        public string SendAttachment()
        {
            var header = new SendAttachmentMessageHeader();
            var request = new SendAttachmentPayLoad();
            string transId;
            AttachmentResponse[] response;
            var result = client.SendAttachment(header, request, out transId, out response);
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);

            return json;
        }

        public void Dispose()
        {
            try
            {
                client?.Close();
            }
            catch { }
        }
    }
}

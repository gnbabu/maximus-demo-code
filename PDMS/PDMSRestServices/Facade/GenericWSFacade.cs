using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Models;
using System.Buffers;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace PDMSRestServices.Facade
{
    public class GenericWSFacade
    {
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
        public GenericWSFacade()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;

        public ResponseDetail makeGenericSIWSRequest(string wsUrl, string hostName, string soapAction, string username, string password, string certificateName, string requestWS)
        {
            Models.ResponseDetail responseDetail = new Models.ResponseDetail();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(requestWS);

                string genericWSCertificateName = certificateName;
                string genericWSHost = hostName;

                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(genericWSCertificateName);
                string user = username;
                string passcode = password;
                Uri apiUrl = new Uri(wsUrl);
                WebRequest pmRequest = HttpWebRequest.Create(wsUrl);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = hostName;
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);
                Stream requestStream = pmHttpRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                string response = "";

                using (WebResponse pmResponse = pmHttpRequest.GetResponse())
                {
                    HttpWebResponse pmHttpResponse = (HttpWebResponse)pmResponse;

                    HttpStatusCode statuscode = pmHttpResponse.StatusCode;

                    using (Stream stream = pmResponse.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            response = sr.ReadToEnd();
                        }
                    }
                }
                responseDetail.ResponseCode = "200";
                responseDetail.ResponseDescp = "Success";
                responseDetail.ResponseBody = response;
                return responseDetail;
            }
            catch (WebException ex)
            {
                string respBody = ex.Response != null ? new StreamReader(ex.Response.GetResponseStream()).ReadToEnd() : ex.ToString();
                responseDetail.ResponseCode = "500";
                responseDetail.ResponseDescp = "Failed in WebException";
                responseDetail.ResponseBody = respBody;
                responseDetail.ErrorMessage = ex.Message;
                responseDetail.ErrorStackTrace = ex.StackTrace;
                return responseDetail;
            }
            catch (Exception ex)
            {
                responseDetail.ResponseCode = "500";
                responseDetail.ResponseDescp = "Failed in Exception";
                responseDetail.ResponseBody = "No Response captured";
                responseDetail.ErrorMessage = ex.Message;
                responseDetail.ErrorStackTrace = ex.StackTrace;
                return responseDetail;
            }
        }
    }
}

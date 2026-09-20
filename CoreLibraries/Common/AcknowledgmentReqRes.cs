using Corp.Core.Libraries.AcknowledgmentService;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class AcknowledgmentReqRes
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

        public AcknowledgmentReqRes()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;
        private static string WSPassword_2 = AppSettings.Get("ProviderManagementWSPassword_2");


        public string AcknowledgmentTargetVendorResponse(vendorAcknowledgmentRequest1 targetVendorResponseRequest)
        {
            string logHeader = string.Format("Error in AcknowledgementReqRes for SITransactionKey - {0}:", targetVendorResponseRequest.MessageHeader.SITransactionKey.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            
            string url = AppSettings.Get("AcknowledgementWSURL", Constants.WebServiceURI.AcknowledgementService);
            string wsPassword = AppSettings.Get("AcknowledgementWSPassword");
            string wsUserName = AppSettings.Get("AcknowledgementWSUserName");

            //string client_secret = "";
            //string secretName = "AcknowledgementWSPassword_OH_PNM_";
            //try
            //{
            //    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
            //    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
            //    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
            //    secretResult.Wait();

            //    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            //}
            //catch (Exception ex)
            //{
            //    ThreadId = Guid.NewGuid();
            //    Logging log = new Logging(ThreadId, "Secret Retrieval");
            //    client_secret = "NO SECRET CONNECTIVITY";
            //    client_secret += " ~|~ ";
            //    client_secret += AppSettings.Get("SecretsRegion");
            //    client_secret += " ~|~ ";
            //    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
            //    client_secret += " ~|~ ";
            //    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
            //    client_secret += " ~|~ ";
            //    client_secret += ex.Message;
            //    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            //}
            //string wsPassword = client_secret;

            //string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            
           // targetVendorResponseRequest.SITransactionKey = sitTransactionKey;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(vendorAcknowledgmentRequest1));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, targetVendorResponseRequest, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/acknowledgment\">"
                + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return makeWSRequestResponse(xmlDoc, url, wsUserName, wsPassword, targetVendorResponseRequest.MessageHeader.SITransactionKey.ToString());
        }
        public string makeWSRequestResponse( XmlDocument xmlDoc, string url, string userName, string password, string siKey)
        {
            string logHeader = string.Format("Error in AcknowledgementReqRes for SITransactionKey - {0}:", siKey.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string wsCertificateName = AppSettings.Get("AcknowledgementWSCertificateName");
                string WebServiceHost = AppSettings.Get("SIWebServiceHost");
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
                string user = userName;
                string passcode = WSPassword_2;
                //string client_secret = "";
                //string secretName = "ProviderManagementWSPassword_2_OH_PNM_";
                //try
                //{
                //    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                //    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                //    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                //    secretResult.Wait();

                //    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                //}
                //catch (Exception ex)
                //{
                //    client_secret = "NO SECRET CONNECTIVITY";
                //    client_secret += " ~|~ ";
                //    client_secret += AppSettings.Get("SecretsRegion");
                //    client_secret += " ~|~ ";
                //    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                //    client_secret += " ~|~ ";
                //    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                //    client_secret += " ~|~ ";
                //    client_secret += ex.Message;
                //    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                //}

                //string passcode = client_secret;
                Uri apiUrl = new Uri(url);
                WebRequest pmRequest = HttpWebRequest.Create(url);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                //eVerificationRequest.ContentType = "text/xml; encoding='utf-8'";
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = WebServiceHost;
                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", "\"http://mes.gov/vendorAcknowledgment\"");
                Stream requestStream = pmHttpRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                var response = "";

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

                return response.ToString();
            }
            catch (WebException ex)
            {
                log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                throw ex;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                throw ex;
            }
        }
        public static string wssSecurityHeader(string wsUserName, string wsPassword)
        {
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(wsUserName, wsPassword, PasswordOption.SendPlainText);
                string xml = "<soapenv:Header>" +
                    @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                        usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                    "</wsse:Security>" +
                  "</soapenv:Header>";
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

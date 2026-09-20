using Corp.Core.Libraries.HospiceReference;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class HospiceReqRes
    {
        public static string SearchHospiceResponse(SearchRequest searchRequest)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string wsPassword = AppSettings.Get("HospiceWSPassword");
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SearchRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, searchRequest, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/SearchRequest\">"
                + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return makeWSRequestResponse(xmlDoc, url, wsHost, "\"http://mes.gov/SearchResponses\"", Constants.HospiceServiceResponse.searchResponses, wsUserName, wsPassword
                );
        }
        public static string HospiceInquireRequest(InquireRequest inquireRequest)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string wsPassword = AppSettings.Get("HospiceWSPassword");
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | 
                SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquireRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, inquireRequest, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/InquireRequest\">"
                + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return makeWSRequestResponse(xmlDoc, url,wsHost, "\"http://mes.gov/InquireResponse\"", Constants.HospiceServiceResponse.inquireResponse, wsUserName, wsPassword
                );
        }

        public static string AddUpdateHospice(HospiceRequestResponse hospiceRequestResponse)
        {
            string url = AppSettings.Get("HospiceWSURL", Constants.WebServiceURI.HospiceService);
            string wsHost = AppSettings.Get("HospiceWSHost");
            string wsUserName = AppSettings.Get("HospiceWSUserName");
            string wsPassword = AppSettings.Get("HospiceWSPassword");
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 |
                SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;


            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(HospiceRequestResponse));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, hospiceRequestResponse, emptyNs);
            string xml = stream2.ToString();
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/HospiceRequestResponse\">"
                + Environment.NewLine + wssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return makeWSRequestResponse(xmlDoc, url, wsHost, "\"http://mes.gov/AddUpdateResponse\"", Constants.HospiceServiceResponse.addUpdateResponse, wsUserName, wsPassword
                );
        }
        public static string makeWSRequestResponse(XmlDocument xmlDoc, string url,string host,string soapAction, string responseType,
            string userName, string password)
        {
            try
            {
                string wsCertificateName = AppSettings.Get("HospiceWSCertificateName");
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
                string user = userName;
                string passcode = password;
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
                pmHttpRequest.Host = host;
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
                String encoded = string.Format(System.Convert.ToBase64String(Encoding.ASCII.GetBytes("{0}" + ":" + "{1}")), userName, password);
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction); 
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


                object result;
                var xDoc = XDocument.Parse(response);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals(responseType));//Constants.HospiceServiceResponse.searchResponses

                var serializer = new XmlSerializer(typeof(SoapResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                SoapResponse obj = (SoapResponse)result;
                string ModuleTransactionId = obj.ModuleTransactionId;
                string SITransactionKey = obj.SITransactionKey;
                string ResponseCode = obj.ResponseCode;
                string ResponseDetails = obj.ResponseDetails;
                string ResponseMessage = obj.ResponseMessage;
                string ResponseType = obj.ResponseType;

                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.saveSoapResponseCodeException(ModuleTransactionId, SITransactionKey, ResponseCode,
                ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId), response.ToString());

                return response.ToString();
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
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

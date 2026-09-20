using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Addressing;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{
    public class ServiceAgentHelper
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

        public ServiceAgentHelper()
        {
            ThreadId = Guid.NewGuid();
        }
        private static Logging log = null;

        public static string MakeServiceCall(XmlDocument xmlDoc, string url, string soapAction,
           string userName, string password, string wsCertificateName, string host, string pnmTransactionKey)
        {
            var response = "";
            var responseNS = "";
            var requestType = "";
            var siTransactionKey = "";
            string hsWebAPITesting = AppSettings.Get("HSWebAPITesting", "false");
            int hsTimeOut = Convert.ToInt32(AppSettings.Get("HSResponseTimeOut", "2000"));
            int hsRWTimeOut = Convert.ToInt32(AppSettings.Get("HSRWResponseTimeOut", "2000"));

            if (soapAction.ToLower().Contains("addupdatehospice"))
            {
                siTransactionKey = xmlDoc.GetElementsByTagName("hos:SITransactionKey")[0].InnerText;
                responseNS = "http://ohio.gov/AddUpdateResponse";
                requestType = "AddUpdateHospice";
            }
            else if (soapAction.ToLower().Contains("searchhospice"))
            {
                siTransactionKey = xmlDoc.GetElementsByTagName("sear:SITransactionKey")[0].InnerText;
                responseNS = "http://ohio.gov/SearchResponses";
                requestType = "SearchHospice";
            }
            else if (soapAction.ToLower().Contains("inquirehospice"))
            {
                siTransactionKey = xmlDoc.GetElementsByTagName("inq:SITransactionKey")[0].InnerText;
                responseNS = "http://ohio.gov/HospiceRequestResponse";
                requestType = "InquireHospice";
            }
            else if (soapAction.ToLower().Contains("sendattachment"))
            {
                siTransactionKey = xmlDoc.GetElementsByTagName("att:SITransactionKey")[0].InnerText;
                responseNS = "http://mes.gov/attachment";
                requestType = "SendAttachment";
            }

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(Guid.NewGuid(), logMsg);
            try
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Hospice Request", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                ServicePointManager.Expect100Continue = true;
                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
                Uri apiUrl = new Uri(url);
                WebRequest pmRequest = HttpWebRequest.Create(url);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = host;
                pmHttpRequest.Timeout = hsTimeOut;
                pmHttpRequest.ReadWriteTimeout = hsRWTimeOut;

                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);

                HospiceEnrollmentDA.InsertHospiceEnrollmentServiceReqRes("", requestType, xmlDoc.InnerXml.ToString(), "", "", "", "", "", new Guid(Constants.appAdminUserId), "", pnmTransactionKey);

                if (hsWebAPITesting.Equals("false"))
                {

                    Stream requestStream = pmHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
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
                }
                else
                {
                    DataSet dswa = null;
                    if (soapAction.ToLower().Contains("addupdatehospice"))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("addupdatehospiceWebAPI");
                    }
                    if (soapAction.ToLower().Contains("searchhospice"))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("searchhospiceWebAPI");
                    }
                    if (soapAction.ToLower().Contains("inquirehospice"))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("inquirehospiceWebAPI");
                    }
                    if (soapAction.ToLower().Contains("sendattachment"))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("sendattachmentWebAPI");
                    }

                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }

                log.CreateLogEntry(string.Format("{0} {1}", "Hospice Response : ", response), Logging.LogPriority.Error);

                //Log request and response to Outbound_Hospice_Enrollment_Req_Res table
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                //Deserialize and populate response fields
                GetSoapResponseInfo(response, responseNS, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage, out ResponseType);
                //save to outbound log
                HospiceEnrollmentDA.UpdateHospiceEnrollmentServiceReqRes(SITransactionKey, requestType, xmlDoc.InnerXml.ToString(), response, ResponseCode, ResponseMessage, ResponseType,
                    ResponseDetails, new Guid(Constants.appAdminUserId), "", pnmTransactionKey);

                return response;
            }
            catch (WebException ex)
            {
                //log request and ex as response
                HospiceEnrollmentDA.UpdateHospiceEnrollmentServiceReqRes(siTransactionKey, requestType, xmlDoc.InnerXml.ToString(), "", "", "", "", "",
                    new Guid(Constants.appAdminUserId), ex.ToString(), pnmTransactionKey);
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpresponse = ex.Response as HttpWebResponse;
                    if (httpresponse != null)
                    {
                        //http status code avaliable
                        var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        var xDoc = XDocument.Parse(respBody);
                        log.CreateLogEntry(string.Format("{0} {1}", "Error while service call", ex.ToString()), Logging.LogPriority.Error);
                        log.CreateLogEntry(string.Format("{0} {1}", "Error while service call inner excption", xDoc.ToString()), Logging.LogPriority.Error);
                    }
                    else
                    {
                        // no http status code available
                        log.CreateLogEntry(string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                    }
                }
                else
                {
                    // no http status code available
                    log.CreateLogEntry(string.Format("Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                }
                throw ex;
            }
            catch (Exception ex)
            {
                //log request and ex as response
                HospiceEnrollmentDA.UpdateHospiceEnrollmentServiceReqRes(siTransactionKey, requestType, xmlDoc.InnerXml.ToString(), "", "", "", "", "",
                    new Guid(Constants.appAdminUserId), ex.ToString(), pnmTransactionKey);
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
                throw ex;
            }
        }

        private static void GetSoapResponseInfo(string respBody, string requestNS, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
            out string ResponseMessage, out string ResponseType)
        {
            SITransactionKey = string.Empty;
            ResponseCode = string.Empty;
            ResponseDetails = string.Empty;
            ResponseMessage = string.Empty;
            ResponseType = string.Empty;

            try
            {
                var xDoc = XDocument.Parse(respBody);
                XNamespace ns = XNamespace.Get(requestNS);
                var xServiceResult = xDoc.Root.Descendants(ns + "ResponseHeader").FirstOrDefault();
                SITransactionKey = (string)xServiceResult.Element(ns + "SITransactionKey");
                ResponseCode = (string)xServiceResult.Element(ns + "ResponseCode");
                ResponseDetails = (string)xServiceResult.Element(ns + "ResponseDetails");
                ResponseMessage = (string)xServiceResult.Element(ns + "ResponseMessage");
                ResponseType = (string)xServiceResult.Element(ns + "ResponseType");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }
        private static void GetSoapResponseAttachmentInfo(string respBody, string requestNS, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
    out string ResponseMessage, out string ResponseType)
        {
            SITransactionKey = string.Empty;
            ResponseCode = string.Empty;
            ResponseDetails = string.Empty;
            ResponseMessage = string.Empty;
            ResponseType = string.Empty;

            try
            {
                var xDoc = XDocument.Parse(respBody);
                XNamespace ns = XNamespace.Get(requestNS);
                var xServiceResult = xDoc.Root.Descendants(ns + "SendAttachmentResponse").FirstOrDefault();
                SITransactionKey = (string)xServiceResult.Element(ns + "SITransactionKey");
                var xServiceResult1 = xServiceResult.Descendants(ns + "Response").FirstOrDefault();
                ResponseCode = (string)xServiceResult1.Element(ns + "ResponseCode");
                ResponseDetails = (string)xServiceResult1.Element(ns + "ResponseDetails");
                ResponseMessage = (string)xServiceResult1.Element(ns + "ResponseMessage");
                ResponseType = (string)xServiceResult1.Element(ns + "ResponseType");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }
        public static string MakeServiceCallNew(XmlDocument xmlDoc, string url, string soapAction,
                    string userName, string wsCertificateName, string password_2)
        {
            var response = "";
            string hsWebAPITesting = AppSettings.Get("HSWebAPITesting", "false");
            int hsTimeOut = Convert.ToInt32(AppSettings.Get("HSResponseTimeOut", "2000"));
            int hsRWTimeOut = Convert.ToInt32(AppSettings.Get("HSRWResponseTimeOut", "2000"));
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(Guid.NewGuid(), logMsg);
            try
            {
                log.CreateLogEntry(string.Format("SendAttachment: {0} {1}", "Hospice Response: ", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                ServicePointManager.Expect100Continue = true;
                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
                Uri apiUrl = new Uri(url);
                WebRequest pmRequest = HttpWebRequest.Create(url);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = "dp.test.oh.healthinteractive.net:443";
                pmHttpRequest.Timeout = hsTimeOut;
                pmHttpRequest.ReadWriteTimeout = hsRWTimeOut;

                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password_2));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);

                if (hsWebAPITesting.Equals("false"))
                {

                    Stream requestStream = pmHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();

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
                }
                else
                {
                    DataSet dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("sendAttachmentHospiceWebAPI");
                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }
                log.CreateLogEntry(string.Format("SendAttachment: {0} {1}", "Hospice Response : ", response), Logging.LogPriority.Error);

                return response;

            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpresponse = ex.Response as HttpWebResponse;
                    if (httpresponse != null)
                    {
                        //http status code avaliable
                        var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        var xDoc = XDocument.Parse(respBody);
                        log.CreateLogEntry(string.Format("SendAttachment: {0} {1}", "Error while service call", ex.ToString()), Logging.LogPriority.Error);
                        log.CreateLogEntry(string.Format("SendAttachment: {0} {1}", "Error while service call inner excption", xDoc.ToString()), Logging.LogPriority.Error);
                    }
                    else
                    {
                        // no http status code available
                        log.CreateLogEntry(string.Format("SendAttachment: Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                    }
                }
                else
                {
                    // no http status code available
                    log.CreateLogEntry(string.Format("SendAttachment: Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("SendAttachment: Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
            }
            return response;
        }

        public static string SecurityHeader(string wsUserName, string wsPassword)
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
        public static string MakeServiceCallToSendAttachment(XmlDocument xmlDoc, string url, string soapAction,
           string userName, string password, string wsCertificateName, string host, string type = "")
        {
            var response = "";
            var responseNS = "";
            var requestType = "";
            var siTransactionKey = "";
            string hsWebAPITesting = AppSettings.Get("HSWebAPITesting", "false");
            int hsTimeOut = Convert.ToInt32(AppSettings.Get("HSResponseTimeOut", "2000"));
            int hsRWTimeOut = Convert.ToInt32(AppSettings.Get("HSRWResponseTimeOut", "2000"));
            string SITransactionKey = string.Empty;
            string ResponseCode = string.Empty;
            string ResponseDetails = string.Empty;
            string ResponseMessage = string.Empty;
            string ResponseType = string.Empty;

            if (soapAction.ToLower().Contains("sendattachment"))
            {
                siTransactionKey = xmlDoc.GetElementsByTagName("att:SITransactionKey")[0].InnerText;
                responseNS = "http://mes.gov/attachment";
                requestType = "SendAttachment";
            }

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(Guid.NewGuid(), logMsg);
            try
            {
                log.CreateLogEntry(string.Format("{0} {1}", "Send Attachmenet", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                ServicePointManager.Expect100Continue = true;
                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);
                Uri apiUrl = new Uri(url);
                WebRequest pmRequest = HttpWebRequest.Create(url);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = host;
                pmHttpRequest.Timeout = hsTimeOut;
                pmHttpRequest.ReadWriteTimeout = hsRWTimeOut;

                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);


                if (hsWebAPITesting.Equals("false"))
                {

                    Stream requestStream = pmHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
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
                }
                else
                {
                    DataSet dswa = null;

                    if (soapAction.ToLower().Contains("sendattachment"))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("sendattachmentWebAPI");
                    }

                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }

                log.CreateLogEntry(string.Format("{0} {1}", "Send Attachment Response : ", response), Logging.LogPriority.Error);

                //Log request and response to Outbound_Hospice_Enrollment_Req_Res table


                //Deserialize and populate response fields
                GetSoapResponseAttachmentInfo(response, responseNS, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage, out ResponseType);
                //save to outbound log
                // Create an XmlNamespaceManager to resolve the default namespace.
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsmgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
                nsmgr.AddNamespace("att", "http://mes.gov/attachment");

                // Select the first book written by an author whose last name is Atwood.

                XmlElement root = xmlDoc.DocumentElement;
                XmlNode oldchild = root.SelectSingleNode("descendant::soapenv:Body/att:SendAttachment/att:Payload/att:AttachmentInfo/att:AttachmentData", nsmgr);
                XmlNode newchild = oldchild.RemoveChild(root.SelectSingleNode("descendant::soapenv:Body/att:SendAttachment/att:Payload/att:AttachmentInfo/att:AttachmentData", nsmgr).LastChild);

                SendAttachmentReqRes.SaveAttachmentServiceReqRes(SITransactionKey, requestType, xmlDoc.InnerXml.ToString(), response, ResponseCode, ResponseMessage, ResponseType,
                    ResponseDetails, new Guid(Constants.appAdminUserId), "");

                return response;
            }
            catch (WebException ex)
            {
                //log request and ex as response
                SendAttachmentReqRes.SaveAttachmentServiceReqRes(SITransactionKey, requestType, xmlDoc.InnerXml.ToString(), response, ResponseCode, ResponseMessage, ResponseType, ResponseDetails,
                    new Guid(Constants.appAdminUserId), ex.ToString());
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var httpresponse = ex.Response as HttpWebResponse;
                    if (httpresponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        log.CreateLogEntry(string.Format("CPC Upload Attachment - 403 Forbidden Access to the remote server. Exception Message : {0}, Inner Exception : {1}",
        ex.Message, ex.InnerException), Logging.LogPriority.Error);

                    }
                    if (httpresponse != null)
                    {
                        //http status code avaliable
                        var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        var xDoc = XDocument.Parse(respBody);
                        log.CreateLogEntry(string.Format("CPC Upload Attachment - {0} {1}", "Error while service call", ex.ToString()), Logging.LogPriority.Error);
                        log.CreateLogEntry(string.Format("CPC Upload Attachment - {0} {1}", "Error while service call inner excption", xDoc.ToString()), Logging.LogPriority.Error);
                        return respBody;
                    }
                    else
                    {
                        // no http status code available
                        log.CreateLogEntry(string.Format("CPC Upload Attachment - Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                    }
                }
                else
                {
                    // no http status code available
                    log.CreateLogEntry(string.Format("CPC Upload Attachment - Response is null, unable to make connection. Exception Message : {0}, Inner Exception : {1}",
                            ex.Message, ex.InnerException), Logging.LogPriority.Error);
                }
                return response;
                throw ex;
            }
            catch (Exception ex)
            {
                //log request and ex as response
                SendAttachmentReqRes.SaveAttachmentServiceReqRes(siTransactionKey, requestType, xmlDoc.InnerXml.ToString(), "", "", "", "", "",
                    new Guid(Constants.appAdminUserId), ex.ToString());
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
                return response;
                throw ex;
            }
        }
    }
}
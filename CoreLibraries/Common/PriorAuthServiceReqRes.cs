using Corp.Core.Libraries.PriorAuthServiceReference;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{
    public class PriorAuthServiceReqRes
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

        public PriorAuthServiceReqRes()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;

        private string authRespone = null;
        public string priorAuthAddUpdateOperation(int transactionID, AddUpdatePriorAuthRequest auReqPay)
        {
            string logHeader = string.Format("PriorAuthService: for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string priorAuthWSUserName = AppSettings.Get("PriorAuthServiceWSUserName");
                string client_secret = "";
                string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword = client_secret;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AddUpdatePriorAuthRequest));
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var streamRequest = new StringWriter();
                var writer = XmlWriter.Create(streamRequest, settings);
                x.Serialize(writer, auReqPay, ns);
                string xml = streamRequest.ToString();

                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                    + Environment.NewLine + wssSecurityHeader(priorAuthWSUserName, priorAuthWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
                var auPrior = xmlDoc.InnerXml.ToString().Replace("AddUpdatePriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "AddUpdatePriorAuthRequest")
                    .Replace("AddUpdatePriorAuthRequest", "pri:AddUpdatePriorAuthRequest")
                    .Replace("MessageHeader", "pri:MessageHeader")
                    .Replace("PriorAuthRequest278", "pri:PriorAuthRequest278");

                xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(auPrior);

                return makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.AddUpdatePriorAuth, CON.PriorAuthSearchSoapAction.AddUpdatePriorAuth);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                return Constants.TransactionResult.TransactionFailed;
            }
        }

        public string priorAuthInquiryOperation(int transactionID, InquirePriorAuthRequest auReqPay)
        {
            string logHeader = string.Format("PriorAuthService: for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string priorAuthWSUserName = AppSettings.Get("PriorAuthServiceWSUserName");
                string client_secret = "";
                string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword = client_secret;


                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquirePriorAuthRequest));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var streamRequest = new StringWriter();
                var writer = XmlWriter.Create(streamRequest, settings);
                x.Serialize(writer, auReqPay, ns);
                string xml = streamRequest.ToString();

                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                    + Environment.NewLine + wssSecurityHeader(priorAuthWSUserName, priorAuthWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
                var inqPrior = xmlDoc.InnerXml.ToString().Replace("InquirePriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "InquirePriorAuthRequest")
                    .Replace("InquirePriorAuthRequest", "pri:InquirePriorAuthRequest")
                    .Replace("MessageHeader", "pri:MessageHeader")
                    .Replace("RequestPayload", "pri:RequestPayload");

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(inqPrior);

                return makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.InquirePriorAuth, CON.PriorAuthSearchSoapAction.InquirePriorAuth);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                return Constants.TransactionResult.TransactionFailed;
            }
        }

        //Prior Auth Search 
        public string priorAuthSearchOperation(int transactionID, SearchPriorAuthRequest auReqPay)
        {
            string logHeader = string.Format("PriorAuthService: for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string priorAuthWSUserName = AppSettings.Get("PriorAuthServiceWSUserName");
                string client_secret = "";
                string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword = client_secret;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SearchPriorAuthRequest));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var streamRequest = new StringWriter();
                var writer = XmlWriter.Create(streamRequest, settings);
                x.Serialize(writer, auReqPay, ns);
                string xml = streamRequest.ToString();

                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\">"
                    + Environment.NewLine + wssSecurityHeader(priorAuthWSUserName, priorAuthWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                xmlDoc.DocumentElement.SetAttribute("xmlns:pri", "http://service.caremgmt.fi/PriorAuthService");
                var searchPrior = xmlDoc.InnerXml.ToString().Replace("SearchPriorAuthRequest xmlns:pri=\"http://service.caremgmt.fi/PriorAuthService\"", "SearchPriorAuthRequest")
                    .Replace("SearchPriorAuthRequest", "pri:SearchPriorAuthRequest")
                    .Replace("MessageHeader", "pri:MessageHeader")
                    .Replace("RequestPayload", "pri:RequestPayload");

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(searchPrior);


                DataSet ds = new DataSet();
                var retResult = makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.SearchPriorAuth, CON.PriorAuthSearchSoapAction.SearchPriorAuth);
                if (retResult.Equals(Constants.TransactionResult.TransactionPassed))
                {
                    return authRespone;
                }
                else
                {
                    return Constants.TransactionResult.TransactionFailed;
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                return "";
            }
        }

        public string makePriorAuthAddUpdateRequest(int transactionID, DataSet requestData, int subscriber)
        {

            bool makeRequest = false;
            string strCheck = AppSettings.Get("MakePAWSRequestCallToSI", "false");
            if (strCheck.Equals("true"))
            {
                makeRequest = true;
            }
            string logHeader = string.Format("PriorAuth AddUpdate for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string response = string.Empty;
                string priorAuthWSUserName = AppSettings.Get("PriorAuthServiceWSUserName");
                string client_secret = "";
                string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword = client_secret;

                AddUpdatePriorAuthRequest auReqPay = new AddUpdatePriorAuthRequest();
                auReqPay.MessageHeader = PriorAuthServiceController.fillMessageHeader(transactionID, subscriber, 0);
                auReqPay.PriorAuthRequest278 = PriorAuthServiceController.fillPriorAuthRequest278Type(requestData);

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(AddUpdatePriorAuthRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                emptyNs.Add("pri", "http://service.caremgmt.fi/PriorAuthService");
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, auReqPay, emptyNs);
                string xml = stream2.ToString();
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://service.caremgmt.fi/PriorAuthService\">"
                    + Environment.NewLine + wssSecurityHeader(priorAuthWSUserName, priorAuthWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(xml);

                if (makeRequest)
                {
                    return makePriorAuthWSRequestResponse(transactionID, xmlDoc, CON.PriorAuthServiceResponse.AddUpdatePriorAuth, CON.PriorAuthSearchSoapAction.AddUpdatePriorAuth);
                }
                else
                {
                    return xmlDoc.InnerXml.ToString();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuth AddUpdate {0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                return Constants.TransactionResult.TransactionFailed;
            }
        }


        public string makePriorAuthWSRequestResponse(int tranasctionID, XmlDocument xmlDoc, string service, string soapAction)
        {
            string logHeader = string.Format("PriorAuthService: Error for Prior Auth Service- {0}:", xmlDoc.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string priorAuthWSURL = AppSettings.Get("PriorAuthServiceWSURL");
                //string priorAuthWSURL = "http://localhost:9092/PriorAuthServiceSOAPVS";
                string priorAuthWSUserName = AppSettings.Get("PriorAuthServiceWSUserName");
                string client_secret = "";
                string secretName = "PriorAuthServiceWSPassword_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword = client_secret;

                client_secret = "";
                secretName = "PriorAuthServiceWSPassword_2_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret = "NO SECRET CONNECTIVITY";
                    client_secret += " ~|~ ";
                    client_secret += AppSettings.Get("SecretsRegion");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret += " ~|~ ";
                    client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret += " ~|~ ";
                    client_secret += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                string priorAuthWSPassword_2 = client_secret;
                string PriorAuthWSCertificateName = AppSettings.Get("PriorAuthServiceWSCertificateName");
                string prioAuthWSHost = AppSettings.Get("SIWebServiceHost");
                string paWebAPITesting = AppSettings.Get("PAWebAPITesting", "false");

                string ModuleTransactionId = string.Empty;
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                InfoAccessController.SaveSoapPriorAuthRequestPayload(tranasctionID, DateTime.Now, new Guid(Constants.appAdminUserId), xmlDoc.InnerXml.ToString());

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(PriorAuthWSCertificateName);
                string user = priorAuthWSUserName;
                string passcode = priorAuthWSPassword_2;
                Uri apiUrl = new Uri(priorAuthWSURL);
                WebRequest paRequest = HttpWebRequest.Create(priorAuthWSURL);
                HttpWebRequest paHttpRequest = (HttpWebRequest)paRequest;
                byte[] bytes;
                bytes = Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                paHttpRequest.ContentType = "text/xml; charset=utf-8";

                paHttpRequest.KeepAlive = true;
                paHttpRequest.Method = "POST";
                paHttpRequest.SendChunked = true;
                paHttpRequest.UserAgent = ".NET Framework";
                paHttpRequest.Host = prioAuthWSHost;

                if (clientCertificate != null)
                {
                    paHttpRequest.ClientCertificates.Add(clientCertificate);
                }

                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                paHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                paHttpRequest.Headers.Add("SOAPAction", soapAction);

                string response = "";
                if (paWebAPITesting.Equals("false"))
                {
                    Stream requestStream = paHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();

                    using (WebResponse pmResponse = paHttpRequest.GetResponse())
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
                    if (service.Equals(CON.PriorAuthServiceResponse.SearchPriorAuth))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("PriorAuthSearchWebAPI");
                    }
                    if (service.Equals(CON.PriorAuthServiceResponse.InquirePriorAuth))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("PriorAuthInquiryWebAPI");
                    }
                    if (service.Equals(CON.PriorAuthServiceResponse.AddUpdatePriorAuth))
                    {
                        dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("PriorAuthAddUpdateWebAPI");
                    }
                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }
                try
                {
                    //log is temporary
                    log.CreateLogEntry(string.Format("PriorAuthService: {0} {1}", logHeader, getXMLPA(response).ToString()), Logging.LogPriority.Debug);

                    DeserializeSoapPriorAuthResponse(response, service, out ModuleTransactionId, out ResponseCode, out ResponseType, out ResponseDetails, out ResponseMessage, out SITransactionKey);

                    InfoAccessController.SaveSoapPriorAuthResponseCodeException(tranasctionID, SITransactionKey, ResponseCode,
                        ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId), getXMLPA(response));

                    if (ResponseType.Equals(Constants.ResponseTypes.SPA_success, StringComparison.InvariantCultureIgnoreCase))
                    {
                        authRespone = response;
                        return CON.TransactionResult.TransactionPassed;
                    }
                    else
                    {
                        return CON.TransactionResult.TransactionFailed;
                    }
                }
                catch (Exception ex)
                {
                    log.CreateLogEntry(string.Format("PriorAuthService: {0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                    return CON.TransactionResult.TransactionFailed;
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        //http status code avaliable
                        log.CreateLogEntry(string.Format("PriorAuthService: {0} {1} {2}", logHeader, ex.ToString(), response.ToString()), Logging.LogPriority.Error);
                        processResponse(ex, service, tranasctionID);
                        return CON.TransactionResult.TransactionFailed;
                    }
                    else
                    {
                        // no http status code available
                        log.CreateLogEntry(string.Format("PriorAuthService: {0} {1} {2}", logHeader, ex.ToString(), response.ToString()), Logging.LogPriority.Error);
                        InfoAccessController.UpdatePriorAuthTransactionQueueSIResponse(tranasctionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        return CON.TransactionResult.TransactionFailed;
                    }
                }
                else
                {
                    // no http status code available and check for null response - happens when service is down
                    log.CreateLogEntry(string.Format("PriorAuthService: {0} {1} {2}", logHeader, ex.ToString(), ex.Response != null ? ex.Response.ToString() : ex.ToString()), Logging.LogPriority.Error);
                    InfoAccessController.UpdatePriorAuthTransactionQueueSIResponse(tranasctionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                    return CON.TransactionResult.TransactionFailed;
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuthService: {0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdatePriorAuthTransactionQueueSIResponse(tranasctionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                return CON.TransactionResult.TransactionFailed;
            }
        }


        public string getXMLPA(string resp)
        {
            System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(resp);
            if (xDoc.Declaration != null)
            {
                if (xDoc.Declaration.Encoding != null)
                {
                    xDoc.Declaration.Encoding = "utf-16";
                }
            }

            return xDoc.ToString();
        }

        public void processResponse(WebException exResp, string serviceName, int transactionID)
        {
            var resp = (HttpWebResponse)exResp.Response;

            switch (((HttpWebResponse)exResp.Response).StatusCode)
            {
                case (HttpStatusCode.BadRequest): // 404
                    processBadRequest(exResp, serviceName, transactionID);
                    break;

                case HttpStatusCode.InternalServerError: // 503
                    processInternalServerError(exResp, transactionID);
                    break;
            }
        }

        public void processBadRequest(WebException exResp, string serviceName, int transactionID)
        {
            string logHeader = string.Format("PriorAuthService: Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string ModuleTransactionId = string.Empty;
            string SITransactionKey = string.Empty;
            string ResponseCode = string.Empty;
            string ResponseDetails = string.Empty;
            string ResponseMessage = string.Empty;
            string ResponseType = string.Empty;
            log = new Logging(this.ThreadId, logMsg);

            string respBody = new StreamReader(exResp.Response.GetResponseStream()).ReadToEnd();
            try
            {
                if (serviceName.Equals(Constants.ProviderManagementServiceResponse.enrollProvider))
                {
                    DeserializeSoapPriorAuthResponse(respBody, serviceName, out ModuleTransactionId, out ResponseCode, out ResponseType, out ResponseDetails, out ResponseMessage, out SITransactionKey);

                }
                else
                {
                    DeserializeSoapPriorAuthResponse(respBody, serviceName, out ModuleTransactionId, out ResponseCode, out ResponseType, out ResponseDetails, out ResponseMessage, out SITransactionKey);

                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuthService: {0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveSoapPriorAuthResponseCodeException(transactionID, SITransactionKey, ResponseCode,
                        ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId), getXMLPA(respBody.ToString()));
        }

        public void processInternalServerError(WebException exResp, int transactionID)
        {
            string logHeader = string.Format("PriorAuthService: Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string fautString = string.Empty;
            string faultCode = string.Empty;
            object result;
            var respBody = new StreamReader(exResp.Response.GetResponseStream()).ReadToEnd();
            try
            {
                var xDoc = XDocument.Parse(respBody);
                var xFaultResponse = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("Fault"));
                var serializer = new XmlSerializer(typeof(FaultResponse));
                using (var reader = new StringReader(xFaultResponse.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                FaultResponse obj = (FaultResponse)result;
                fautString = obj.faultstring;
                faultCode = obj.faultcode;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuthService: Deserilization of SOAP Fault Response Failed {0}, {1}", ex.ToString(), respBody), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveFaultCodePriorAuthException(fautString, faultCode, transactionID, DateTime.Now, new Guid(Constants.appAdminUserId),
                getXMLPA(respBody.ToString()));
        }

        public void DeserializeSoapPriorAuthResponse(string respBody, string serviceName, out string ModuleTransactionId, out string ResponseCode, out string ResponseType,
            out string ResponseDetails, out string ResponseMessage, out string SITransactionKey)
        {
            object result;
            ModuleTransactionId = string.Empty;
            SITransactionKey = string.Empty;
            ResponseCode = string.Empty;
            ResponseDetails = string.Empty;
            ResponseMessage = string.Empty;
            ResponseType = string.Empty;

            try
            {
                if (serviceName.Equals(CON.PriorAuthServiceResponse.SearchPriorAuth))
                {
                    var xDocResp = XDocument.Parse(respBody);
                    var serializerResp = new XmlSerializer(typeof(EnvelopeSearchResponse));
                    EnvelopeSearchResponse sam = (EnvelopeSearchResponse)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));
                    ModuleTransactionId = sam.Body.SearchPriorAuthResponse.ResponseHeader.ModuleTransactionId;
                    SITransactionKey = sam.Body.SearchPriorAuthResponse.ResponseHeader.SITransactionKey;
                    ResponseCode = sam.Body.SearchPriorAuthResponse.ResponseHeader.ResponseCode;
                    ResponseDetails = sam.Body.SearchPriorAuthResponse.ResponseHeader.ResponseDetails;
                    ResponseMessage = sam.Body.SearchPriorAuthResponse.ResponseHeader.ResponseMessage;
                    ResponseType = sam.Body.SearchPriorAuthResponse.ResponseHeader.ResponseType.ToString();
                    //authRespone = sam.Body.SearchPriorAuthResponse.ResponsePayload;
                }
                else if (serviceName.Equals(CON.PriorAuthServiceResponse.AddUpdatePriorAuth))
                {
                    var xDocResp = XDocument.Parse(respBody);
                    var serializerResp = new XmlSerializer(typeof(EnvelopeAddUpdateResponse));
                    EnvelopeAddUpdateResponse sam = (EnvelopeAddUpdateResponse)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));
                    ModuleTransactionId = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.ModuleTransactionId;
                    SITransactionKey = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.SITransactionKey;
                    ResponseCode = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.ResponseCode;
                    ResponseDetails = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.ResponseDetails;
                    ResponseMessage = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.ResponseMessage;
                    ResponseType = sam.Body.AddUpdatePriorAuthResponse.ResponseHeader.ResponseType.ToString();
                }
                else
                {
                    var xDocResp = XDocument.Parse(respBody);
                    var serializerResp = new XmlSerializer(typeof(EnvelopeInqResponse));
                    EnvelopeInqResponse sam = (EnvelopeInqResponse)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));
                    ModuleTransactionId = sam.Body.InquirePriorAuthResponse.ResponseHeader.ModuleTransactionId;
                    SITransactionKey = sam.Body.InquirePriorAuthResponse.ResponseHeader.SITransactionKey;
                    ResponseCode = sam.Body.InquirePriorAuthResponse.ResponseHeader.ResponseCode;
                    ResponseDetails = sam.Body.InquirePriorAuthResponse.ResponseHeader.ResponseDetails;
                    ResponseMessage = sam.Body.InquirePriorAuthResponse.ResponseHeader.ResponseMessage;
                    ResponseType = sam.Body.InquirePriorAuthResponse.ResponseHeader.ResponseType.ToString();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuthService: Deserilization of SOAP Response Failed {0}, {1}, {2}", ex.ToString(), serviceName, respBody), Logging.LogPriority.Error);
            }
        }


        public string wssSecurityHeader(string priorAuthWSUserName, string priorAuthWSPassword)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(priorAuthWSUserName, priorAuthWSPassword, PasswordOption.SendPlainText);
                string xml = "<soapenv:Header>" +
                    @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                        usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                    "</wsse:Security>" +
                  "</soapenv:Header>";
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("PriorAuthService: wss header failure {0}", ex.ToString()), Logging.LogPriority.Error);
                return null;
            }
        }

    }
}
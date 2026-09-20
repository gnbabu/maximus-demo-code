using Corp.Core.Libraries.AcknowledgmentService;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class FinancialServiceReqRes
    {
        private Guid m_threadId;
        private Logging log = null;
        private static string financialServiceWSURL = AppSettings.Get("FinancialServiceWSURL", Constants.WebServiceURI.FinancialService);
        private static string financialServiceWSUserName = AppSettings.Get("FinancialServiceWSUserName");

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

        public FinancialServiceReqRes()
        {
            ThreadId = Guid.NewGuid();
        }

        public DataSet Get1099SearchHistory(string providerId, string taxID, string taxIDType, string reqYear, string NPIOrMedicaidId)
        {
            string logHeader = string.Format("Financial Service log for providerID- {0}:", providerId.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            log = new Logging(this.ThreadId, logMsg);
            try
            {

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                FinancialServices.SubMessageHeaderSubscriber[] fsSub = new FinancialServices.SubMessageHeaderSubscriber[1];
                fsSub[0] = FinancialServices.SubMessageHeaderSubscriber.FI;

                FinancialServices.MessageHeader msgHeader = new FinancialServices.MessageHeader();
                msgHeader.BusinessFlow = FinancialServices.InqMessageHeaderBusinessFlow.inquire1099;
                msgHeader.StateCode = "OH";
                msgHeader.RequestorSystem = FinancialServices.InqMessageHeaderRequestorSystem.PNM;
                msgHeader.ModuleTransactionId = "";
                msgHeader.SubscriberSystem = fsSub;
                msgHeader.AdditionalModuleTransactionId = string.Empty;
                msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                msgHeader.SITransactionKey = pnmTransactionKey;

                log.CreateLogEntry(string.Format("{0} - 1099History: Built msgHeader {1}", System.DateTime.Now, msgHeader.ToString()), Logging.LogPriority.Information);

                FinancialServices.Search1099 req = new FinancialServices.Search1099();
                req.ProviderId = NPIOrMedicaidId;
                req.TaxId = taxID;
                req.TaxIdType = taxIDType;
                req.ReqYear = reqYear;
                FinancialServices.inquire1099Request req1099 = new FinancialServices.inquire1099Request();
                req1099.MessageHeader = msgHeader;
                req1099.Search1099 = req;

                log.CreateLogEntry(string.Format("{0} - 1099History: Built req1099 {1}", System.DateTime.Now, req1099.ToString()), Logging.LogPriority.Information);

                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("fin", "http://mes.gov/financialservice");
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(FinancialServices.inquire1099Request));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req1099, ns);
                string xml = stream2.ToString();
                log.CreateLogEntry(string.Format("{0} - 1099History: Built xml {1}", System.DateTime.Now, xml), Logging.LogPriority.Information);
                // JLB - TODO - Determine why WrapperName is not applying
                xml = xml.Replace("<inquire1099Request", "<Inquire1099").Replace("</inquire1099Request", "</Inquire1099");
                log.CreateLogEntry(string.Format("{0} - 1099History: After xml cleanup {1}", System.DateTime.Now, xml), Logging.LogPriority.Information);
                string client_secret = "";
                string secretName = "FinancialServiceWSPassword_OH_PNM_";
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

                log.CreateLogEntry(string.Format("{0} - 1099History: After getting AWS secret key {1}", System.DateTime.Now, "Got the key!"), Logging.LogPriority.Information);

                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:fin=\"http://mes.gov/financialservice\">"
                    + Environment.NewLine + wssSecurityHeader(financialServiceWSUserName, client_secret) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();

                log.CreateLogEntry(string.Format("{0} - 1099History: After adding soap env and body to xml {1}", System.DateTime.Now, xml), Logging.LogPriority.Information);

                xmlDoc.LoadXml(xml);
                var imsXml = xmlDoc.InnerXml.ToString().Replace("Inquire1099 xmlns:fin=\"http://mes.gov/financialservice\">", "Inquire1099>").Replace("Inquire1099>", "fin:Inquire1099>").Replace("MessageHeader>", "fin:MessageHeader>").Replace("Search1099>", "fin:Search1099>");
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(imsXml);

                log.CreateLogEntry(string.Format("{0} - 1099History: After adding namespaces to xml and before saving to Outbound table {1}", System.DateTime.Now, imsXml), Logging.LogPriority.Information);

                InfoAccessController.SaveFinancialServiceXMLRecord(providerId, pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, xmlDoc.InnerXml.ToString(), Methods.GetCurrentUserId());

                log.CreateLogEntry(string.Format("{0} - 1099History: After saving to Outbound table: {1}, {2}, {3}, {4}", System.DateTime.Now, providerId, pnmTransactionKey,
                    Constants.ProviderFinancial.Inquire1099, xmlDoc.InnerXml.ToString()), Logging.LogPriority.Information);

                return make1099WSRequestResponse(providerId, xmlDoc, Constants.FinancialServiceSoapAction.Inquire1099, pnmTransactionKey);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode0, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, Methods.GetCurrentUserId());
                return null;
            }
        }

        public DataSet make1099WSRequestResponse(string providerId, XmlDocument xmlDoc, string soapAction, string pnmTransactionKey)
        {
            string logHeader = string.Format("Financial Service Get 1099 History for medicadid as  Provider ID - " + providerId);
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            DataSet ds = new DataSet();
            try
            {
                log.CreateLogEntry(string.Format("{0} - 1099History: In webservice call {1}", System.DateTime.Now, xmlDoc.ToString()), Logging.LogPriority.Information);

                string financialServiceWSCertificateName = AppSettings.Get("FinancialServiceWSCertificateName");
                string WebServiceHost = AppSettings.Get("SIWebServiceHost");
                string fsWebAPITesting = AppSettings.Get("FinancialServiceWebAPITesting", "false");
                int fsTimeOut = Convert.ToInt32(AppSettings.Get("FSResponseTimeOut", "2000"));
                int fsRWTimeOut = Convert.ToInt32(AppSettings.Get("FSRWResponseTimeOut", "2000"));
                ServicePointManager.Expect100Continue = true;

                log.CreateLogEntry(string.Format("{0} - 1099History: Before getting Certs {1}", System.DateTime.Now, financialServiceWSCertificateName), Logging.LogPriority.Information);

                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(financialServiceWSCertificateName);
                string user = financialServiceWSUserName;
                string client_secret = "";
                string secretName = "FinancialServiceWSPassword_OH_PNM_";
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

                log.CreateLogEntry(string.Format("{0} - 1099History: After getting AWS keys {1}", System.DateTime.Now, secretName), Logging.LogPriority.Information);

                string passcode = client_secret;
                Uri apiUrl = new Uri(financialServiceWSURL);
                WebRequest pmRequest = HttpWebRequest.Create(financialServiceWSURL);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = WebServiceHost;
                pmHttpRequest.Timeout = fsTimeOut;
                pmHttpRequest.ReadWriteTimeout = fsRWTimeOut;

                if (clientCertificate != null)
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);

                log.CreateLogEntry(string.Format("{0} - 1099History: After adding cert {1}", System.DateTime.Now, financialServiceWSURL), Logging.LogPriority.Information);

                var response = "";
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                if (fsWebAPITesting.Equals("false"))
                {
                    Stream requestStream = pmHttpRequest.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();

                    log.CreateLogEntry(string.Format("{0} - 1099History: Before calling service {1}", System.DateTime.Now, requestStream.ToString()), Logging.LogPriority.Information);

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

                    log.CreateLogEntry(string.Format("{0} - 1099History: After calling service {1}", System.DateTime.Now, response), Logging.LogPriority.Information);
                }
                else
                {
                    DataSet dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("FinancialService1099WebAPI");
                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }

                //Save Raw Response
                InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, response, Methods.GetCurrentUserId());

                try
                {
                    string respxml = string.Empty;
                    if (Methods.IsValidXml(response))
                    {
                        var xDoc = XDocument.Parse(response);
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Ignore;
                        settings.XmlResolver = null;
                        XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);
                        ds.ReadXml(xmlReader);
                        respxml = ds.GetXml();

                        log.CreateLogEntry(string.Format("{0} - 1099History: Response from Service before Deserialize {1}", System.DateTime.Now, respxml), Logging.LogPriority.Information);

                        DeserializeSoap1099InquireResponse(respxml, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType);
                    }
                    else
                    {
                        respxml = response.ToString();
                    }

                    log.CreateLogEntry(string.Format("{0} - 1099History: Response from Service after Deserialize {1}", System.DateTime.Now, respxml), Logging.LogPriority.Information);


                    InfoAccessController.SaveFinancialServiceReqRes(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, SITransactionKey, ResponseCode, ResponseType,
                        ResponseMessage, xmlDoc.InnerXml.ToString(), respxml.ToString(), Methods.GetCurrentUserId());
                    return ds;
                }
                catch (Exception ex)
                {
                    InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, "Failed at Response Catch", Methods.GetCurrentUserId());

                    InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode1, Constants.ProviderFinancialErrors.PFFailureType, ex.StackTrace.ToString(), DateTime.Now, Methods.GetCurrentUserId());
                    return null;
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
                        string respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();//Save Raw Response
                        InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, respBody, Methods.GetCurrentUserId());

                        processResponse(respBody, ex, Constants.ProviderFinancial.Inquire1099, providerId, pnmTransactionKey);
                        InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode2, Constants.ProviderFinancialErrors.PFFailureType, respBody.ToString(), DateTime.Now, Methods.GetCurrentUserId());
                        return null;
                    }
                    else
                    {
                        InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, response.ToString(), Methods.GetCurrentUserId());

                        // no http status code available
                        InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode3, Constants.ProviderFinancialErrors.PFFailureType, response.ToString(), DateTime.Now, Methods.GetCurrentUserId());
                        return null;
                    }
                }
                else
                {
                    InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, "Failed at No HTTP Code", Methods.GetCurrentUserId());

                    // no http status code available
                    InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode4, Constants.ProviderFinancialErrors.PFFailureType, ex.Response.ToString(), DateTime.Now, Methods.GetCurrentUserId());
                    return null;
                }
            }
            catch (Exception ex)
            {
                InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.Inquire1099, "Failed at final Catch", Methods.GetCurrentUserId());

                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateFinancialServiceSIResponse(providerId, pnmTransactionKey, null, Constants.ProviderFinancial.Inquire1099, Constants.ProviderFinancialErrors.PF1099ErrorCode5, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, Methods.GetCurrentUserId());
                return null;
            }
        }

        public DataSet GetTransactionHistory(string medicaidID, string NPIOrMedicaidId)
        {
            string logHeader = string.Format("Financial Service log for providerID- {0}:", NPIOrMedicaidId.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string pnmTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                FinancialServices.SubMessageHeaderSubscriber[] fsSub = new FinancialServices.SubMessageHeaderSubscriber[1];
                fsSub[0] = FinancialServices.SubMessageHeaderSubscriber.FI;

                FinancialServices.MessageHeader msgHeader = new FinancialServices.MessageHeader();
                msgHeader.BusinessFlow = FinancialServices.InqMessageHeaderBusinessFlow.inquireTransactionHistory;
                msgHeader.StateCode = "OH";
                msgHeader.RequestorSystem = FinancialServices.InqMessageHeaderRequestorSystem.PNM;
                msgHeader.ModuleTransactionId = "";
                msgHeader.SubscriberSystem = fsSub;
                msgHeader.AdditionalModuleTransactionId = string.Empty;
                msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                msgHeader.SITransactionKey = pnmTransactionKey;

                FinancialServices.SearchTransactionHistory req = new FinancialServices.SearchTransactionHistory();
                req.ProviderId = NPIOrMedicaidId;

                FinancialServices.inquireTransactionHistoryRequest tranReq = new FinancialServices.inquireTransactionHistoryRequest();
                tranReq.MessageHeader = msgHeader;
                tranReq.SearchTransactionHistory = req;

                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("fin", "http://mes.gov/financialservice");
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(FinancialServices.inquireTransactionHistoryRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, tranReq, ns);
                string xml = stream2.ToString();

                string client_secret = "";
                string secretName = "FinancialServiceWSPassword_OH_PNM_";
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
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:fin=\"http://mes.gov/financialservice\">"
                    + Environment.NewLine + wssSecurityHeader(financialServiceWSUserName, client_secret) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                // TODO OHPNM-18807 determine why reference.cs is adding incorrect header record (inquireTransactionHistoryRequest instead of fin:InquireTransactionHistory)
                var imsXml = xmlDoc.InnerXml.ToString().Replace("inquireTransactionHistoryRequest xmlns:fin=\"http://mes.gov/financialservice\">", "fin:InquireTransactionHistory>").Replace("inquireTransactionHistoryRequest>", "fin:InquireTransactionHistory>").Replace("InquireTransactionHistoryRequest>", "fin:InquireTransactionHistory>").Replace("MessageHeader>", "fin:MessageHeader>").Replace("SearchTransactionHistory>", "fin:SearchTransactionHistory>");
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(imsXml);

                InfoAccessController.SaveFinancialServiceXMLRecord(NPIOrMedicaidId, pnmTransactionKey, Constants.ProviderFinancial.History1099, xmlDoc.InnerXml.ToString(), new Guid(Constants.providerFinancialId));

                return makeTransHistWSRequestResponse(medicaidID, xmlDoc, Constants.FinancialServiceSoapAction.InquireTransactionHistory, pnmTransactionKey);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateFinancialServiceSIResponse(NPIOrMedicaidId, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode6, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, new Guid(Constants.providerFinancialId));
                return null;
            }
        }

        public DataSet makeTransHistWSRequestResponse(string medID, XmlDocument xmlDoc, string soapAction, string pnmTransactionKey)
        {
            string logHeader = string.Format("Financial Service Get Transaction History for Medicaid ID - " + medID);
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            DataSet ds = new DataSet();
            try
            {
                string financialServiceWSCertificateName = AppSettings.Get("FinancialServiceWSCertificateName");
                string WebServiceHost = AppSettings.Get("SIWebServiceHost");
                string fsWebAPITesting = AppSettings.Get("FinancialServiceWebAPITesting", "false");
                int fsTimeOut = Convert.ToInt32(AppSettings.Get("FSResponseTimeOut", "2000"));
                int fsRWTimeOut = Convert.ToInt32(AppSettings.Get("FSRWResponseTimeOut", "2000"));

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(financialServiceWSCertificateName);
                string user = financialServiceWSUserName;
                string client_secret = "";
                string secretName = "FinancialServiceWSPassword_OH_PNM_";
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
                string passcode = client_secret;
                Uri apiUrl = new Uri(financialServiceWSURL);
                WebRequest pmRequest = HttpWebRequest.Create(financialServiceWSURL);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = WebServiceHost;
                pmHttpRequest.Timeout = fsTimeOut;
                pmHttpRequest.ReadWriteTimeout = fsRWTimeOut;

                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);

                var response = "";
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                if (fsWebAPITesting.Equals("false"))
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
                    DataSet dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("FinancialServiceInquiryWebAPI");
                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }

                //Save Raw Response
                InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, response, Methods.GetCurrentUserId());

                try
                {
                    string respxml = string.Empty;
                    if (Methods.IsValidXml(response))
                    {
                        var xDoc = XDocument.Parse(response);
                        XmlReaderSettings settings = new XmlReaderSettings();
                        settings.DtdProcessing = DtdProcessing.Ignore;
                        settings.XmlResolver = null;
                        XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);
                        ds.ReadXml(xmlReader);
                        respxml = ds.GetXml();
                        DeserializeSoap1099HistoryResponse(respxml, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType);
                    }
                    else
                    {
                        respxml = response.ToString();
                    }
                    InfoAccessController.SaveFinancialServiceReqRes(pnmTransactionKey, Constants.ProviderFinancial.History1099, SITransactionKey, ResponseCode, ResponseType,
                        ResponseMessage, xmlDoc.InnerXml.ToString(), respxml.ToString(), new Guid(Constants.providerFinancialId));
                    return ds;
                }
                catch (Exception ex)
                {
                    InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, "Failed at Response Catch", Methods.GetCurrentUserId());

                    InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode7, Constants.ProviderFinancialErrors.PFFailureType, ex.StackTrace.ToString(), DateTime.Now, new Guid(Constants.providerFinancialId));
                    return null;
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
                        string respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                        InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, respBody, Methods.GetCurrentUserId());

                        processResponse(respBody, ex, Constants.ProviderFinancial.History1099, medID, pnmTransactionKey);
                        InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode8, Constants.ProviderFinancialErrors.PFFailureType, respBody.ToString(), DateTime.Now, new Guid(Constants.providerFinancialId));
                        return null;
                    }
                    else
                    {
                        InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, response.ToString(), Methods.GetCurrentUserId());

                        // no http status code available
                        InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode9, Constants.ProviderFinancialErrors.PFFailureType, response.ToString(), DateTime.Now, new Guid(Constants.providerFinancialId));
                        return null;
                    }
                }
                else
                {
                    InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, "Failed at No HTTP Code", Methods.GetCurrentUserId());

                    // no http status code available
                    InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode10, Constants.ProviderFinancialErrors.PFFailureType, ex.Response.ToString(), DateTime.Now, new Guid(Constants.providerFinancialId));
                    return null;
                }
            }
            catch (Exception ex)
            {
                InfoAccessController.SaveFinancialServiceRawResponse(pnmTransactionKey, Constants.ProviderFinancial.History1099, "Failed at Final Catch", Methods.GetCurrentUserId());

                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode11, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, new Guid(Constants.providerFinancialId));
                return null;
            }
        }


        public void processInternalServerError(WebException exResp, string medID, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", medID);
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
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, Constants.ProviderFinancial.History1099, Constants.ProviderFinancialErrors.PF1099ErrorCode12, Constants.ProviderFinancialErrors.PFFailureType, respBody.ToString(), DateTime.Now, new Guid(Constants.providerFinancialId));
        }

        public void processResponse(string exRespStr, WebException exResp, string serviceName, string medID, string pnmTransactionKey)
        {
            switch (((HttpWebResponse)exResp.Response).StatusCode)
            {
                case (HttpStatusCode.BadRequest): // 404
                    InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, serviceName, Constants.ProviderFinancialErrors.PF1099ErrorCode13, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, new Guid(Constants.providerFinancialId));
                    processBadRequest(exRespStr, serviceName, medID, pnmTransactionKey);
                    break;

                case HttpStatusCode.InternalServerError: // 503
                    InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, null, serviceName, Constants.ProviderFinancialErrors.PF1099ErrorCode14, Constants.ProviderFinancialErrors.PFFailureType, Constants.ProviderFinancialErrors.PF1099Message, DateTime.Now, new Guid(Constants.providerFinancialId));
                    processInternalServerError(exResp, medID, pnmTransactionKey);
                    break;
            }
        }

        public void processBadRequest(string exRespStr, string serviceName, string medID, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", medID);
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string SITransactionKey = string.Empty;
            string ResponseCode = string.Empty;
            string ResponseDetails = string.Empty;
            string ResponseMessage = string.Empty;
            string ResponseType = string.Empty;
            log = new Logging(this.ThreadId, logMsg);

            string respBody = exRespStr;
            try
            {
                if (serviceName.Equals(Constants.ProviderFinancial.Inquire1099))
                {
                    DeserializeSoap1099InquireResponse(respBody, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType);
                }
                else
                {
                    DeserializeSoap1099HistoryResponse(respBody, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType);
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.UpdateFinancialServiceSIResponse(medID, pnmTransactionKey, SITransactionKey, serviceName, ResponseCode, ResponseType, ResponseMessage, DateTime.Now, new Guid(Constants.providerFinancialId));
        }

        public void DeserializeSoap1099InquireResponse(string respBody, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
    out string ResponseMessage, out string ResponseType)
        {
            object result;
            SITransactionKey = string.Empty;
            ResponseCode = string.Empty;
            ResponseDetails = string.Empty;
            ResponseMessage = string.Empty;
            ResponseType = string.Empty;

            try
            {
                var xDoc = XDocument.Parse(respBody);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("Inquire1099Response"));

                var serializer = new XmlSerializer(typeof(SoapProviderFinancialInquireResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                SoapProviderFinancialInquireResponse obj = (SoapProviderFinancialInquireResponse)result;
                SITransactionKey = obj.SITransactionKey;
                ResponseCode = obj.ResponseCode;
                ResponseDetails = obj.ResponseDetails;
                ResponseMessage = obj.ResponseMessage;
                ResponseType = obj.ResponseType;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }

        public void DeserializeSoap1099HistoryResponse(string respBody, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
    out string ResponseMessage, out string ResponseType)
        {
            object result;
            SITransactionKey = string.Empty;
            ResponseCode = string.Empty;
            ResponseDetails = string.Empty;
            ResponseMessage = string.Empty;
            ResponseType = string.Empty;

            try
            {
                var xDoc = XDocument.Parse(respBody);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals("InquireTransactionHistoryResponse"));

                var serializer = new XmlSerializer(typeof(SoapProviderFinancialHistoryResponse));
                if (xLoginResult != null)
                {
                    using (var reader = new StringReader(xLoginResult.ToString()))
                    {
                        result = serializer.Deserialize(reader);
                    }
                    SoapProviderFinancialHistoryResponse obj = (SoapProviderFinancialHistoryResponse)result;
                    SITransactionKey = obj.SITransactionKey;
                    ResponseCode = obj.ResponseCode;
                    ResponseDetails = obj.ResponseDetails;
                    ResponseMessage = obj.ResponseMessage;
                    ResponseType = obj.ResponseType;
                }
                else
                {
                    ResponseDetails = "Failure";
                    ResponseMessage = "Could not parse return payload";
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }

        public void processInternalServerError(WebException exResp, int transactionID, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
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
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveFaultCodeException(fautString, faultCode, transactionID, DateTime.Now, new Guid(Constants.appAdminUserId),
                respBody.ToString(), pnmTransactionKey);
        }



        public string wssSecurityHeader(string userName, string password)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(userName, password, PasswordOption.SendPlainText);
                string xml = "<soapenv:Header>" +
                    @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                        usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                    "</wsse:Security>" +
                  "</soapenv:Header>";
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
                return null;
            }
        }

    }
}
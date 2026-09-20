using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class IncidentManagementReqRes
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
        public IncidentManagementReqRes()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;
        private static string IncidentManagementServiceReferenceWSURL = AppSettings.Get("IncidentManagementServiceWSURL", Constants.WebServiceURI.IncidentManagementService);
        private static string IncidentManagementServiceReferenceWSUserName = AppSettings.Get("IncidentManagementServiceWSUserName");

        public string UpdateIncidentProviderStatus(string medicaidID, string IMSCaseNumber, int reg_id, string requestFor, IncidentManagementService.UpdateProviderIncidentStatusInfo upReqInfo)
        {
            bool makeRequest = false;
            string strCheck = AppSettings.Get("MakeWSRequestCallToSI", "false");
            if (strCheck.Equals("true"))
            {
                makeRequest = true;
            }
            if (makeRequest)
            {
                string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

                IncidentManagementService.MessageHeaderSubscriber[] imSub = new IncidentManagementService.MessageHeaderSubscriber[1];
                imSub[0] = IncidentManagementService.MessageHeaderSubscriber.IMS;

                IncidentManagementService.MessageHeader msg = new IncidentManagementService.MessageHeader();
                msg.BusinessFlow = IncidentManagementService.MessageHeaderBusinessFlow.UpdateProviderIncidentStatus;
                //msg.BusinessFlowSpecified = true;
                msg.RequestorSystem = IncidentManagementService.MessageHeaderRequestorSystem.PNM;
                msg.StateCode = "OH";
                msg.SubscriberSystem = imSub;
                msg.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                msg.SITransactionKey = sitTransactionKey;

                //IncidentManagementService.UpdateProviderIncidentStatusInfo upReqInfo = new IncidentManagementService.UpdateProviderIncidentStatusInfo();
                //upReqInfo.MedicaidProviderID = requestFor == "IssueNOD" ? string.Empty : medicaidID;           
                //upReqInfo.ProviderName = name;
                //upReqInfo.ProviderStatus = status;
                //upReqInfo.PAOMailedDate = PAOMailedDate;
                //upReqInfo.PAIUnclaimed = PAIUncliamed;
                //upReqInfo.HearingRequestedDate = HearingReqDate;
                //upReqInfo.HearingRequestDueDate = HearingReqDueDate;
                //upReqInfo.SettlementReachedDate = SettlementReachedDate;
                //upReqInfo.AODateIssued = AODateIssued;
                //upReqInfo.IMSAssociateID = IMSAssociateID;
                //upReqInfo.IMSCaseNumber = IMSCaseNumber;
                //upReqInfo.PNMNODID = PNMNODID;
                //upReqInfo.ActionByDepartment = requestFor == "IssueNOD" ? "Yes" : "No";
                //upReqInfo.DateNODIssued = NODIssuedDate;
                //upReqInfo.PlanOfCorrectionDueDate = POCDuedate;
                //upReqInfo.PlanOfCorrectionAcceptedDate = POCAcceptedDate;
                //upReqInfo.PlanOfCorrectionReceivedDate = POCReceivedDate;
                //upReqInfo.RevisedPlanOfCorrectionDate = RevisedPOCdate;
                //upReqInfo.RevisedPlanOfCorrectionDueDate = RevisedPOCduedate;

                IncidentManagementService.UpdateProviderIncidentStatusRequestPayload upReq = new IncidentManagementService.UpdateProviderIncidentStatusRequestPayload();
                upReq.UpdateProviderIncidentStatusInfo = upReqInfo;


                IncidentManagementService.updateProviderIncidentStatusRequest req = new IncidentManagementService.updateProviderIncidentStatusRequest();
                req.MessageHeader = msg;
                req.Payload = upReq;

                System.Xml.Serialization.XmlSerializerNamespaces ns = new System.Xml.Serialization.XmlSerializerNamespaces();
                ns.Add("inc", "http://mes.gov/incidentmanagement");
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.IncidentManagementService.updateProviderIncidentStatusRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, ns);
                string xml = stream2.ToString();
                string client_secret = "";
                string secretName = "IncidentManagementServiceWSPassword_OH_PNM_";
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

                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">"
                    + Environment.NewLine + wssSecurityHeader(IncidentManagementServiceReferenceWSUserName, client_secret) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                xmlDoc.DocumentElement.SetAttribute("xmlns:inc", "http://mes.gov/incidentmanagement");
                var imsXml = xmlDoc.InnerXml.ToString().Replace("UpdateProviderIncidentStatusRequest xmlns:inc=\"http://mes.gov/incidentmanagement\">", "UpdateProviderIncidentStatusRequest>").Replace("UpdateProviderIncidentStatusRequest>", "inc:UpdateProviderIncidentStatusRequest>").Replace("MessageHeader>", "inc:MessageHeader>").Replace("Payload>", "inc:Payload>");
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(imsXml);

                int rowID =  InfoAccessController.SaveIMSReqRes(reg_id, IMSCaseNumber, requestFor, xmlDoc.InnerXml.ToString(), Constants.appAdminUserId);

                return makeWSRequestResponse(medicaidID, xmlDoc, Constants.IMSSoapAction.IncidentManagementService, sitTransactionKey, rowID);
                
            }
            else
               return "Success";

        }

        public string makeWSRequestResponse(string medID, XmlDocument xmlDoc, string soapAction, string pnmTransactionKey, int rowID)
        {
            string rtn = string.Empty;
            string logHeader = string.Format("Incident Management Service Update Provider Status for Medicaid ID - " + medID);
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            DataSet ds = new DataSet();
            try
            {
                string incidentServiceWSCertificateName = AppSettings.Get("IncidentManagementWSCertificateName");
                string WebServiceHost = AppSettings.Get("SIWebServiceHost");
                string ModuleTransactionId = string.Empty;
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(incidentServiceWSCertificateName);
                string user = IncidentManagementServiceReferenceWSUserName;
                string client_secret = "";
                string secretName = "IncidentManagementServiceWSPassword_2_OH_PNM_";
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
                Uri apiUrl = new Uri(IncidentManagementServiceReferenceWSURL);
                WebRequest pmRequest = HttpWebRequest.Create(IncidentManagementServiceReferenceWSURL);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = WebServiceHost;
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
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
                InfoAccessController.UpdateIMSReqRes(rowID,response.ToString(), Constants.appAdminUserId);
                try
                {
                    XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
                    xmlReaderSettings.DtdProcessing = DtdProcessing.Ignore;                   
                    xmlReaderSettings.XmlResolver = null;
                    XmlReader xmlReader = XmlReader.Create(new StringReader(response.ToString()),xmlReaderSettings);
                    ds.ReadXml(xmlReader);

                    DataTable dt = ds.Tables["UpdateProviderIncidentStatusResponse"];
                    DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    if(dr != null)
                    {
                        rtn = Methods.GetStringValue(dr, "ResponseType").ToLower() == "success" ? "Success" : "Fail";
                    }

                }
                catch (Exception ex)
                {
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                    return "Fail";     
                }
               
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        var respBody = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        //http status code avaliable
                        InfoAccessController.UpdateIMSReqRes(rowID, respBody.ToString(), Constants.appAdminUserId);
                        //log is temporary
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        return "Fail";
                    }
                    else
                    {
                        // no http status code available
                        //log is temporary
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        return "Fail";
                    }
                }
                else
                {
                    // no http status code available
                    //log is temporary
                    log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                    return "Fail";
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                return "Fail";
            }
            return rtn;
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

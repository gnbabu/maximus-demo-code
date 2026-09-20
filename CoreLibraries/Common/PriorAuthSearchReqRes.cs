using Corp.Core.Libraries.CareManagement;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    public class PriorAuthSearchReqRes
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
        public PriorAuthSearchReqRes()
        {
            ThreadId = Guid.NewGuid();
        }
        private Logging log = null;
        private static string PriorAuthSearchWSURL = AppSettings.Get("PriorAuthSearchWSURL", Constants.WebServiceURI.CareManagementService);
        private static string PriorAuthSearchWSURLWSUserName = AppSettings.Get("PriorAuthSearchWSUserName");
        private static string logHeader = string.Format("PriorAuthSearch Get Transaction History for Medicaid ID - ");
        public DataSet SearchAuthRequest(SearchPrioAuthModel searchModel)
        {
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            var requestObj = new searchAuthorizationRequest
            {
                MessageHeader = new MessageHeader
                {
                    AdditionalModuleTransactionId = string.Empty,
                    BusinessFlow = MessageHeaderBusinessFlow.SearchAuthorization,
                    ModuleTransactionId = "",
                    RequestorSystem = MessageHeaderRequestorSystem.PNM,
                    RequestTimestamp = GetStringDateTime(DateTime.Now.ToString()),
                    SITransactionKey = sitTransactionKey,
                    StateCode = MessageHeaderStateCode.OH,
                    SubscriberSystem = MessageHeaderSubscriberSystem.MITS,
                    SubscriberSystemSpecified = false
                },
                SearchAuthorizationPayload = new SearchAuthorizationPayload
                {
                    AssignmentCode = searchModel.AssignmentCode,
                    AuthSubmissionDate = searchModel.AuthSubmissionDate,
                    Diagnosis = searchModel.Diagnosis,
                    InpatientProcedure = searchModel.InpatientProcedure,
                    InputLimit = searchModel.InputLimit,
                    InputOffset = searchModel.InputOffset,
                    MemberDOB = searchModel.MemberDOB,
                    MemberID = searchModel.MemberID,
                    OrderingProviderID = searchModel.OrderingProviderID,
                    PriorAuthorizationID = searchModel.PriorAuthorizationID,
                    PriorAuthorizationTypeCode = searchModel.PriorAuthorizationTypeCode,
                    ProcedureCode = searchModel.ProcedureCode,
                    ProviderID = searchModel.ProviderId,
                    RecordStatusCode = searchModel.RecordStatusCode,
                    RevenueCode = searchModel.RevenueCode,
                    StatusCode = searchModel.StatusCode
                }
            };
            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(searchAuthorizationRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream = new StringWriter();
            var writer = XmlWriter.Create(stream, settings);
            x.Serialize(writer, requestObj, emptyNs);
            string xml = stream.ToString();
            string client_secret = "";
            string secretName = "PriorAuthSearchPassword_OH_PNM_";
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
                Logging log = new Logging(Guid.NewGuid(), "");
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
            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/membermanagement\">"
                + Environment.NewLine + wssSecurityHeader(PriorAuthSearchWSURLWSUserName, client_secret) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            log.CreateLogEntry(string.Format("{0} {1}", logHeader, xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
            return SearchAuthWSRequestResponse(xmlDoc, Constants.PriorAuthSearchSoapAction.CareManagementResponse, sitTransactionKey);
        }
        public DataSet SearchAuthWSRequestResponse(XmlDocument xmlDoc, string soapAction, string pnmTransactionKey)
        {

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            DataSet ds = new DataSet();
            string priorAuthSearchServiceWSCertificateName = AppSettings.Get("PriorAuthSearchServiceWSCertificateName");
            string paWebAPITesting = AppSettings.Get("PAWebAPITesting", "false");
            int paTimeOut = Convert.ToInt32(AppSettings.Get("PAResponseTimeOut", "2000"));
            int paRWTimeOut = Convert.ToInt32(AppSettings.Get("PARWResponseTimeOut", "2000"));

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(priorAuthSearchServiceWSCertificateName);
            string user = PriorAuthSearchWSURL;
            string client_secret = "";
            string secretName = "PriorAuthSearchPassword_OH_PNM_";
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
            Uri apiUrl = new Uri(PriorAuthSearchWSURL);
            WebRequest pmRequest = HttpWebRequest.Create(PriorAuthSearchWSURL);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";

            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = "dp.test.oh.healthinteractive.net:443";
            pmHttpRequest.Timeout = paTimeOut;
            pmHttpRequest.ReadWriteTimeout = paRWTimeOut;

            if (clientCertificate != null)
            {
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
            }
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", soapAction);
            var response = "";
            Stream requestStream = null;
            if (paWebAPITesting.Equals("false"))
            {
                requestStream = pmHttpRequest.GetRequestStream();
                if (requestStream != null)
                {
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
                    DataSet dswa = RecipientEligibilityDA.RetrieveWebAPITestingResponse("PriorAuthSearchWebAPI");
                    if (Methods.HasRows(dswa))
                    {
                        DataTable dtwa = dswa.Tables[0];
                        DataRow dr = dtwa.Rows.Count > 0 ? dtwa.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                }
                string pattern = @"(</?)(\w+:)";
                var output = Regex.Replace(response, pattern, "$1");
                var xDoc = XDocument.Parse(output);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Ignore;
                settings.XmlResolver = null;
                XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);
                ds.ReadXml(xmlReader);
                Logging log = new Logging(Guid.NewGuid(), "");
                log.CreateLogEntry(string.Format("{0} {1}", "search Auth response", response.ToString()), Logging.LogPriority.Information);
            }
            else
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, "search Auth requestStream : " + requestStream.ToString()), Logging.LogPriority.Information);
            }
            return ds;
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
        public static string GetStringDateTime(string elementName)
        {
            string returnValue = "";
            DateTime date;
            if (!string.IsNullOrEmpty(elementName))
            {
                returnValue = HttpUtility.HtmlDecode(elementName.Trim());
                returnValue = returnValue.Replace("''", "'");
                date = Convert.ToDateTime(returnValue, CultureInfo.InvariantCulture);
                returnValue = date.ToString("yyyy-MM-dd'T'HH:mm:ss");
            }

            return returnValue;
        }



    }

    public class SearchPrioAuthModel
    {

        public string MemberID { get; set; }
        public string MemberDOB { get; set; }
        public string PriorAuthorizationID { get; set; }

        public string AssignmentCode { get; set; }
        public string StatusCode { get; set; }
        public string OrderingProviderID { get; set; }
        public string InpatientProcedure { get; set; }
        public string PriorAuthorizationTypeCode { get; set; }
        public string ProcedureCode { get; set; }
        public string Diagnosis { get; set; }
        public string AuthSubmissionDate { get; set; }
        public string RevenueCode { get; set; }
        public string RecordStatusCode { get; set; }

        public string InputOffset { get; set; }
        public string InputLimit { get; set; }
        public string ProviderId { get; set; }

        public string PayerName { get; set; }

        public string PatientTrackingNumber { get; set; }

        public string PAEffectiveDate { get; set; }

        public string PAEndDate { get; set; }
    }
    //search fields:- 
    // txtMedicaidBillingNumber.Text // memberId
    //  txtBirthDate.Text // DOb
    //txtPriorAuthNumber.Text  // Authnumber
    // ddlAssignment.SelectedValue //Assignmentcode
    // ddlStatus.SelectedItem //StatusCode
    //txtorderProvnpi.Text// OrderingProviderID
    // InpatientProcedure not found in design
    //txtProcedureCode.Text//
    //txtRevenuecode.Text//RevenueCode
    // txtSubmissiondate.Text //AuthSubmissionDate
    // txtDiagnoisCode.Text//Diagnosis
    //RecordStatusCode not found in design

    //  txtPatientAccountNumber


}
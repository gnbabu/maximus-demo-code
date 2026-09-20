using MAXIMUS.Core.Libraries;
using System.Data;
using Corp.Core.Libraries;
using Newtonsoft.Json;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Data.SqlClient;
using PDMSRestServices.Models;
using PDMSRestServices.Facade;
using System.Runtime.InteropServices;
using Corp.Core.Libraries.FI.ClaimsSearchReference;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Web.Services3.Security.Tokens;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Globalization;

namespace PDMSRestServices.Controllers.Facade
{
    public class ClaimSearch
    {
        public static string CallClaimsSearchService(PDMSRestServices.Models.SearchClaimsRequest payload)
        {
            var searchRes = string.Empty;
            string requestPayload = string.Empty;
            string url = AppSettings.Get("ClaimsWSURL", Constants.WebServiceURI.ClaimsService);
            //string url = "http://localhost:8092/ClaimSearchSOAPVS";
            string wsCertificateName = AppSettings.Get("ClaimsWSCertificateName");
            string wsHost = AppSettings.Get("ClaimsWSHost");
            string wsUserName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
            Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
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
                client_secret += String.Concat("ClaimsWSPassword_", AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string wsPassword = client_secret;
            int claimsTimeOut = Convert.ToInt32(AppSettings.Get("ClaimsResponseTimeOut", "2000"));
            int claimsRWTimeOut = Convert.ToInt32(AppSettings.Get("ClaimsRWResponseTimeOut", "2000"));

            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(wsCertificateName);

                PDMSRestServices.Models.ClaimsSearch.MessageHeaderType messageHeader = CreateSearchMessageHeader();

                PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequestPayloadType request = new Models.ClaimsSearch.SearchClaimsRequestPayloadType();

                if (!string.IsNullOrEmpty(payload.PayorType))
                    request.PayorType = payload.PayorType;

                if (!string.IsNullOrEmpty(payload.MemberMedicaidId))
                    request.MemberMedicaidId = payload.MemberMedicaidId;

                if (!string.IsNullOrEmpty(payload.ICN))
                    request.ICN = payload.ICN;

                if (!string.IsNullOrEmpty(payload.PatientAccountNumber))
                    request.PatientAccountNumber = payload.PatientAccountNumber;

                if (!string.IsNullOrEmpty(payload.RenderingProviderID))
                    request.RenderingProviderID = payload.RenderingProviderID;

                if (!string.IsNullOrEmpty(payload.BillingProviderID))
                    request.BillingProviderID = payload.BillingProviderID;

                if (!string.IsNullOrEmpty(payload.PrescriptionNumber))
                    request.PrescriptionNumber = payload.PrescriptionNumber;

                if (!string.IsNullOrEmpty(payload.ClaimType))
                    request.ClaimType = payload.ClaimType;

                if (!string.IsNullOrEmpty(payload.Status))
                    request.Status = payload.Status;

                if (!string.IsNullOrEmpty(payload.PageSize))
                    request.PageSize = payload.PageSize;

                if (!string.IsNullOrEmpty(payload.Offset))
                    request.Offset = payload.Offset;


                if (payload.TotalChargesSpecified != null && payload.TotalChargesSpecified != false)
                    request.TotalChargesSpecified = Convert.ToBoolean(payload.TotalChargesSpecified);

                if (payload.TotalCharges != null && payload.TotalCharges > 0)
                    request.TotalCharges = Convert.ToDecimal(payload.TotalCharges);

                if (payload.FromDOSSpecified != null && payload.FromDOSSpecified != false)
                    request.FromDOSSpecified = Convert.ToBoolean(payload.FromDOSSpecified);

                if (payload.ThruDOSSpecified != null && payload.ThruDOSSpecified != false)
                    request.ThruDOSSpecified = Convert.ToBoolean(payload.ThruDOSSpecified);

                if (payload.RemittanceAdviceDateSpecified != null && payload.RemittanceAdviceDateSpecified != false)
                    request.RemittanceAdviceDateSpecified = Convert.ToBoolean(payload.RemittanceAdviceDateSpecified);

                if (!string.IsNullOrEmpty(payload.FromDOS) && IsValidDate(payload.FromDOS))
                {
                    if (payload.FromDOS != null && Convert.ToDateTime(payload.FromDOS) != DateTime.MinValue)
                        request.FromDOS = Convert.ToDateTime(payload.FromDOS);
                }
                if (!string.IsNullOrEmpty(payload.ThruDOS) && IsValidDate(payload.ThruDOS))
                {
                    if (payload.ThruDOS != null && Convert.ToDateTime(payload.ThruDOS) != DateTime.MinValue)
                        request.ThruDOS = Convert.ToDateTime(payload.ThruDOS);
                }
                if (!string.IsNullOrEmpty(payload.RemittanceAdviceDate) && IsValidDate(payload.RemittanceAdviceDate))
                {
                    if (payload.RemittanceAdviceDate != null && Convert.ToDateTime(payload.RemittanceAdviceDate) != DateTime.MinValue)
                        request.RemittanceAdviceDate = Convert.ToDateTime(payload.RemittanceAdviceDate);
                }

                PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequest searchClaimsRequestObj = new PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequest();
                searchClaimsRequestObj.MessageHeader = messageHeader;
                searchClaimsRequestObj.RequestPayload = request;
                string clWebAPITesting = AppSettings.Get("ClaimsWebAPITesting", "false");

                XmlSerializer x = new XmlSerializer(typeof(PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequest));

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("sear", "http://service.operationmgmt.fi/ClaimsService/search");
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var streamRequest = new StringWriter();
                var writer = XmlWriter.Create(streamRequest, settings);
                x.Serialize(writer, searchClaimsRequestObj, ns);
                string xml = streamRequest.ToString();
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:sear=\"http://service.operationmgmt.fi/ClaimsService/search\">"
                       + Environment.NewLine + WssSecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
                       Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.LoadXml(xml);
                xmlDoc.DocumentElement.SetAttribute("xmlns:sear", "http://service.operationmgmt.fi/ClaimsService/search");
                // OHPNM-7319 hack to remove an empty TotalCharges element from the search string
                var searchClaim = xmlDoc.InnerXml.ToString().Replace("SearchClaimsRequest xmlns:sear=\"http://service.operationmgmt.fi/ClaimsService/search\"", "SearchClaimsRequest")
                    .Replace("SearchClaimsRequest", "sear:SearchClaimsRequest")
                    .Replace("MessageHeader", "sear:MessageHeader")
                    .Replace("RequestPayload", "sear:RequestPayload")
                    .Replace("<sear:TotalCharges>0</sear:TotalCharges>", "");

                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(searchClaim);
                ServicePointManager.Expect100Continue = true;
                requestPayload = searchClaim;
                Uri apiUrl = new Uri(url);
                WebRequest pmRequest = HttpWebRequest.Create(apiUrl);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = wsHost;
                pmHttpRequest.Timeout = claimsTimeOut;
                pmHttpRequest.ReadWriteTimeout = claimsRWTimeOut;


                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(wsUserName + ":" + wsPassword));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", CON.ClaimSearchSoapAction.ClaimSearchSoap);
                Stream requestStream;
                if (clWebAPITesting.Equals("true"))
                {
                    requestStream = null;
                }
                else
                {
                    requestStream = pmHttpRequest.GetRequestStream();
                }

                if (requestStream != null || clWebAPITesting.Equals("true"))
                {
                    if (clWebAPITesting.Equals("false"))
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();


                        try
                        {
                            using (WebResponse pmResponse = pmHttpRequest.GetResponse())
                            {
                                HttpWebResponse pmHttpResponse = (HttpWebResponse)pmResponse;

                                HttpStatusCode statuscode = pmHttpResponse.StatusCode;

                                using (Stream stream = pmResponse.GetResponseStream())
                                {
                                    using (StreamReader sr = new StreamReader(stream))
                                    {
                                        searchRes = sr.ReadToEnd();
                                        //WebClient client = new WebClient();
                                        //string response1 = client.DownloadString("D:\\test.xml");
                                        //searchRes = response1;
                                    }
                                }
                            }
                        }
                        catch (WebException ex)
                        {
                            log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
                            searchRes = "<soapenv:Envelope xmlns:sear=\"http://service.operationmgmt.fi/ClaimsService/search\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n  <soapenv:Body>\r\n    <SearchClaimsResponse xmlns=\"http://service.operationmgmt.fi/ClaimsService/search\">\r\n      <ResponseHeader>\r\n        <SITransactionKey>no SI Key was received..</SITransactionKey>\r\n        <ResponseCode>4</ResponseCode>\r\n        <ResponseType>FAILURE</ResponseType>\r\n        <ResponseMessage>Error reading web reply.</ResponseMessage>\r\n        <ResponseDetails>FAILURE: Error reading web reply.</ResponseDetails>\r\n      </ResponseHeader>\r\n      <Errors>\r\n        <ErrorDetails>\r\n          <ErrorCode>4</ErrorCode>\r\n          <ErrorDescription>Error Reading Web Reply.</ErrorDescription>\r\n        </ErrorDetails>\r\n      </Errors>\r\n    </SearchClaimsResponse>\r\n  </soapenv:Body>\r\n</soapenv:Envelope>";
                        }
                        LogRequestResponseWithPassThrough(searchClaimsRequestObj, searchClaim, searchRes, payload.UserName);
                    }
                    else
                    {
                        DataSet ds = RecipientEligibilityDA.RetrieveWebAPITestingResponse("ClaimsSearchWebAPI");
                        DataTable dt = ds.Tables[0];
                        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        searchRes = GetString("APIXML", dr);

                        LogRequestResponseWithPassThrough(searchClaimsRequestObj, searchClaim, searchRes, payload.UserName);
                    }
                }
            }
            catch (Exception ex)
            {    //To do
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException, ex.StackTrace.ToString()), Logging.LogPriority.Error);
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimSearch, string.Empty, string.Empty, string.Empty,
                       xmlDoc.InnerXml.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Convert.ToString(GetCurrentUserId(payload.UserName)), Convert.ToString(GetCurrentUserId(payload.UserName)),
                       0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Search" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
            }

            return searchRes;
        }
        public static string GetString(string elementName, DataRow dr)
        {
            string returnValue = "";
            if (dr.Table.Columns.Contains(elementName))
            {
                if (!dr.IsNull(elementName))
                {
                    returnValue = HttpUtility.HtmlDecode(dr[elementName].ToString());
                    returnValue = returnValue.Replace("''", "'");
                }
            }
            return returnValue;
        }
        public static string SecurityHeader(string wsUserName, string wsPassword)
        {
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(wsUserName, wsPassword, PasswordOption.SendPlainText);
                string xml = "<soapenv:Header>" +
                    @"<wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">" +
                        usernameTokenSection.GetXml(new XmlDocument()).OuterXml.ToString().
                        Replace("<wsse:Nonce", "<wsse:Nonce EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\"") +
                    "</wsse:Security>" +
                  "</soapenv:Header>";
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string WssSecurityHeader(string userName, string password)
        {
            try
            {
                Guid gud = Guid.NewGuid();
                var soapSecurityHeader = new SoapSecurityHeader("SecurityToken-" + gud.ToString(), userName, password);

                string str = soapSecurityHeader.ToString();
                str = str.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "");
                str = str.Replace(" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\"", "");
                str = "<soapenv:Header>" + str + "</soapenv:Header>";
                return str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static void LogRequestResponseWithPassThrough(PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequest requestObj,
                string request, string response, string userName = "")
        {
            int passthroughType = (int)PassthroughController.PassthroughTransactionType.ClaimSearch;
            var ackresponse = string.Empty;
            string responseCode = string.Empty;
            string responseDetails = string.Empty;
            string responseMessage = string.Empty;
            string responseType = string.Empty;
            string moduleTransactionId = string.Empty;
            string SITransactionKey = string.Empty;
            string additionalModuleID = String.Empty;
            string message = string.Empty;
            string requestorSystem = string.Empty;
            string subscriberSystem = string.Empty;
            DataSet resDataset = new DataSet();
            Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
            try
            {
                if (requestObj != null)
                {

                    PDMSRestServices.Models.ClaimsSearch.MessageHeaderType mHeader = requestObj.MessageHeader;
                    PDMSRestServices.Models.ClaimsSearch.SearchClaimsRequestPayloadType payload = requestObj.RequestPayload;

                    var xDoc = XDocument.Parse(response);
                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.XmlResolver = null;
                    XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);

                    resDataset.ReadXml(xmlReader);

                    if (mHeader != null)
                    {
                        requestorSystem = mHeader.RequestorSystem.ToString();
                        subscriberSystem = String.Join(", ", mHeader.SubscriberSystem.ToArray());

                    }


                    if (resDataset != null & resDataset.Tables.Count > 0)
                    {
                        DataTable dtSearchResponse = resDataset.Tables["ResponseHeader"] != null ? resDataset.Tables["ResponseHeader"] : null;

                        if (dtSearchResponse != null && dtSearchResponse.Rows.Count > 0)
                        {
                            DataRow drHeader = dtSearchResponse.Rows[0];
                            SITransactionKey = dtSearchResponse.Columns.Contains("SITransactionKey") ? drHeader["SITransactionKey"].ToString() : null;
                            moduleTransactionId = dtSearchResponse.Columns.Contains("ModuleTransactionId") ? drHeader["ModuleTransactionId"].ToString() : null;
                            additionalModuleID = dtSearchResponse.Columns.Contains("AdditionalModuleTransactionId") ? drHeader["AdditionalModuleTransactionId"].ToString() : null;
                            responseCode = dtSearchResponse.Columns.Contains("ResponseCode") ? drHeader["ResponseCode"].ToString() : null;
                            responseType = dtSearchResponse.Columns.Contains("ResponseType") ? drHeader["ResponseType"].ToString() : null;
                            responseMessage = dtSearchResponse.Columns.Contains("ResponseMessage") ? drHeader["ResponseMessage"].ToString() : null;
                            responseDetails = dtSearchResponse.Columns.Contains("ResponseDetails") ? drHeader["ResponseDetails"].ToString() : null;
                        }
                        try
                        {
                            response = getXMLPA(response);
                            PassthroughController.InsertPassthroughTransactionQueue(passthroughType, SITransactionKey, moduleTransactionId, additionalModuleID
                               , request, response, requestorSystem, subscriberSystem, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Convert.ToString(GetCurrentUserId(userName)), Convert.ToString(GetCurrentUserId(userName))
                               , 0, string.Empty, string.Empty, responseCode, responseType, responseMessage, responseDetails);
                        }
                        catch (Exception ex)
                        {
                            PassthroughController.InsertPassthroughTransactionQueue(passthroughType, SITransactionKey, moduleTransactionId, additionalModuleID
                               , request, null, requestorSystem, subscriberSystem, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Convert.ToString(GetCurrentUserId(userName)), Convert.ToString(GetCurrentUserId(userName))
                               , 0, string.Empty, string.Empty, string.Empty, string.Empty, ex.Message, ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
                PassthroughController.InsertPassthroughTransactionQueue(passthroughType, SITransactionKey, moduleTransactionId, additionalModuleID
                   , request, null, string.Empty, string.Empty, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Convert.ToString(GetCurrentUserId(userName)), Convert.ToString(GetCurrentUserId(userName))
                   , 0, string.Empty, string.Empty, string.Empty, string.Empty, ex.Message, ex.Message);
            }
        }
        public static Guid GetCurrentUserId(string Username)
        {
            return HelperFacade.GetUserId(Username);
        }
        private static string getXMLPA(string resp)
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
        private static PDMSRestServices.Models.ClaimsSearch.MessageHeaderType CreateSearchMessageHeader()
        {

            List<PDMSRestServices.Models.ClaimsSearch.MessageHeaderTypeSubscriber> msgHeaderSubList = new List<PDMSRestServices.Models.ClaimsSearch.MessageHeaderTypeSubscriber>();
            msgHeaderSubList.Add(PDMSRestServices.Models.ClaimsSearch.MessageHeaderTypeSubscriber.FI);

            PDMSRestServices.Models.ClaimsSearch.MessageHeaderType msgHeader = new PDMSRestServices.Models.ClaimsSearch.MessageHeaderType
            {
                BusinessFlow = PDMSRestServices.Models.ClaimsSearch.MessageHeaderTypeBusinessFlow.SearchClaims,
                RequestorSystem = PDMSRestServices.Models.ClaimsSearch.MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = Constants.StateCode,
                SubscriberSystem = msgHeaderSubList.ToArray()
            };
            return msgHeader;
        }
        public static DataSet GetClaimsDBData(PDMSRestServices.Models.SearchClaimsRequest payload)
        {
            DataSet dsresponse;
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(payload.MemberMedicaidId))
            {
                parameters.Add(SqlParms.CreateParameter("MedicalBillingNumber", DbType.String, payload.MemberMedicaidId.Trim(), true));
            }
            if (!string.IsNullOrEmpty(payload.PatientAccountNumber))
            {
                parameters.Add(SqlParms.CreateParameter("PatientAccountNumber", DbType.String, payload.PatientAccountNumber.Trim(), true));
            }
            if (!string.IsNullOrEmpty(payload.ClaimType))
            {
                parameters.Add(SqlParms.CreateParameter("ClaimType", DbType.String, payload.ClaimType.ToString(), true));
            }
            if (!string.IsNullOrEmpty(payload.PrescriptionNumber))
            {
                parameters.Add(SqlParms.CreateParameter("PrescriptionNumber", DbType.String, payload.PrescriptionNumber.Trim(), true));
            }
            if (payload.TotalCharges != null && payload.TotalCharges > 0)
            {
                parameters.Add(SqlParms.CreateParameter("AmountBilled", DbType.String, payload.TotalCharges, true));
            }
            if (!string.IsNullOrEmpty(payload.PayorType))
            {
                parameters.Add(SqlParms.CreateParameter("PayorName", DbType.String, payload.PayorType.ToString(), true));
            }
            if (!string.IsNullOrEmpty(payload.RenderingProviderID))
            {
                parameters.Add(SqlParms.CreateParameter("RenderingProviderID", DbType.String, payload.RenderingProviderID.Trim(), true));
            }
            if (!string.IsNullOrEmpty(payload.FromDOS))
            {
                if (payload.FromDOS != null && Convert.ToDateTime(payload.FromDOS) != DateTime.MinValue && Convert.ToDateTime(payload.FromDOS) > DateTime.MinValue)
                {
                    //parameters.Add(SqlParms.CreateParameter("FromDOS", DbType.DateTime, Convert.ToDateTime(payload.FromDOS), true));
                }
            }
            if (!string.IsNullOrEmpty(payload.ThruDOS))
            {
                if (payload.ThruDOS != null && Convert.ToDateTime(payload.ThruDOS) != DateTime.MinValue && Convert.ToDateTime(payload.ThruDOS) > DateTime.MinValue)
                {
                    //parameters.Add(SqlParms.CreateParameter("ToDOS", DbType.DateTime, Convert.ToDateTime(payload.ThruDOS), true));
                }
            }
            if (!string.IsNullOrEmpty(payload.BillingProviderID))
            {
                parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, payload.BillingProviderID.Trim(), true));
            }
            dsresponse = DataAccess.ExecuteStoredProcedure("usp_Search_Claim", parameters, "Search_Claim");

            Logging log = new Logging(Guid.NewGuid(), "Claim DB Search");
            log.CreateLogEntry(string.Format("{0} {1}", "", "Claim Search in db for Medicade id: " + payload.MemberMedicaidId.Trim() + "with DB result : " + dsresponse.Tables[0].ToString()), Logging.LogPriority.Information);

            return dsresponse;
        }
        public static List<T> GetPagedList<T>(List<T> list, int pageNumber, int pageSize)
        {
            if (list != null && list.Count() > 0)
            {
                int skip = (pageNumber) * pageSize;
                return list.Skip(skip).Take(pageSize).ToList();
            }
            return list;
        }

        private static bool IsValidDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return false;

            dateString = dateString.Trim();

            DateTime tempDate;

            bool isValid = DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out tempDate);
            return isValid;

        }
    }
}

using Corp.Core.Libraries.FI.ClaimsSearchReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{
    public static partial class ClaimsSearchServiceAgent
    {

       

       
        private static MessageHeaderType CreateSearchMessageHeader()
        {
            List<MessageHeaderTypeSubscriber> msgHeaderSubList = new List<MessageHeaderTypeSubscriber>();
            msgHeaderSubList.Add(MessageHeaderTypeSubscriber.FI);
          
            MessageHeaderType msgHeader = new MessageHeaderType
            {
                BusinessFlow = MessageHeaderTypeBusinessFlow.SearchClaims,
                RequestorSystem = MessageHeaderTypeRequestorSystem.PNM,
                RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss")),
                StateCode = Constants.StateCode,
                SubscriberSystem = msgHeaderSubList.ToArray()
            };
            return msgHeader;
        }

       

        public static string CallClaimsSearchService(SearchClaimsRequestPayloadType payload)
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

                MessageHeaderType messageHeader = CreateSearchMessageHeader();

                SearchClaimsRequest searchClaimsRequestObj = new SearchClaimsRequest();
                searchClaimsRequestObj.MessageHeader = messageHeader;
                searchClaimsRequestObj.RequestPayload = payload;
                string clWebAPITesting = AppSettings.Get("ClaimsWebAPITesting", "false");

                XmlSerializer x = new XmlSerializer(typeof(SearchClaimsRequest));
                
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
                       + Environment.NewLine + ServiceAgentHelper.SecurityHeader(wsUserName, wsPassword) + Environment.NewLine + "<soapenv:Body>" +
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
                        LogRequestResponseWithPassThrough(searchClaimsRequestObj, searchClaim, searchRes);
                    }
                    else
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();

                        parameters.Add(SqlHelper.CreateParameter("APIName", DbType.String, "ClaimsSearchWebAPI", true));

                        if (!string.IsNullOrEmpty(payload.MemberMedicaidId))
                            parameters.Add(SqlHelper.CreateParameter("medicaidBilling", DbType.String, payload.MemberMedicaidId, true));

                        if (!string.IsNullOrEmpty(payload.ClaimType))
                            parameters.Add(SqlHelper.CreateParameter("ClaimType", DbType.String, payload.ClaimType, true));

                        if (!string.IsNullOrEmpty(payload.PayorType))
                            parameters.Add(SqlHelper.CreateParameter("PayorName", DbType.String, payload.PayorType, true));

                        if (!string.IsNullOrEmpty(payload.ICN))
                            parameters.Add(SqlHelper.CreateParameter("ICN", DbType.String, payload.ICN, true));

                        if (payload.RemittanceAdviceDate != DateTime.MinValue && payload.RemittanceAdviceDate != null)
                            parameters.Add(SqlHelper.CreateParameter("RADate", DbType.Date, payload.RemittanceAdviceDate, true));

                        if (!string.IsNullOrEmpty(payload.PrescriptionNumber))
                            parameters.Add(SqlHelper.CreateParameter("PrecriptionNumber", DbType.String, payload.PrescriptionNumber, true));

                        if (payload.TotalCharges > 0)
                            parameters.Add(SqlHelper.CreateParameter("AmountBilled", DbType.String, Convert.ToString(payload.TotalCharges), true));

                        if (!string.IsNullOrEmpty(payload.RenderingProviderID))
                            parameters.Add(SqlHelper.CreateParameter("RenderingProviderId", DbType.String, payload.RenderingProviderID, true));

                        if (!string.IsNullOrEmpty(payload.Status))
                            parameters.Add(SqlHelper.CreateParameter("ClaimStatus", DbType.String, payload.Status, true));


                        //DataSet ds = RecipientEligibilityDA.RetrieveWebAPITestingResponse("ClaimsSearchWebAPI");
                        DataSet ds = RecipientEligibilityDA.RetrieveWebAPITestingResponseBySearchParam(parameters, "ClaimsSearchWebAPI");
                        DataTable dt = ds.Tables[0];

                        if (dt == null || dt.Rows.Count <= 0)
                        {
                            return searchRes;
                        }

                        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        searchRes = Methods.GetString("APIXML", dr);

                        LogRequestResponseWithPassThrough(searchClaimsRequestObj, searchClaim, searchRes);
                    }
                }
            }
            catch (Exception ex)
            {    //To do
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1} , Stack Trace:{2}", ex.Message, ex.InnerException,ex.StackTrace.ToString()), Logging.LogPriority.Error);
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimSearch, string.Empty, string.Empty, string.Empty,
                       xmlDoc.InnerXml.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                       0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Search" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
            }
         
            return searchRes;
        }
        private static void LogRequestResponseWithPassThrough(SearchClaimsRequest requestObj, string request, string response)
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

                    MessageHeaderType mHeader = requestObj.MessageHeader;
                    SearchClaimsRequestPayloadType payload = requestObj.RequestPayload;

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
                               , request, response, requestorSystem, subscriberSystem, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString()
                               , 0, string.Empty, string.Empty, responseCode, responseType, responseMessage, responseDetails);
                        }
                        catch (Exception ex)
                        {
                            PassthroughController.InsertPassthroughTransactionQueue(passthroughType, SITransactionKey, moduleTransactionId, additionalModuleID
                               , request, null, requestorSystem, subscriberSystem, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString()
                               , 0, string.Empty, string.Empty, string.Empty, string.Empty, ex.Message, ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("Exception Message : {0}, Inner Exception : {1}", ex.Message, ex.InnerException), Logging.LogPriority.Error);
                PassthroughController.InsertPassthroughTransactionQueue(passthroughType, SITransactionKey, moduleTransactionId, additionalModuleID
                   , request, null, string.Empty, string.Empty, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString()
                   , 0, string.Empty, string.Empty, string.Empty, string.Empty, ex.Message, ex.Message);
            }
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
    }
}

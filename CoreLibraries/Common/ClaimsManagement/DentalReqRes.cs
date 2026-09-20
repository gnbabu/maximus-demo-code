using MAXIMUS.Core.Libraries;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Corp.Core.Libraries.FI.ClaimsDentalService;
using CON = MAXIMUS.Core.Libraries.Constants;
using static Corp.Core.Libraries.PassthroughController;
using System.ServiceModel;
using System.Data;

namespace Corp.Core.Libraries.ClaimsManagement
{
    public class DentalReqRes : ClaimReqRes
    {
        public DentalReqRes() : base()
        {
            SetServieConfigData();
        }
        private Logging log;
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

        protected override void SetServieConfigData()
        {
            url = AppSettings.Get("ClaimsInquireWSURL", Constants.WebServiceURI.ClaimsDentalService);
            //url = "http://localhost:8090/ClaimsServiceSOAPVS";
            //url = "http://10.118.47.72:9103/ClaimsServiceDentalSOAPVS";
            userName = AppSettings.Get("ClaimsWSUserName");
            string client_secret = "";
            string secretName = "ClaimsWSPassword_OH_PNM_";
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
                ThreadId = Guid.NewGuid();
                Logging log = new Logging(ThreadId, "Secret Retrieval");
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
            password = client_secret;
            claimsCertName = AppSettings.Get("ClaimsWSCertificateName");
        }

        private string claimsInquireSoapAction = "http://service.operationmgmt.fi/ClaimsService/dental/InquireClaim";
        /// <summary>
        /// Claims Dental Inquiry service call
        /// </summary>
        /// <param name="payorType">Payer Type</param>
        /// <param name="icn">ICN#</param>
        /// <param name="providerId">Provider Id</param>
        /// <returns></returns>
        public InquireClaimResponse InquireClaimRequest(string payorType, string icn, string providerId)
        {
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            MessageHeaderType msgheader = new MessageHeaderType();
            msgheader.BusinessFlow = MessageHeaderTypeBusinessFlow.InquireClaim;

            msgheader.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;
            msgheader.ModuleTransactionId = "";
            msgheader.StateCode = "OH";
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                var sst = new MessageHeaderTypeSubscriber[1];
                sst[0] = MessageHeaderTypeSubscriber.FI;
                msgheader.SubscriberSystem = sst;
                msgheader.AdditionalModuleTransactionId = string.Empty;
                msgheader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
                msgheader.SITransactionKey = sitTransactionKey;

                var reqPayLoadType = new InquireClaimRequestPayloadType();
                reqPayLoadType.PayorType = payorType;
                reqPayLoadType.ICN = icn;
                reqPayLoadType.ProviderId = providerId;

                var inquireClaimReq = new InquireClaimRequest();
                inquireClaimReq.MessageHeader = msgheader;
                inquireClaimReq.RequestPayload = reqPayLoadType;


                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquireClaimRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, inquireClaimReq, emptyNs);
                string xml = stream2.ToString();
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://service.operationmgmt.fi/ClaimsService/dental\">"
                    + Environment.NewLine + wssSecurityHeader(userName, password) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                xmlDoc.LoadXml(xml);

                log = new Logging(m_threadId, "");

                log.CreateLogEntry(string.Format("{0} {1}", "", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
            }
            catch (Exception ex)
            {
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                 "ICN number: " + icn.ToString(), xmlDoc.InnerXml.ToString(), null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                 0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Dental Inquiry Response:" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
                throw ex;

            }
            return GetClaimsDentalInquireResponse(icn, providerId, xmlDoc, sitTransactionKey);
        }

        /// <summary>
        /// To get Claims Dental Inquiry service response
        /// </summary>
        /// <param name="icn"></param>
        /// <param name="xmlDoc"></param>
        /// <param name="soapAction"></param>
        /// <param name="sitTransactionKey"></param>
        /// <returns></returns>
        private InquireClaimResponse GetClaimsDentalInquireResponse(string icn, string medicaidId, XmlDocument xmlDoc, string sitTransactionKey)
        {
            InquireClaimResponse resr = new InquireClaimResponse();
            string SIWSHost = AppSettings.Get("SIWebServiceHost");
            string logHeader = string.Format("Get Claims Dental Inquiry for ICN# - " + icn);
            string clDentalWebAPITesting = AppSettings.Get("ClaimsWebAPITesting", "false");
            int claimsTimeOut = Convert.ToInt32(AppSettings.Get("ClaimsResponseTimeOut", "2000"));
            int claimsRWTimeOut = Convert.ToInt32(AppSettings.Get("ClaimsRWResponseTimeOut", "2000"));

            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(m_threadId, logMsg);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            CertUtil _certs = new CertUtil();
            X509Certificate2 clientCertificate = _certs.GetCertificateByName(claimsCertName);
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search request url :", url), Logging.LogPriority.Information);
            WebRequest pmRequest = HttpWebRequest.Create(url);
            HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
            pmHttpRequest.ContentType = "text/xml; charset=utf-8";
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search request XML :", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Information);
            pmHttpRequest.KeepAlive = true;
            pmHttpRequest.Method = "POST";
            pmHttpRequest.SendChunked = true;
            pmHttpRequest.UserAgent = ".NET Framework";
            pmHttpRequest.Host = SIWSHost;
            pmHttpRequest.Timeout = claimsTimeOut;
            pmHttpRequest.ReadWriteTimeout = claimsRWTimeOut;


            if (clientCertificate != null)
            {
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
            }
            String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(userName + ":" + password));
            pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
            pmHttpRequest.Headers.Add("SOAPAction", claimsInquireSoapAction);
            //log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search request for claimsInquireSoapAction :", claimsInquireSoapAction), Logging.LogPriority.Information);
            //log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search request :", pmHttpRequest), Logging.LogPriority.Information);
            // PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughTransactionType.ClaimInquiry, sitTransactionKey, null, null,
            //                      xmlDoc.InnerXml.ToString(), null, "PNM", "FI", medicaidId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
            //                      (int)CON.ClaimType.DENTAL,  "For ICN: " + icn, icn, null, null, null, null);

            Stream requestStream;
            var response = "";
            if (clDentalWebAPITesting.Equals("true"))
            {
                requestStream = null;
            }
            else
            {
                requestStream = pmHttpRequest.GetRequestStream();
            }

            try
            {
                if (requestStream != null || clDentalWebAPITesting.Equals("true"))
                {
                    if (clDentalWebAPITesting.Equals("false"))
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
                        DataSet ds = RecipientEligibilityDA.RetrieveWebAPITestingResponse("ClaimsInquiryDentalWebAPI");
                        DataTable dt = ds.Tables[0];
                        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }
                    log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search Response :", response), Logging.LogPriority.Information);


                    if (response.Contains(@"<soapenv:Envelope"))
                    {
                        response = response.Substring(response.IndexOf(@"<soapenv:Envelope"));
                    }

                    //Add Passthrough Transaction Que
                    var pid = PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughTransactionType.ClaimInquiry, sitTransactionKey, null, null,
                                xmlDoc.InnerXml.ToString(), response, "PNM", "FI", medicaidId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                                (int)CON.ClaimType.DENTAL, string.Empty, icn, null, null, null, null);

                    if (!response.Contains("<faultcode>"))
                    {
                        log = new Logging(m_threadId, "");
                        var output = response.Replace(">?<", "><").Replace("&amp;", "");
                        var xDocResp = XDocument.Parse(output);
                        var serializerResp = new XmlSerializer(typeof(DentalEnvRes));
                        DentalEnvRes eir = (DentalEnvRes)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));
                        resr = eir.Body.InquireClaimResponse;

                        //Update Passthrough Transaction Que
                        if (resr != null)
                        {
                            try
                            {
                                PassthroughController.UpdateTransactionQueueFIResponse(pid, resr.ResponseHeader.ResponseCode, resr.ResponseHeader.ResponseType.ToString(), resr.ResponseHeader.ResponseMessage, DateTime.Now, Methods.GetCurrentUserId(), resr.ResponseHeader.SITransactionKey, resr.ResponseHeader.ResponseMessage);
                            }
                            catch (Exception ex)
                            {
                                log = new Logging(m_threadId, "");
                                log.CreateLogEntry(string.Format("{0} {1}", "UpdateTransactionQueueFIResponse", ex), Logging.LogPriority.Error);
                                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimSearch, string.Empty, string.Empty, string.Empty,
                            resr.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                            0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Dental" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry(string.Format("{0} {1}", "DentalInquiryResponseError", response.ToString()), Logging.LogPriority.Error);
                        PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughTransactionType.ClaimInquiry, sitTransactionKey, null, null,
                                xmlDoc.InnerXml.ToString(), response, "PNM", "FI", medicaidId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                                (int)CON.ClaimType.DENTAL, string.Empty, icn, null, null, null, null);
                        throw new FaultException(response.ToString());
                    }
                }
                else
                {
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, "requestStream : " + requestStream.ToString()), Logging.LogPriority.Information);
                    PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughTransactionType.ClaimInquiry, sitTransactionKey, null, null,
                               xmlDoc.InnerXml.ToString(), null, "PNM", "FI", medicaidId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                               (int)CON.ClaimType.DENTAL, string.Empty, icn, null, null, null, null);
                }
            }
            catch (Exception ex)
            {
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughTransactionType.ClaimInquiry, sitTransactionKey, null, null,
                               xmlDoc.InnerXml.ToString(), null, "PNM", "FI", medicaidId, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                               (int)CON.ClaimType.DENTAL, string.Empty, icn, null, null, ex.Message, null);
            }
            return resr;
        }
    }
}
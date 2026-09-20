using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Security.Tokens;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Corp.Core.Libraries.EligInquiryService;
using System.Linq;
using System.ServiceModel;
using System.Collections.Generic;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace Corp.Core.Libraries
{
    public class RecipientEligibilitySearchReqRes
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
        public RecipientEligibilitySearchReqRes()
        {
            ThreadId = Guid.NewGuid();
        }
        private Logging log = null;
        private static string eligibilityWSURL = AppSettings.Get("EligibilityWSURL", Constants.WebServiceURI.RecipientEligibilityService);
        //private static string eligibilityWSURL = "http://localhost:8088/MemberEligibilityService";
        //private static string eligibilityWSURL = "http://10.118.47.72:9107/MemberEligibilityInquiryServiceSOAPVS";
        private static string eligibilityWSUserName = AppSettings.Get("EligibilityWSUserName");


        /// <summary>
        /// Method to call the MemberEligibility service call as request/response stream
        /// </summary>
        /// <param name="medicaidID">Medicaid Id</param>
        /// <param name="ssn">SSN</param>
        /// <param name="dob">Date of Birth</param>
        /// <param name="fromdate">From Date</param>
        /// <param name="toDate">To Date</param>
        /// <param name="procedureCode">Procedure Code</param>
        /// <param name="medicaidbillingnumber">Medicaid Billing#</param>
        /// <returns></returns>
        public RecipientEligibilitySearchResponse SearchRequest(string ssn, string medicaidID, Guid userId, string dob, string fromdate, string toDate, string procedureCode, string medicaidbillingnumber, string requestType = null)
        {
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            MessageHeaderType msgheader = new MessageHeaderType();
            msgheader.BusinessFlow = Constants.InqMessageHeaderBusinessFlow.InquireMemberEligibility.ToString();

            msgheader.RequestorSystem = MessageHeaderType.RequestorSystemType.PNM;
            msgheader.ModuleTransactionId = "";
            msgheader.StateCode = "OH";

            var sst = new MessageHeaderType.SubscriberSystemType();
            sst.Add("FI");
            msgheader.SubscriberSystem = sst;
            msgheader.AdditionalModuleTransactionId = string.Empty;
            msgheader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss"));
            msgheader.SITransactionKey = sitTransactionKey;

            var reqPayLoadType = new InquireMemberEligibilityRequestPayloadType();
            string dobValue = !string.IsNullOrEmpty(dob) ? Convert.ToDateTime(dob).ToString("yyyy-MM-dd") : string.Empty;
            reqPayLoadType.DateOfBirth = dobValue;
            reqPayLoadType.FromDateOfService = Convert.ToDateTime(fromdate).ToString("yyyy-MM-dd");
            reqPayLoadType.MedicaidId = medicaidbillingnumber;
            if (!string.IsNullOrEmpty(procedureCode))
                reqPayLoadType.ProcedureCode = procedureCode;
            reqPayLoadType.SSN = ssn;
            reqPayLoadType.ToDateOfService = Convert.ToDateTime(toDate).ToString("yyyy-MM-dd");

            var requestBodyobj = new InquireMemberEligibilityRequestBody();
            requestBodyobj.MessageHeader = msgheader;
            requestBodyobj.RequestPayload = reqPayLoadType;

            var requestObj = new InquireMemberEligibilityRequest(requestBodyobj);

            /**************************************************************************************/
            ////If you want to call this as Service Client(not as Request/Response Stream) - Keep this code in case if we ever wanted to try this method later
            //var binding = new BasicHttpBinding();
            //EndpointAddress ea = new EndpointAddress(eligibilityWSURL);
            //MemberEligibilityInquiryServiceClient eis = new MemberEligibilityInquiryServiceClient(binding, ea);

            //eis.ClientCredentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.TrustedPublisher, X509FindType.FindByIssuerName, "name_of_issuer");
            //((BasicHttpBinding)eis.Endpoint.Binding).Security.Mode = BasicHttpSecurityMode.Transport;
            //((BasicHttpBinding)eis.Endpoint.Binding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;

            //MemberEligibilityInfoType meit = new MemberEligibilityInfoType();
            //ErrorsType et = new ErrorsType();

            //var user = eis.ClientCredentials.UserName;
            //user.UserName = eligibilityWSURLWSUserName;
            //user.Password = eligibilityWSURLWSPassword;

            //eis.InquireMemberEligibility(msgheader, reqPayLoadType, out meit, out et);
            /**************************************************************************************/

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(InquireMemberEligibilityRequest));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            //emptyNs.Add("mem", "http://service.membermgmt.fi/MemberEligibilityInquiryService");

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, requestObj, emptyNs);
            string xml = stream2.ToString().Replace("<Body>", "").Replace("</Body>", "");
            string client_secret = "";
            string secretName = "EligibilityWSPassword_OH_PNM_";
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

            //Hack for Subscriber system
            xml = xml.Replace("<string>FI</string>", "<Subscriber>FI</Subscriber>");
            xml = xml.Replace("<", "<mem:").Replace("<mem:/", "</mem:");

            xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:mem=\"http://service.membermgmt.fi/MemberEligibilityInquiryService\">"
                + Environment.NewLine + wssSecurityHeader(eligibilityWSUserName, client_secret) + Environment.NewLine + "<soapenv:Body>" +
                Environment.NewLine + xml;
            xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            log.CreateLogEntry(string.Format("{0} {1}", "", xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);

            return eligibilityWSRequestResponse(medicaidbillingnumber, medicaidID, userId, xmlDoc, Constants.EliglibityServiceSoapAction.EligVerifResponse, sitTransactionKey, requestType);
        }

        /// <summary>
        /// WebService call to FI for MemberEligibility verification
        /// </summary>
        /// <param name="medID">Medicaid Billing Number</param>
        /// <param name="xmlDoc"></param>
        /// <param name="soapAction"></param>
        /// <param name="sitTransactionKey"></param>
        /// <returns></returns>
        public RecipientEligibilitySearchResponse eligibilityWSRequestResponse(string medID, string medicaidID, Guid userId, XmlDocument xmlDoc,
            string soapAction, string sitTransactionKey, string requestType = null)
        {
            string logHeader = string.Format("Eligibility Get Transaction History for Medicaid Billing Number - " + medID);
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            RecipientEligibilitySearchResponse resr = new RecipientEligibilitySearchResponse();
            var response = "";
            try
            {
                string eligibilityServiceWSCertificateName = AppSettings.Get("EligibilityServiceWSCertificateName");
                string SIWSHost = AppSettings.Get("SIWebServiceHost");
                string meWebAPITesting = AppSettings.Get("MEWebAPITesting", "false");
                int eligibilityTimeOut = Convert.ToInt32(AppSettings.Get("EligibilityResponseTimeOut", "2000"));
                int eligibilityRWTimeOut = Convert.ToInt32(AppSettings.Get("EligibilityRWResponseTimeOut", "2000"));

                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(eligibilityServiceWSCertificateName);
                WebRequest pmRequest = HttpWebRequest.Create(eligibilityWSURL);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = SIWSHost;
                pmHttpRequest.Timeout = eligibilityTimeOut;
                pmHttpRequest.ReadWriteTimeout = eligibilityRWTimeOut;

                if (clientCertificate != null)
                {
                    pmHttpRequest.ClientCertificates.Add(clientCertificate);
                }
                string client_secret = "";
                string secretName = "EligibilityWSPassword_2_OH_PNM_";
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
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(eligibilityWSUserName + ":" + client_secret));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);

                Stream requestStream;
                if (meWebAPITesting.Equals("true"))
                {
                    requestStream = null;
                }
                else
                {
                    requestStream = pmHttpRequest.GetRequestStream();
                }
                if (requestStream != null || meWebAPITesting.Equals("true"))
                {
                    if (meWebAPITesting.Equals("false"))
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
                        DataSet ds = RecipientEligibilityDA.RetrieveWebAPITestingResponse("MemberEligibilityWebAPI");
                        DataTable dt = ds.Tables[0];
                        DataRow dr = dt.Rows.Count > 0 ? dt.Rows[0] : null;
                        response = Methods.GetString("APIXML", dr);
                    }

                    var output = response.Replace(">?<", "><");
                    var xDoc = XDocument.Parse(output);

                    try
                    {
                        var responseHeader = GetEligibilityResponseHeader(xDoc);
                        resr.ResponseHeaderDetails = responseHeader;
                        if (responseHeader != null)
                        {
                            //save to outbound log
                            RecipientEligibilityDA.SaveMemberEligibilityServiceReqRes(responseHeader.SITransactionKey, medicaidID, requestType,
                                xmlDoc.InnerXml.ToString(), response, responseHeader.ResponseCode, responseHeader.ResponseMessage,
                                responseHeader.ResponseType, userId, "");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.CreateLogEntry(string.Format("{0} {1}", logHeader, "Exception : " + ex.StackTrace.ToString()), Logging.LogPriority.Information);
                    }

                    if ((response.Contains("FAILURE")) || (response.Contains("Failure")))
                    {
                        DataSet ds = new DataSet();
                        TextReader tr = new StringReader(xDoc.ToString());
                        ds.ReadXml(tr);
                        resr = new RecipientEligibilitySearchResponse();
                        if (ds.Tables["ErrorDetails"] != null && ds.Tables["ErrorDetails"].Rows.Count > 0)
                        {
                            var listErrorDetails = new List<ErrorDetail>();
                            foreach (DataRow row in ds.Tables["ErrorDetails"].Rows)
                            {
                                ErrorDetail errorDetail = new ErrorDetail();
                                errorDetail.Code = row["ErrorCode"].ToString();
                                errorDetail.Description = row["ErrorDescription"].ToString();
                                listErrorDetails.Add(errorDetail);
                            }
                            resr.ErrorDetails = listErrorDetails;
                        }
                    }
                    else if (!response.Contains("<faultcode>"))
                    {
                        //Load data in a typed object
                        resr = GetEligibilityDataFromResponse(xDoc);
                    }
                    else
                    {
                        log.CreateLogEntry(string.Format("{0} {1}", "Response", response.ToString()), Logging.LogPriority.Error);
                        throw new FaultException(response.ToString());
                    }
                }
                else
                {
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, "requestStream : " + requestStream.ToString()), Logging.LogPriority.Information);
                }
            }
            catch (WebException ex)
            {
                string respBody = ex.Response != null ? new StreamReader(ex.Response.GetResponseStream()).ReadToEnd() : ex.ToString();
                //Log request and response to Outbound_Member_Eligibility_Service_Req_Res table
                RecipientEligibilityDA.SaveMemberEligibilityServiceReqRes(sitTransactionKey, medicaidID, requestType, xmlDoc.InnerXml.ToString(), respBody, CON.MemberEligibilityErrors.MEErrorCode0, "Exception->RecipientEligibilitySearchReqRes->eligibilityWSRequestResponse ->" + ex, CON.MemberEligibilityErrors.MEFailureType,
                                     userId, "");
                log.CreateLogEntry(string.Format("{0} {1} {2}", "Exception -> RecipientEligibilitySearchReqRes -> eligibilityWSRequestResponse", ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
            }
            catch (Exception ex)
            {
                string exception = ex.Message.ToString();
                if (ex.InnerException != null && string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    exception += ex.InnerException.Message;
                }
                log.CreateLogEntry(string.Format("{0} {1}", "Exception -> RecipientEligibilitySearchReqRes -> eligibilityWSRequestResponse", exception), Logging.LogPriority.Error);
                RecipientEligibilityDA.SaveMemberEligibilityServiceReqRes(sitTransactionKey, medicaidID, requestType,
                    xmlDoc.InnerXml.ToString(), response, CON.MemberEligibilityErrors.MEErrorCode1,
                    "Exception->RecipientEligibilitySearchReqRes->eligibilityWSRequestResponse ->" + exception, CON.MemberEligibilityErrors.MEFailureType, userId, "");
            }
            return resr;
        }
        /// <summary>
        /// Parse the response stream and load the POCO response object
        /// </summary>
        /// <param name="xDoc">Response stream back from the service</param>
        /// <returns>RecipientEligibilitySearchResponse</returns>
        private RecipientEligibilitySearchResponse GetEligibilityDataFromResponse(XDocument xDoc)
        {
            var resr = new RecipientEligibilitySearchResponse();
            XNamespace ns = "http://service.membermgmt.fi/MemberEligibilityInquiryService";
            //Load Response Header info here
            ResponseHeader rh = GetEligibilityResponseHeader(xDoc);
            resr.ResponseHeaderDetails = rh;
            //Load Recipient Information here
            var ri = xDoc.Descendants(ns + "RecipientInfo").Select(x => new RecipientInformation
            {
                MedicaidId = (string)x.Element(ns + "MedicaidId"),
                DateOfBirth = x.Element(ns + "DateOfBirth").Value == "" ? null : (DateTime?)x.Element(ns + "DateOfBirth"),
                DateOfDeath = x.Element(ns + "DateOfDeath") != null ? (x.Element(ns + "DateOfDeath").Value == "" ? null : (DateTime?)x.Element(ns + "DateOfDeath")) : null,
                FirstName = (string)x.Element(ns + "FirstName") != null ? (string)x.Element(ns + "FirstName") : null,
                MiddleName = x.Element(ns + "MiddleName") != null ? (string)x.Element(ns + "MiddleName") : string.Empty,
                LastName = x.Element(ns + "LastName") != null ? (string)x.Element(ns + "LastName") : null,
                SSN = x.Element(ns + "SSN") != null ? (string)x.Element(ns + "SSN") : null,
                Gender = x.Element(ns + "Gender") != null ? (string)x.Element(ns + "Gender") : null,
                AddressLine1 = x.Element(ns + "MemberAddress") != null ? (x.Element(ns + "MemberAddress").Element(ns + "AddressLine1") != null ? (string)x.Element(ns + "MemberAddress").Element(ns + "AddressLine1") : null) : null,
                AddressLine2 = x.Element(ns + "MemberAddress") != null ? (x.Element(ns + "MemberAddress").Element(ns + "AddressLine2") != null ? (string)x.Element(ns + "MemberAddress").Element(ns + "AddressLine2") : null) : null,
                City = x.Element(ns + "MemberAddress") != null ? (x.Element(ns + "MemberAddress").Element(ns + "City") != null ? (string)x.Element(ns + "MemberAddress").Element(ns + "City") : null) : null,
                StateCode = x.Element(ns + "MemberAddress") != null ? (x.Element(ns + "MemberAddress").Element(ns + "StateCode") != null ? (string)x.Element(ns + "MemberAddress").Element(ns + "StateCode") : null) : null,
                ZipCode5 = x.Element(ns + "MemberAddress") != null ? (x.Element(ns + "MemberAddress").Element(ns + "ZipCode5") != null ? (string)x.Element(ns + "MemberAddress").Element(ns + "ZipCode5") : null) : null
            }).FirstOrDefault();
            resr.RecipientInfo = ri;
            //Load BenifitsAssignmentPlan list here
            var benifitResults = xDoc.Descendants(ns + "BenefitAssignmentPlan").Select(x => new BenifitAssignmentPlan
            {
                AssignmentPlan = (string)x.Element(ns + "AssignmentPlan"),
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")
            }).ToList();
            //Delete any empty objects when count is 1 and there is no data in it for all lists.
            if (benifitResults.Count == 1 && string.IsNullOrEmpty(benifitResults[0].AssignmentPlan) && benifitResults[0].EffectiveDate == null
                && benifitResults[0].EndDate == null)
                benifitResults = null;
            resr.BenifitAssignmentPlans = benifitResults;
            //Load ManagedCarePlan list here                
            var mcpResults = xDoc.Descendants(ns + "ManagedCarePlan").Select(x => new ManagedCarePlan
            {
                PlanId = (string)x.Element(ns + "ManagedCarePlanId"),
                PlanName = (string)x.Element(ns + "PlanName"),
                PlanDescription = (string)x.Element(ns + "PlanDescription"),
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate"),
                ManagedCareBenefits = x.Element(ns + "ManagedCareBenefits") != null ? (string)x.Element(ns + "ManagedCareBenefits") : null
            }).ToList();
            if (mcpResults.Count == 1 && mcpResults[0].PlanId == null && string.IsNullOrEmpty(mcpResults[0].PlanName)
                && string.IsNullOrEmpty(mcpResults[0].PlanDescription))
                mcpResults = null;
            resr.ManagedCarePlans = mcpResults;
            //Load ThirdPartyLiability list here
            var tplResults = xDoc.Descendants(ns + "ThirdPartyLiability").Select(x => new ThirdPartyLiability
            {
                CarrierName = x.Element(ns + "CarrierName") != null ? (string)x.Element(ns + "CarrierName") : null,
                CarrierNumber = x.Element(ns + "CarrierNumber") != null ? (string)x.Element(ns + "CarrierNumber") : null,
                PolicyNumber = x.Element(ns + "PolicyNumber") != null ? (string)x.Element(ns + "PolicyNumber") : null,
                PolicyHolder = x.Element(ns + "PolicyHolder") != null ? (string)x.Element(ns + "PolicyHolder") : null,
                CoverageType = x.Element(ns + "CoverageType") != null ? (string)x.Element(ns + "CoverageType") : null,
                Coverage = x.Element(ns + "Coverage") != null ? (string)x.Element(ns + "Coverage") : null,
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate"),
                GroupNumber = x.Element(ns + "GroupNumber") != null ? (string)x.Element(ns + "GroupNumber") : null
            }).ToList();
            if (tplResults.Count == 1 && tplResults[0].EffectiveDate == null && tplResults[0].EndDate == null)
                tplResults = null;
            resr.ThirdPartyLiabilities = tplResults;
            //Load PatientLiability list here
            var plResults = xDoc.Descendants(ns + "PatientLiability").Select(x => new PatientLiability
            {
                FinancialPayer = (string)x.Element(ns + "PatientLiabilityFinancialPayer"),
                MonthlyAmount = x.Element(ns + "MonthlyAmount").Value == "" ? null : (decimal?)x.Element(ns + "MonthlyAmount"),
                Type = x.Element(ns + "Type") != null ? (string)x.Element(ns + "Type") : null,
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")
            }).ToList();
            if (plResults.Count == 1 && string.IsNullOrEmpty(plResults[0].FinancialPayer) && plResults[0].EffectiveDate == null && plResults[0].EffectiveDate == null)
                plResults = null;
            foreach (var item in plResults)
            {
                if (item.MonthlyAmount.HasValue)
                    item.MonthlyAmount = Decimal.Round(item.MonthlyAmount.Value, 2);
            }
            resr.PatientLiabilities = plResults;
            //Load LTCFPlacements list here
            var ltcfpResults = xDoc.Descendants(ns + "LTCFPlacement").Select(x => new LTCFPlacement
            {
                FacilityType = (string)x.Element(ns + "FacilityType"),
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate") != null ? (x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")) : null,
                EffectiveDateMedicaidCoverage = x.Element(ns + "EffectiveDateMedicaidCoverage").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDateMedicaidCoverage"),
                EndDateMedicaidCoverage = x.Element(ns + "EndDateMedicaidCoverage").Value == "" ? null : (DateTime?)x.Element(ns + "EndDateMedicaidCoverage")
            }).ToList();
            if (ltcfpResults.Count == 1 && string.IsNullOrEmpty(ltcfpResults[0].FacilityType) && ltcfpResults[0].EffectiveDate == null
                && ltcfpResults[0].EffectiveDateMedicaidCoverage == null && ltcfpResults[0].EndDateMedicaidCoverage == null)
                plResults = null;
            resr.LTCFPlacements = ltcfpResults;
            //Load Lockins list here
            var liResults = xDoc.Descendants(ns + "LockinDetail").Select(x => new Lockin
            {
                LockinPlan = (string)x.Element(ns + "LockinPlan"),
                LockinType = (string)x.Element(ns + "LockinType"),
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate"),
                ProviderNPI = x.Element(ns + "ProviderNPI").Value == "" ? null : (string)x.Element(ns + "ProviderNPI"),
                ProviderName = (string)x.Element(ns + "ProviderName"),
                ProviderPhoneNumber = x.Element(ns + "ProviderPhoneNumber") != null ? (string)x.Element(ns + "ProviderPhoneNumber") : null
            }).ToList();
            if (liResults.Count == 1 && string.IsNullOrEmpty(liResults[0].LockinPlan) && string.IsNullOrEmpty(liResults[0].LockinType)
                && liResults[0].EffectiveDate == null && liResults[0].EndDate == null && liResults[0].ProviderNPI == null &&
                string.IsNullOrEmpty(liResults[0].ProviderName))
                liResults = null;
            resr.Lockins = liResults;
            //Load MedicareCoverageDetails list here
            var mcdResults = xDoc.Descendants(ns + "MedicareCoverageDetail").Select(x => new MedicareCoverageDetail
            {
                Coverage = (string)x.Element(ns + "Coverage"),
                PlanId = (string)x.Element(ns + "PlanId"),
                PlanName = (string)x.Element(ns + "PlanName"),
                MedicareId = (string)x.Element(ns + "MedicareId"),
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")
            }).ToList();
            if (mcdResults.Count == 1 && string.IsNullOrEmpty(mcdResults[0].Coverage) && string.IsNullOrEmpty(mcdResults[0].PlanId) &&
                string.IsNullOrEmpty(mcdResults[0].PlanName) && string.IsNullOrEmpty(mcdResults[0].MedicareId)
                && mcdResults[0].EffectiveDate == null && mcdResults[0].EndDate == null)
                mcdResults = null;
            resr.MedicareCoverageDetails = mcdResults;
            //Load SNIFlOCDetail list here
            var sniflocResults = xDoc.Descendants(ns + "SNIFlOCDetail").Select(x => new SNIFlOCDetail
            {
                FacilityType = (string)x.Element(ns + "FacilityType"),
                Status = (string)x.Element(ns + "Status"),
                DeterminationDate = x.Element(ns + "DeterminationDate").Value == "" ? null : (DateTime?)x.Element(ns + "DeterminationDate"),
                LOCDetermination = x.Element(ns + "LOCDetermination") != null ? (string)x.Element(ns + "LOCDetermination") : null,
                Description = x.Element(ns + "Description") != null ? (string)x.Element(ns + "Description") : null,
                StartDate = x.Element(ns + "StartDate").Value == "" ? null : (DateTime?)x.Element(ns + "StartDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")
            }).ToList();
            if (sniflocResults.Count == 1 && string.IsNullOrEmpty(sniflocResults[0].FacilityType) && string.IsNullOrEmpty(sniflocResults[0].Status) &&
                sniflocResults[0].DeterminationDate == null && sniflocResults[0].EndDate == null)
                sniflocResults = null;
            resr.SNIFlOCDetails = sniflocResults;

            //OHPNM-12784 
            foreach (var item in sniflocResults)
            {
                if (item.DeterminationDate != null && item.DeterminationDate == Convert.ToDateTime("12/31/2078"))
                    item.DeterminationDate = null;
            }

            //Load ServiceLimitations list here
            var slResults = xDoc.Descendants(ns + "ServiceLimitations").Select(x => new ServiceLimitation
            {
                ProcedureCode = x.Element(ns + "ProcedureCode") != null ? (string)x.Element(ns + "ProcedureCode") : null,
                ServiceLimitDescription = x.Element(ns + "ServiceLimitDescription") != null ? (string)x.Element(ns + "ServiceLimitDescription") : null,
                BenefitDescription = x.Element(ns + "BenefitDescription") != null ? (string)x.Element(ns + "BenefitDescription") : null,
                TotalLimits = x.Element(ns + "TotalLimits") != null ? (x.Element(ns + "TotalLimits").Value == "" ? null : (int?)x.Element(ns + "TotalLimits")) : null,
                UsedLimits = x.Element(ns + "UsedLimits") != null ? (x.Element(ns + "UsedLimits").Value == "" ? null : (int?)x.Element(ns + "UsedLimits")) : null,
                RemainingLimits = x.Element(ns + "RemainingLimits") != null ? (x.Element(ns + "RemainingLimits").Value == "" ? null : (int?)x.Element(ns + "RemainingLimits")) : null,
                Timeframe = x.Element(ns + "Timeframe") != null ? (string)x.Element(ns + "Timeframe") : null,
                DateOfNextService = x.Element(ns + "DateOfNextService") != null ? (x.Element(ns + "DateOfNextService").Value == "" ? null : (DateTime?)x.Element(ns + "DateOfNextService")) : null
            }).ToList();
            if (slResults.Count == 1 && string.IsNullOrEmpty(slResults[0].ProcedureCode) && string.IsNullOrEmpty(slResults[0].ServiceLimitDescription) &&
                string.IsNullOrEmpty(slResults[0].BenefitDescription) && slResults[0].TotalLimits == null && slResults[0].UsedLimits == null &&
                slResults[0].RemainingLimits == null && string.IsNullOrEmpty(slResults[0].Timeframe) && slResults[0].DateOfNextService == null)
                slResults = null;
            resr.ServiceLimitations = slResults;
            //Load RestrictedCoverages list here
            var rcResults = xDoc.Descendants(ns + "RestictedCoverage").Select(x => new RestrictedCoverage
            {
                EffectiveDate = x.Element(ns + "EffectiveDate").Value == "" ? null : (DateTime?)x.Element(ns + "EffectiveDate"),
                EndDate = x.Element(ns + "EndDate").Value == "" ? null : (DateTime?)x.Element(ns + "EndDate")
            }).ToList();
            if (rcResults.Count == 1 && rcResults[0].EffectiveDate == null && rcResults[0].EndDate == null)
                rcResults = null;
            resr.RestrictedCoverages = rcResults;
            //Load Under19FamilyMembers list here
            var u19fmResults = xDoc.Descendants(ns + "FamilyMemberInfo").Select(x => new Under19FamilyMember
            {
                MedicaidId = x.Element(ns + "MedicaidId").Value == "" ? null : (string)x.Element(ns + "MedicaidId"),
                DateOfBirth = x.Element(ns + "DateOfBirth") != null ? (DateTime?)x.Element(ns + "DateOfBirth") : null,
                FirstName = x.Element(ns + "FirstName") != null ? (string)x.Element(ns + "FirstName") : null,
                MiddleInitial = x.Element(ns + "MiddleName") != null ? (string)x.Element(ns + "MiddleName") : null,
                LastName = x.Element(ns + "LastName") != null ? (string)x.Element(ns + "LastName") : null,
                Gender = x.Element(ns + "Gender") != null ? (string)x.Element(ns + "Gender") : null
            }).ToList();
            if (u19fmResults.Count == 1 && u19fmResults[0].DateOfBirth == null && string.IsNullOrEmpty(u19fmResults[0].LastName))
                u19fmResults = null;
            resr.Under19FamilyMembers = u19fmResults;

            //Load any errors here
            var edResults = xDoc.Descendants(ns + "ErrorDetails").Select(x => new ErrorDetail
            {
                Code = (string)x.Element(ns + "ErrorCode"),
                Description = (string)x.Element(ns + "ErrorDescription")
            }).ToList();

            if (edResults.Count == 1 && string.IsNullOrEmpty(edResults[0].Code) && string.IsNullOrEmpty(edResults[0].Description))
                edResults = null;
            resr.ErrorDetails = edResults;

            return resr;
        }

        private static ResponseHeader GetEligibilityResponseHeader(XDocument xDoc)
        {
            XNamespace ns = "http://service.membermgmt.fi/MemberEligibilityInquiryService";
            return xDoc.Descendants(ns + "ResponseHeader").Select(x => new ResponseHeader
            {
                SITransactionKey = x.Element(ns + "SITransactionKey") != null ? (string)x.Element(ns + "SITransactionKey") : null,
                ModuleTransactionId = x.Element(ns + "ModuleTransactionId") != null ? (string)x.Element(ns + "ModuleTransactionId") : null,
                AdditionalModuleTransactionId = x.Element(ns + "AdditionalModuleTransactionId") != null ? (string)x.Element(ns + "AdditionalModuleTransactionId") : null,
                ResponseCode = x.Element(ns + "ResponseCode") != null ? (string)x.Element(ns + "ResponseCode") : null,
                ResponseType = x.Element(ns + "ResponseType") != null ? (string)x.Element(ns + "ResponseType") : null,
                ResponseMessage = x.Element(ns + "ResponseMessage") != null ? (string)x.Element(ns + "ResponseMessage") : null,
                ResponseDetails = x.Element(ns + "ResponseDetails") != null ? (string)x.Element(ns + "ResponseDetails") : null
            }).FirstOrDefault();
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

        public RecipientInformation GetRecipientInformation(string medicaidbillingnumber, string medicaidID, Guid userId, string dateofBirth, string requestType = null)
        {
            var resr = SearchRequest(string.Empty, medicaidID, userId, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"),
                string.Empty, medicaidbillingnumber, requestType);

            if (resr != null && resr.RecipientInfo != null)
            {
                if (resr.ErrorDetails != null)
                    resr.RecipientInfo.Errors = resr.ErrorDetails;
                return resr.RecipientInfo;
            }
            else if (resr.ErrorDetails != null)
            {
                var ri = new RecipientInformation();
                ri.Errors = resr.ErrorDetails;
                resr.RecipientInfo = ri;
                return resr.RecipientInfo;
            }
            return null;
        }
        public RecipientEligibilitySearchResponse GetRecipientEligibilitySearchResponse(string medicaidbillingnumber, string medicaidID, Guid userId, string dateofBirth, string requestType = null)
        {
            var resr = SearchRequest(string.Empty, medicaidID, userId, dateofBirth, DateTime.Now.AddMonths(-48).ToString("MM/dd/yyyy"), DateTime.Now.ToString("MM/dd/yyyy"),
                string.Empty, medicaidbillingnumber, requestType);
            return resr;
        }
    }
}
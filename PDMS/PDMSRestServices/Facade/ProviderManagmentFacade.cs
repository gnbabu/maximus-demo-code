using MAXIMUS.Core.Libraries;
using System.Data;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Corp.Core.Libraries;

namespace PDMSRestServices.Facade
{
    public class ProviderManagmentFacade
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
        public ProviderManagmentFacade()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;

        public string providerManagementEnrollRequest(int transactionID, bool makeRequest = false)
        {
            makeRequest = false;
            string strCheck = AppSettings.Get("MakeWSRequestCallToSI", "false");
            if (strCheck.Equals("true"))
            {
                makeRequest = true;
            }
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                DataSet ds = InfoAccessController.GetStagingProviderEnrollmentByTransactionID(transactionID);

                //Use this to perform dataFixes
                //PreProcessingDataFixes.PreProcessingDataFixesMain(log, transactionID);

                if (ds == null)
                {
                    log.CreateLogEntry(string.Format("{0} DataSet Returned null for transactionID:- {1}", logHeader, transactionID.ToString()), Logging.LogPriority.Error);
                    return Constants.TransactionResult.TransactionFailed;
                }

                string providerManagementWSURL = AppSettings.Get("ProviderManagementWSURL", Constants.WebServiceURI.ProviderManagement);
                string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");
                string client_secret = "";
                string secretName = "ProviderManagementWSPassword_OH_PNM_";
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
                string providerManagementWSPassword = client_secret;
                client_secret = "";
                secretName = "ProviderManagementWSPassword_2_OH_PNM_";
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
                string providerManagementWSPassword_2 = client_secret;

                string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

                Corp.Core.Libraries.ProviderManagementReference.EnrollRequestPayload enrollRequestPayload = new Corp.Core.Libraries.ProviderManagementReference.EnrollRequestPayload();
                enrollRequestPayload.ProviderInformation = new Corp.Core.Libraries.ProviderManagementReference.EnrollProviderInformation
                {
                    ProviderDemographics = ProviderManagementController.fillProviderDemographic(ds.Tables[6]),
                    ProviderAddress = ProviderManagementController.fillProviderAddress(ds.Tables[0]),
                    ProviderTaxonomyClassification = ProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                    ProviderType = ProviderManagementController.fillProviderType(ds.Tables[20]),
                    OwnerRelationship = ProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                    ProviderAffiliations = ProviderManagementController.fillAffiliationList(ds.Tables[2]),
                    ProviderAlternateIdentifiers = ProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                    ProviderEFTEnrollment = ProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                    ProviderLanguage = ProviderManagementController.fillLanguageList(ds.Tables[5], transactionID),
                    ProviderApplication = ProviderManagementController.fillenrollProviderApplication(ds.Tables[8]),
                    ProviderAttestations = ProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                    ProviderBusinessStatus = ProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                    ProviderCHOP = ProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                    ProviderContact = ProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                    ProviderManagedEmployees = ProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                    ProviderOwnerships = ProviderManagementController.fillProviderOwnershipList(ds.Tables[31]),
                    ProviderProgramAffiliations = ProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                    ProviderReviews = ProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                    ProviderServiceLocation = ProviderManagementController.fillEnrollProviderServiceLocation(ds),
                    ProviderServices = ProviderManagementController.fillProviderServicesListServices(ds.Tables[18])
                };

                Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[] imq = new Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[1];
                imq[0] = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber.ALL;
                Corp.Core.Libraries.ProviderManagementReference.MessageHeader msgHeader = new Corp.Core.Libraries.ProviderManagementReference.MessageHeader();
                msgHeader.BusinessFlow = "EnrollProvider";
                msgHeader.StateCode = "OH";
                msgHeader.RequestorSystem = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderRequestorSystem.PNM;
                msgHeader.ModuleTransactionId = transactionID.ToString();
                msgHeader.SubscriberSystem = imq;
                msgHeader.AdditionalModuleTransactionId = string.Empty;
                msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                msgHeader.SITransactionKey = sitTransactionKey;

                Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest epiPM = new Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest();
                epiPM.MessageHeader = msgHeader;
                epiPM.Payload = enrollRequestPayload;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.ProviderManagementReference.enrollProviderRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, epiPM, emptyNs);
                string xml = stream2.ToString();

                string wsHeader = WssSecurityHeader(providerManagementWSUserName, providerManagementWSPassword);
                if (wsHeader == null)
                {
                    log.CreateLogEntry(string.Format("{0} error while creating the wsHeader", logHeader), Logging.LogPriority.Error);
                    return Constants.TransactionResult.TransactionFailed;
                }
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/providermanagement\">"
                    + Environment.NewLine + wsHeader + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);


                if (makeRequest)
                {
                    InfoAccessController.SaveProviderMgmtXMLRecord(transactionID, null, Constants.ProviderManagementRequestorSystems.PNM, Constants.ProviderManagementSubscriberSystems.ALL,
                    null, xmlDoc.InnerXml.ToString(), null, null, null, null, null, new Guid(Constants.appAdminUserId));

                    return makeWSRequestResponse(transactionID, xmlDoc, providerManagementWSURL, Constants.ProviderManagementServiceResponse.enrollProvider, providerManagementWSUserName,
                        providerManagementWSPassword, providerManagementWSPassword_2, Constants.ProviderManagementSoapAction.ProviderManagement, sitTransactionKey);
                }
                else
                {
                    return xmlDoc.InnerXml.ToString();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error_Exception, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Staging_Error_Exception, DateTime.Now, new Guid(Constants.appAdminUserId));
                return Constants.TransactionResult.TransactionFailed;
            }
        }



        public string providerManagementUpdateRequest(int transactionID, bool makeRequest = false)
        {
            makeRequest = false;
            string strCheck = AppSettings.Get("MakeWSRequestCallToSI", "false");
            if (strCheck.Equals("true"))
            {
                makeRequest = true;
            }
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                DataSet ds = InfoAccessController.GetStagingProviderEnrollmentByTransactionID(transactionID);

                //Use this to perform dataFixes
                //PreProcessingDataFixes.PreProcessingDataFixesMain(log, transactionID);

                if (ds == null)
                {
                    log.CreateLogEntry(string.Format("{0} DataSet Returned null for transactionID:- {1}", logHeader, transactionID.ToString()), Logging.LogPriority.Error);
                    return Constants.TransactionResult.TransactionFailed;
                }

                string providerManagementWSURL = AppSettings.Get("ProviderManagementWSURL", Constants.WebServiceURI.ProviderManagement);
                string providerManagementWSUserName = AppSettings.Get("ProviderManagementWSUserName");

                string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

                Corp.Core.Libraries.ProviderManagementReference.UpdateRequestPayload updateRequestPayload = new Corp.Core.Libraries.ProviderManagementReference.UpdateRequestPayload();
                updateRequestPayload.ProviderInformation = new Corp.Core.Libraries.ProviderManagementReference.UpdateProviderInformation
                {
                    ProviderDemographics = ProviderManagementController.fillUpdateProviderDemographic(ds.Tables[6]),
                    ProviderAddress = ProviderManagementController.fillUpdateProviderAddress(ds.Tables[0]),
                    ProviderTaxonomyClassification = ProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                    ProviderType = ProviderManagementController.fillProviderType(ds.Tables[20]),
                    OwnerRelationship = ProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                    ProviderAffiliations = ProviderManagementController.fillAffiliationList(ds.Tables[2]),
                    ProviderAlternateIdentifiers = ProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                    ProviderEFTEnrollment = ProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                    ProviderLanguage = ProviderManagementController.fillLanguageList(ds.Tables[5], transactionID),
                    ProviderApplication = ProviderManagementController.fillUpdateProviderApplication(ds.Tables[8]),
                    ProviderAttestations = ProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                    ProviderBusinessStatus = ProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                    ProviderCHOP = ProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                    ProviderContact = ProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                    ProviderManagedEmployees = ProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                    ProviderOwnerships = ProviderManagementController.fillUpdateProviderOwnershipList(ds.Tables[31]),
                    ProviderProgramAffiliations = ProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                    ProviderReviews = ProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                    ProviderServiceLocation = ProviderManagementController.fillUpdateProviderServiceLocation(ds),
                    ProviderServices = ProviderManagementController.fillProviderServicesListServices(ds.Tables[18])
                };

                Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[] imq = new Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber[1];
                imq[0] = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderSubscriber.ALL;

                Corp.Core.Libraries.ProviderManagementReference.MessageHeader msgHeader = new Corp.Core.Libraries.ProviderManagementReference.MessageHeader();
                msgHeader.BusinessFlow = "UpdateProvider";
                msgHeader.StateCode = "OH";
                msgHeader.RequestorSystem = Corp.Core.Libraries.ProviderManagementReference.InqMessageHeaderRequestorSystem.PNM;
                msgHeader.ModuleTransactionId = transactionID.ToString();
                msgHeader.SubscriberSystem = imq;
                msgHeader.AdditionalModuleTransactionId = string.Empty;
                msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                msgHeader.SITransactionKey = sitTransactionKey;

                Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest epiPM = new Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest();
                epiPM.MessageHeader = msgHeader;
                epiPM.Payload = updateRequestPayload;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Corp.Core.Libraries.ProviderManagementReference.updateProviderRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, epiPM, emptyNs);
                string xml = stream2.ToString();
                string client_secret = "";
                string secretName = "ProviderManagementWSPassword_OH_PNM_";
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
                string wsHeader = WssSecurityHeader(providerManagementWSUserName, client_secret);
                if (wsHeader == null)
                {
                    log.CreateLogEntry(string.Format("{0} error while creating the wsHeader", logHeader), Logging.LogPriority.Error);
                    return Constants.TransactionResult.TransactionFailed;
                }
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/providermanagement\">"
                    + Environment.NewLine + wsHeader + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                string client_secret_2 = "";
                secretName = "ProviderManagementWSPassword_2_OH_PNM_";
                try
                {
                    var secret = new AmazonSecretsManager(AppSettings.Get("SecretsRegion"));
                    string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                    var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                    secretResult.Wait();

                    client_secret_2 = secretResult.Result[String.Concat(secretName, environmentName)];
                }
                catch (Exception ex)
                {
                    client_secret_2 = "NO SECRET CONNECTIVITY";
                    client_secret_2 += " ~|~ ";
                    client_secret_2 += AppSettings.Get("SecretsRegion");
                    client_secret_2 += " ~|~ ";
                    client_secret_2 += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                    client_secret_2 += " ~|~ ";
                    client_secret_2 += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                    client_secret_2 += " ~|~ ";
                    client_secret_2 += ex.Message;

                    log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
                }
                if (makeRequest)
                {
                    InfoAccessController.SaveProviderMgmtXMLRecord(transactionID, null, Constants.ProviderManagementRequestorSystems.PNM, Constants.ProviderManagementSubscriberSystems.ALL,
                    null, xmlDoc.InnerXml.ToString(), null, null, null, null, null, new Guid(Constants.appAdminUserId));

                    return makeWSRequestResponse(transactionID, xmlDoc, providerManagementWSURL, Constants.ProviderManagementServiceResponse.updateProvider, providerManagementWSUserName,
                        client_secret, client_secret_2, Constants.ProviderManagementSoapAction.UpdateProviderManagement, sitTransactionKey);
                }
                else
                {
                    return xmlDoc.InnerXml.ToString();
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error_Exception, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Staging_Error_Exception, DateTime.Now, new Guid(Constants.appAdminUserId));
                return Constants.TransactionResult.TransactionFailed;
            }
        }



        public string makeWSRequestResponse(int transactionID, XmlDocument xmlDoc, string providerManagementWSURL, string service, string providerManagementWSUserName, string providerManagementWSPassword,
          string providerManagementWSPassword_2, string soapAction, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                string providerManagementWSCertificateName = AppSettings.Get("ProviderManagementWSCertificateName");
                string providerManagementWSHost = AppSettings.Get("SIWebServiceHost");

                string ModuleTransactionId = string.Empty;
                string SITransactionKey = string.Empty;
                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                CertUtil _certs = new CertUtil();
                X509Certificate2 clientCertificate = _certs.GetCertificateByName(providerManagementWSCertificateName);
                string user = providerManagementWSUserName;
                string passcode = providerManagementWSPassword_2;
                Uri apiUrl = new Uri(providerManagementWSURL);
                WebRequest pmRequest = HttpWebRequest.Create(providerManagementWSURL);
                HttpWebRequest pmHttpRequest = (HttpWebRequest)pmRequest;
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlDoc.InnerXml.ToString());
                pmHttpRequest.ContentType = "text/xml; charset=utf-8";

                pmHttpRequest.KeepAlive = true;
                pmHttpRequest.Method = "POST";
                pmHttpRequest.SendChunked = true;
                pmHttpRequest.UserAgent = ".NET Framework";
                pmHttpRequest.Host = providerManagementWSHost;
                pmHttpRequest.ClientCertificates.Add(clientCertificate);
                String encoded = System.Convert.ToBase64String(Encoding.ASCII.GetBytes(user + ":" + passcode));
                pmHttpRequest.Headers[HttpRequestHeader.Authorization] = string.Format("Basic {0}", encoded);
                pmHttpRequest.Headers.Add("SOAPAction", soapAction);
                Stream requestStream = pmHttpRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                string response = "";

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

                try
                {
                    if (service.Equals(Constants.ProviderManagementServiceResponse.enrollProvider))
                    {
                        DeserializeSoapEnrollResponse(response, service, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
                    }
                    else
                    {
                        DeserializeSoapUpdateResponse(response, service, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
                    }

                }
                catch (Exception ex)
                {
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                }
                InfoAccessController.SaveSoapResponseCodeException(transactionID, SITransactionKey, ResponseCode,
                    ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId),
                    response.ToString(), pnmTransactionKey);

                //log is temporary
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, xmlDoc.InnerXml.ToString()), Logging.LogPriority.Debug);

                if (ResponseCode.Equals(Constants.ResponseCodes.MITS_SI_SUCCESS_ACK))
                {
                    return Constants.TransactionResult.TransactionPassed;
                }
                else
                {
                    return Constants.TransactionResult.TransactionFailed;
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
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        processResponse(ex, service, transactionID, pnmTransactionKey);
                        return Constants.TransactionResult.TransactionFailed;
                    }
                    else
                    {
                        // no http status code available
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        return Constants.TransactionResult.TransactionFailed;
                    }
                }
                else
                {
                    // no http status code available
                    log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                    return Constants.TransactionResult.TransactionFailed;
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error_Exception, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error_Exception, DateTime.Now, new Guid(Constants.appAdminUserId));
                return Constants.TransactionResult.TransactionFailed;
            }
        }


        public string WssSecurityHeader(string userName, string password)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
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
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
                return null;
            }
        }

        public void processResponse(WebException exResp, string serviceName, int transactionID, string pnmTransactionKey)
        {
            var resp = (HttpWebResponse)exResp.Response;

            switch (((HttpWebResponse)exResp.Response).StatusCode)
            {
                case (HttpStatusCode.BadRequest): // 404
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                    processBadRequest(exResp, serviceName, transactionID, pnmTransactionKey);
                    break;

                case HttpStatusCode.InternalServerError: // 503
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                    processInternalServerError(exResp, transactionID, pnmTransactionKey);
                    break;
            }
        }

        public void DeserializeSoapEnrollResponse(string respBody, string serviceName, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
    out string ResponseMessage, out string ResponseType, out string ModuleTransactionId)
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
                var xDoc = XDocument.Parse(respBody);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals(serviceName));

                var serializer = new XmlSerializer(typeof(SoapResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                SoapResponse obj = (SoapResponse)result;
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

        public void DeserializeSoapUpdateResponse(string respBody, string serviceName, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
            out string ResponseMessage, out string ResponseType, out string ModuleTransactionId)
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
                var xDoc = XDocument.Parse(respBody);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals(serviceName));

                var serializer = new XmlSerializer(typeof(SoapUpdateResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                SoapUpdateResponse obj = (SoapUpdateResponse)result;
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

        public void processBadRequest(WebException exResp, string serviceName, int transactionID, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
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
                    DeserializeSoapEnrollResponse(respBody, serviceName, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
                }
                else
                {
                    DeserializeSoapUpdateResponse(respBody, serviceName, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveSoapResponseCodeException(transactionID, SITransactionKey, ResponseCode,
                ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId),
                respBody.ToString(), pnmTransactionKey);
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

    }
}

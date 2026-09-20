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
    public class PartialProviderRequestResponse
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

        public PartialProviderRequestResponse()
        {
            ThreadId = Guid.NewGuid();
        }

        private Logging log = null;

        public PartialProviderManagementReference.InqMessageHeaderSubscriber[] sendToSubscriberService(string sendTo)
        {
            PartialProviderManagementReference.InqMessageHeaderSubscriber[] imq = new PartialProviderManagementReference.InqMessageHeaderSubscriber[1];
            
            switch (sendTo)
            {
                case Constants.TransactionTypeValues.PSM_Partial:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                    return imq;
                case Constants.TransactionTypeValues.PSM_FULL:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                    return imq;
                case Constants.TransactionTypeValues.PCW_Partial:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                    return imq;
                case Constants.TransactionTypeValues.PCW_Full:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                    return imq;
                case Constants.PartialProviderSubscriberSystems.PSM:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PSM;
                    return imq;
                case Constants.PartialProviderSubscriberSystems.PCW:
                    imq[0] = PartialProviderManagementReference.InqMessageHeaderSubscriber.PCW;
                    return imq;
            }
            return null;
        }

        public string partialProviderManagementSubmitRequest(int transactionID, string sendTo, bool makeRequest = false, bool? ReqRes = true)
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
                string partialProviderManagementWSURL = AppSettings.Get("PartialProviderManagementWSURL", Constants.WebServiceURI.PartialProviderManagement);
                string soapAction = Constants.ProviderManagementSoapAction.PartialProviderManagement;

                if (ReqRes == false)
                {
                    partialProviderManagementWSURL = AppSettings.Get("PartialProviderManagementPubSubWSURL", Constants.WebServiceURI.PartialProviderPubSubManagement);
                    soapAction = Constants.ProviderManagementSoapAction.PartialProviderManagementPubSub;
                }

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
                PartialProviderManagementReference.InqMessageHeaderSubscriber[] subscriberSystem = sendToSubscriberService(sendTo);
                

                if (subscriberSystem == null)
                {
                    log.CreateLogEntry(string.Format("{0} Incorrect Subscriber System assigned for transactionID:- {1}", logHeader, transactionID.ToString()), Logging.LogPriority.Error);
                    return Constants.TransactionResult.TransactionFailed;
                }

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;


                PartialProviderManagementReference.SubmitPartialProviderRequestPayload enSubmitPartialProviderRequestPayload = new PartialProviderManagementReference.SubmitPartialProviderRequestPayload();
                enSubmitPartialProviderRequestPayload.ProviderInformation = new PartialProviderManagementReference.SubmitPartialProviderInformation
                {
                    ProviderDemographics = PartialProviderManagementController.fillProviderDemographic(ds.Tables[6]),
                    ProviderApplication = PartialProviderManagementController.fillenrollProviderApplication(ds.Tables[8]),
                    ProviderAddress = PartialProviderManagementController.fillProviderAddress(ds.Tables[0]),
                    ProviderTaxonomyClassification = PartialProviderManagementController.fillTaxonomyClassification(ds.Tables[19]),
                    ProviderType = PartialProviderManagementController.fillProviderType(ds.Tables[20]),
                    OwnerRelationship = PartialProviderManagementController.fillOwnerRelationshipLists(ds.Tables[1]),
                    ProviderAffiliations = PartialProviderManagementController.fillAffiliationList(ds.Tables[2]),
                    ProviderAlternateIdentifiers = PartialProviderManagementController.fillAlternateIdList(ds.Tables[3]),
                    ProviderEFTEnrollment = PartialProviderManagementController.fillEFTEnrollmentList(ds.Tables[4]),
                    ProviderLanguage = PartialProviderManagementController.fillLanguageList(ds.Tables[5]),

                    ProviderAttestations = PartialProviderManagementController.fillProviderAttestationsListAttestations(ds.Tables[9]),
                    ProviderBusinessStatus = PartialProviderManagementController.fillProviderBusinessStatusListBusinessStatus(ds.Tables[10]),
                    ProviderCHOP = PartialProviderManagementController.fillProviderCHOPListCHOP(ds.Tables[11]),
                    ProviderContact = PartialProviderManagementController.fillProviderContactListContact(ds.Tables[12]),
                    ProviderManagedEmployees = PartialProviderManagementController.fillProviderManagedEmployeesListManagedEmployee(ds.Tables[13]),
                    ProviderOwnerships = PartialProviderManagementController.fillProviderOwnershipList(ds.Tables[31]),
                    ProviderProgramAffiliations = PartialProviderManagementController.fillProgramAffiliationsListProgramAffiliation(ds.Tables[15]),
                    ProviderReviews = PartialProviderManagementController.fillProviderReviewsListReview(ds.Tables[16]),
                    ProviderServiceLocation = PartialProviderManagementController.fillEnrollProviderServiceLocation(ds),
                    ProviderServices = PartialProviderManagementController.fillProviderServicesListServices(ds.Tables[18]),
                    ProviderDocuments = PartialProviderManagementController.fillPartialProviderDocumentsList(ds.Tables[7])
                };

                

                PartialProviderManagementReference.MessageHeader msgHeader = new PartialProviderManagementReference.MessageHeader();
                msgHeader.BusinessFlow = "SubmitPartialProvider";
                msgHeader.StateCode = "OH";
                msgHeader.RequestorSystem = PartialProviderManagementReference.InqMessageHeaderRequestorSystem.PNM;
                msgHeader.ModuleTransactionId = transactionID.ToString();
                msgHeader.SubscriberSystem = subscriberSystem;
                msgHeader.AdditionalModuleTransactionId = string.Empty;
                msgHeader.RequestTimestamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);
                //msgHeader.SITransactionKey = sitTransactionKey;

                PartialProviderManagementReference.submitPartialProviderRequest epiPM = new PartialProviderManagementReference.submitPartialProviderRequest();
                epiPM.MessageHeader = msgHeader;
                epiPM.Payload = enSubmitPartialProviderRequestPayload;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(PartialProviderManagementReference.submitPartialProviderRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, epiPM, emptyNs);
                string xml = stream2.ToString();
                xml = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns=\"http://mes.gov/partialprovidermanagement\">"
                    + Environment.NewLine + wssSecurityHeader(providerManagementWSUserName, providerManagementWSPassword) + Environment.NewLine + "<soapenv:Body>" +
                    Environment.NewLine + xml;
                xml = xml + Environment.NewLine + "</soapenv:Body>" + Environment.NewLine + "</soapenv:Envelope>";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                if (makeRequest)
                {
                    InfoAccessController.SaveProviderMgmtXMLRecord(transactionID, null, Constants.ProviderManagementRequestorSystems.PNM, subscriberSystem[0].ToString(),
                    null, xmlDoc.InnerXml.ToString(), null, null, null, null, null, new Guid(Constants.appAdminUserId));

                    return makeWSRequestResponse(transactionID, xmlDoc, partialProviderManagementWSURL, Constants.PartialProviderManagementServiceResponse.submitPartialProvider, providerManagementWSUserName, 
                        providerManagementWSPassword, providerManagementWSPassword_2, soapAction, sitTransactionKey, ReqRes);
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
            string providerManagementWSPassword_2, string soapAction, string pnmTransactionKey, bool? ReqRes)
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
                ResponseElement[] resp = new ResponseElement[] { };

                string ResponseCode = string.Empty;
                string ResponseDetails = string.Empty;
                string ResponseMessage = string.Empty;
                string ResponseType = string.Empty;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
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
                    if (ReqRes == true)
                    {
                        DeserializeSoapPartialResponse(response, service, out SITransactionKey, out ModuleTransactionId, out resp);
                    }
                    else
                    {
                        DeserializeSoapPubSubPartialResponse(response, service, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
                    }
                }
                catch (Exception ex)
                {
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                }

                if (ReqRes == true)
                {
                    InfoAccessController.SaveSoapResponseCodePPException(transactionID, SITransactionKey, DateTime.Now, new Guid(Constants.appAdminUserId),
                        response.ToString(), resp, pnmTransactionKey);
                    //log is temporary
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, xmlDoc.InnerXml.ToString()), Logging.LogPriority.Debug);

                    bool containsFailure = resp.Any(p => p.ResponseType == Constants.ResponseTypes.SI_failue);
                    bool isSuccess = resp.Any(p => p.ResponseCode == Constants.ResponseCodes.WAIVER_SI_SUCCESS || p.ResponseCode == Constants.ResponseCodes.WAIVER_SI_SUCCESS_ACK);

                    if (!containsFailure && isSuccess)
                    {
                        return Constants.TransactionResult.TransactionPassed;
                    }
                    else
                    {
                        return Constants.TransactionResult.TransactionFailed;
                    }
                }
                else
                {
                    InfoAccessController.SaveSoapResponseCodeException(transactionID, SITransactionKey, ResponseCode,
                    ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId),
                    response.ToString(), pnmTransactionKey);
                    //log is temporary
                    log.CreateLogEntry(string.Format("{0} {1}", logHeader, xmlDoc.InnerXml.ToString()), Logging.LogPriority.Debug);

                    if (ResponseCode.Equals(Constants.ResponseCodes.WAIVER_SI_SUCCESS) || ResponseCode.Equals(Constants.ResponseCodes.WAIVER_SI_SUCCESS_ACK))
                    {
                        return Constants.TransactionResult.TransactionPassed;
                    }
                    else
                    {
                        return Constants.TransactionResult.TransactionFailed;
                    }
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
                        processResponse(ex, service, transactionID, pnmTransactionKey, ReqRes);
                        //log is temporary
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        return Constants.TransactionResult.TransactionFailed;                      
                    }
                    else
                    {
                        // no http status code available
                        //log is temporary
                        log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                        if (ReqRes == true)
                        {
                            InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                            return Constants.TransactionResult.TransactionFailed;
                        }
                        else
                        {
                            InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                            return Constants.TransactionResult.TransactionFailed;
                        }
                        
                    }
                }
                else
                {
                    // no http status code available
                    //log is temporary
                    log.CreateLogEntry(string.Format("{0} {1} {2}", logHeader, ex.ToString(), xmlDoc.InnerXml.ToString()), Logging.LogPriority.Error);
                    if (ReqRes == true)
                    {
                        InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        return Constants.TransactionResult.TransactionFailed;
                    }
                    else
                    {
                        InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        return Constants.TransactionResult.TransactionFailed;
                    }
                    
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
                if (ReqRes == true)
                {
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error_Exception, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error_Exception, DateTime.Now, new Guid(Constants.appAdminUserId));
                    return Constants.TransactionResult.TransactionFailed;
                }
                else
                {
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error_Exception, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error_Exception, DateTime.Now, new Guid(Constants.appAdminUserId));
                    return Constants.TransactionResult.TransactionFailed;
                }
                
            }
        }

        public string wssSecurityHeader(string providerManagementWSUserName, string providerManagementWSPassword)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                UsernameToken usernameTokenSection = new UsernameToken(providerManagementWSUserName, providerManagementWSPassword, PasswordOption.SendPlainText);
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


        public void processResponse(WebException exResp, string serviceName, int transactionID, string pnmTransactionKey, bool? ReqRes = true)
        {
            var resp = (HttpWebResponse)exResp.Response;

            switch (((HttpWebResponse)exResp.Response).StatusCode)
            {
                case (HttpStatusCode.BadRequest): // 404
                    if (ReqRes == true)
                    {
                        InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        processBadRequest(exResp, serviceName, transactionID, pnmTransactionKey);
                    }
                    else
                    {
                        InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                        processBadRequestPubSub(exResp, serviceName, transactionID, pnmTransactionKey);
                    }                    
                    break;

                case HttpStatusCode.InternalServerError: // 503
                    InfoAccessController.UpdateTransactionQueueSIResponse(transactionID, Constants.ResponseCodes.PNM_Default_Error, Constants.ResponseTypes.PNM_Default_Error, Constants.ResponseMessage.PNM_Default_Error, DateTime.Now, new Guid(Constants.appAdminUserId));
                    processInternalServerError(exResp, transactionID, pnmTransactionKey);
                    break;
            }
        }

        public void processBadRequestPubSub(WebException exResp, string serviceName, int transactionID, string pnmTransactionKey)
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
                DeserializeSoapPubSubPartialResponse(respBody, serviceName, out SITransactionKey, out ResponseCode, out ResponseDetails, out ResponseMessage,
                        out ResponseType, out ModuleTransactionId);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveSoapResponseCodeException(transactionID, SITransactionKey, ResponseCode,
                ResponseDetails, ResponseMessage, ResponseType, DateTime.Now, new Guid(Constants.appAdminUserId),
                respBody.ToString(), pnmTransactionKey);
        }

        public void processBadRequest(WebException exResp, string serviceName, int transactionID, string pnmTransactionKey)
        {
            string logHeader = string.Format("Error for transactionID- {0}:", transactionID.ToString());
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            string ModuleTransactionId = string.Empty;
            string SITransactionKey = string.Empty;
            ResponseElement[] resp = null;

            log = new Logging(this.ThreadId, logMsg);

            string respBody = new StreamReader(exResp.Response.GetResponseStream()).ReadToEnd();
            try
            {
                DeserializeSoapPartialResponse(respBody, serviceName, out SITransactionKey, out ModuleTransactionId, out resp);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0}", ex.ToString()), Logging.LogPriority.Error);
            }
            InfoAccessController.SaveSoapResponseCodePPException(transactionID, SITransactionKey, DateTime.Now, new Guid(Constants.appAdminUserId),
                respBody.ToString(), resp, pnmTransactionKey);
        }

        public void DeserializeSoapPubSubPartialResponse(string respBody, string serviceName, out string SITransactionKey, out string ResponseCode, out string ResponseDetails,
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

                var serializer = new XmlSerializer(typeof(PartialPubSubSoapResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                PartialPubSubSoapResponse obj = (PartialPubSubSoapResponse)result;
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

        public void DeserializeSoapPartialResponse(string respBody, string serviceName, out string SITransactionKey, out string ModuleTransactionId, out ResponseElement[] resp)
        {
            object result;
            ModuleTransactionId = string.Empty;
            SITransactionKey = string.Empty;
            resp = null;

            try
            {
                var xDoc = XDocument.Parse(respBody);
                var xLoginResult = xDoc.Root.Descendants().FirstOrDefault(d => d.Name.LocalName.Equals(serviceName));

                var serializer = new XmlSerializer(typeof(PartialSoapResponse));
                using (var reader = new StringReader(xLoginResult.ToString()))
                {
                    result = serializer.Deserialize(reader);
                }
                PartialSoapResponse obj = (PartialSoapResponse)result;
                SITransactionKey = obj.SITransactionKey;
                resp = obj.ResponseElement;
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

    }
}

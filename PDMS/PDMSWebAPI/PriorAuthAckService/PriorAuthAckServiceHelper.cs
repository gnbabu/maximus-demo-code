using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.PriorAuthAckService
{
    public class PriorAuthAckServiceHelper
    {
        public static void saveRequestResponse(Logging log, Guid requestID, string param1, string param2, string responseStatus, string responsePayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                if (responseStatus.Equals("Success"))
                {
                    parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, param1, true));
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, param1, true));
                    parameters.Add(SqlParms.CreateParameter("ERROR_DESCP", DbType.String, param2, true));
                }
                parameters.Add(SqlParms.CreateParameter("RESPONSE_STATUS", DbType.String, responseStatus, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_PAYLOAD", DbType.String, responsePayload, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.PriorAuthService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_PRIORAUTH_ACK_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static ReceivePriorAuthUpdatesResponse priorAuthAUDataRespError(Logging log, int operationID, string errorCode, string errorDescription, Guid requestID
    , ReceivePriorAuthUpdatesRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;

                ResponseHeaderType rt = new ResponseHeaderType();
                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.ResponseCode = errorCode;
                rt.ResponseTypeSpecified = true;
                rt.ResponseType = ResponseHeaderTypeResponseType.FAILURE;
                rt.ResponseMessage = errorDescription;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;

                ReceivePriorAuthUpdatesResponse rtn = new ReceivePriorAuthUpdatesResponse();
                rtn.ResponseHeader = rt;
                responsePayload = priorAuthAckParseXMLToStringRes(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return priorAuthAUResponseGenericError(log, errorCode, errorDescription, request);
            }
        }

        public static ReceivePriorAuthUpdatesResponse priorAuthAUResponseGenericError(Logging log, string errorCode, string errorDescription,
    ReceivePriorAuthUpdatesRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;

            ResponseHeaderType rt = new ResponseHeaderType();
            rt.SITransactionKey = SITransactionKey;
            rt.ModuleTransactionId = moduleTransactionID;
            rt.AdditionalModuleTransactionId = addnModuleTransactionID;
            rt.ResponseCode = errorCode;
            rt.ResponseTypeSpecified = true;
            rt.ResponseType = ResponseHeaderTypeResponseType.FAILURE;
            rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
            rt.ResponseMessage = errorDescription;

            ReceivePriorAuthUpdatesResponse rtn = new ReceivePriorAuthUpdatesResponse();
            rtn.ResponseHeader = rt;
            return rtn;
        }


        public static ReceivePriorAuthUpdatesResponse addUpdateSuccessResponse(Logging log, Guid requestID,
            int operationID, ReceivePriorAuthUpdatesRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;

                ResponseHeaderType rht = new ResponseHeaderType();
                rht.SITransactionKey = SITransactionKey;
                rht.ModuleTransactionId = moduleTransactionID;
                rht.AdditionalModuleTransactionId = addnModuleTransactionID;
                rht.ResponseTypeSpecified = true;
                rht.ResponseType = ResponseHeaderTypeResponseType.SUCCESS;
                rht.ResponseCode = "1000";
                rht.ResponseMessage = "Request Received and Processed Successfully";
                ReceivePriorAuthUpdatesResponse rtn = new ReceivePriorAuthUpdatesResponse();
                rtn.ResponseHeader = rht;
                responsePayload = priorAuthAckParseXMLToStringRes(log, rtn);

                saveRequestResponse(log, requestID, SITransactionKey, "Request Received and Processed Successfully", "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return priorAuthAUDataRespError(log, operationID, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public static void saveRequestResponse(Logging log, Guid requestID, int operationID, string BusinessFlow, string StateCode, string requestorSystem,
            string moduleTransactionID, string AdditionalModuleTransactionID, string SITransactionKey, string requestPayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_TYPE_ID", DbType.Int32, operationID, true));
                parameters.Add(SqlParms.CreateParameter("BUSINESS_FLOW", DbType.String, BusinessFlow, true));
                parameters.Add(SqlParms.CreateParameter("STATE_CODE", DbType.String, StateCode, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_ID", DbType.String, moduleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, AdditionalModuleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestPayload, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.PriorAuthService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_PRIORAUTH_ACK_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string priorAuthAckParseXMLToStringReq(Logging log, ReceivePriorAuthUpdatesRequest req)
        {
            try
            {

                XmlSerializer x = x = new XmlSerializer(typeof(ReceivePriorAuthUpdatesRequest));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string priorAuthAckParseXMLToStringRes(Logging log, ReceivePriorAuthUpdatesResponse req)
        {
            try
            {
                XmlSerializer x = x = new XmlSerializer(typeof(ReceivePriorAuthUpdatesResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }


        public static void saveTemplateDetails(Logging log, Dictionary<string, object> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, object> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value.ToString()))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure("usp_InsertDOCUMENTS_PRIOR_AUTH_SUBMISSION_278", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static void processTemplateBody(Logging log, Guid requestID, ReceivePriorAuthUpdatesRequest req)
        {
            try
            {
                if (req != null &&
                   req.PriorAuthResponse278 != null &&
                   req.PriorAuthResponse278.BHTContainter != null)
                {
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    string PriorAuthStatus = string.Empty;
                    string PriorAuthNo = string.Empty;
                    string PriorAuthDate = string.Empty;
                    string PatientTrackingNumber = string.Empty;

                    if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Prior Auth Status section. Started processing..", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E != null)
                        {
                            PriorAuthStatus = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR01_ActionCode?.ToString();
                            PriorAuthNo = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR02_ReviewIdentificationNumber?.ToString();
                        }



                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationIssueDate_2000E != null)
                        {
                            PriorAuthDate = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationIssueDate_2000E.DTP03_AccidentDate?.ToString();
                        }

                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E != null &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E.Count() > 0 &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E[0] != null)
                        {
                            PatientTrackingNumber = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientEventTrackingNumber_2000E[0].TRN02_PatientEventTraceNumber?.ToString();
                        }
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Prior Auth Status section. Processing completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string PriorAuthRDOB = string.Empty;
                    string LName = string.Empty;
                    string FName = string.Empty;

                    if (req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C != null && req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C != null)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Subscriber Name details section. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        if (req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberDemographicInformation_2000C != null)
                            PriorAuthRDOB = req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberDemographicInformation_2000C.DMG02_SubscriberBirthDate?.ToString();

                        if (req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C != null)
                        {
                            LName = req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C.NM103_UMOLastOrOrganizationName?.ToString();
                            FName = req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberName_2010C.NM104_UMOFirstName?.ToString();
                        }
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Subscriber Name details section. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string MedBillNo = string.Empty;
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched UMO Name details section. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    if (req.PriorAuthResponse278.BHTContainter.UMODetails_2000A != null &&
                        req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A != null)
                    {
                        MedBillNo = req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM109_UMOIdentifier?.ToString();
                    }

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched UMO Name details section. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string RespDT = string.Empty;
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Transaction Time. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    if (req.PriorAuthResponse278.BHTContainter.BeginningOfHierarchicalTransaction != null)
                    {
                        string RespDDate = req.PriorAuthResponse278.BHTContainter.BeginningOfHierarchicalTransaction.BHT04_TransactionSetCreationDate;
                        string RespDTime = req.PriorAuthResponse278.BHTContainter.BeginningOfHierarchicalTransaction.BHT05_TransactionSetCreationTime;
                        RespDT = RespDDate + " " + RespDTime;
                    }

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Transaction Time. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Administrative Reference Number. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    string PriorICN = string.Empty;
                    if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E != null)
                    {
                        PriorICN = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.AdministrativeReferenceNumber_2000E?.REF02_RequesterSupplementalIdentification;
                    }

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Administrative Reference Number. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string ReqLName = string.Empty;
                    string ReqFName = string.Empty;
                    string ReqProvNPI = string.Empty;
                    string ReqProvMedID = string.Empty;

                    if (req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B != null &&
                        req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                        req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B != null)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Requester Name details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        ReqLName = req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM103_UMOLastOrOrganizationName?.ToString();
                        ReqFName = req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM104_UMOFirstName?.ToString();
                        ReqProvNPI = req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterName_2010B.NM109_UMOIdentifier?.ToString();

                        if (req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B != null &&
                            req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B.Count() > 0 &&
                            req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B[0] != null)
                        {
                            if (!string.IsNullOrEmpty(req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B[0].REF01_ReferenceIdentificationQualifier) &&
                                req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B[0].REF01_ReferenceIdentificationQualifier.ToUpper() == "G2")
                            {
                                ReqProvMedID = req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterSupplementalIdentification_2010B[0].REF04_ReferenceIdentifier;
                            }
                        }
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Requester Name details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string PayerFName = string.Empty;
                    string PayerLName = string.Empty;
                    string PayerID = string.Empty;
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Payer details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    if (req.PriorAuthResponse278.BHTContainter.UMODetails_2000A != null &&
                        req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A != null)
                    {
                        PayerFName = req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM104_UMOFirstName?.ToString();
                        PayerLName = req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM103_UMOLastOrOrganizationName?.ToString();
                        PayerID = req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMOName_2010A.NM109_UMOIdentifier?.ToString();
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Payer details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string PayerEmail = string.Empty;
                    string PayerContactNo = string.Empty;

                    if (!string.IsNullOrEmpty(PayerID))
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Payer Email Info details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, PayerID, true));
                        DataSet DSRegAddressDetails = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSDetails", param, "DSRegAddressDetails");
                        if (DSRegAddressDetails != null &&
                           DSRegAddressDetails.Tables != null &&
                           DSRegAddressDetails.Tables.Count > 0 &&
                           DSRegAddressDetails.Tables[0].Rows != null &&
                           DSRegAddressDetails.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = DSRegAddressDetails.Tables[0].Rows[0];
                            PayerEmail = row["EMAIL1"] != null ? row["EMAIL1"].ToString() : string.Empty;
                            PayerContactNo = row["PHONE1"] != null ? row["PHONE1"].ToString() : string.Empty;
                        }
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Payer Email Info details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string PriorAuthDecisionReason = string.Empty;
                    string PriorAuthEffectiveDate = string.Empty;
                    string ReviewerNote = string.Empty;
                    string PriorAuthExpirationDate = string.Empty;

                    if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Patient Event details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E != null)
                        {
                            PriorAuthDecisionReason = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.HealthCareServicesReview_2000E.HCR03_ReviewDecisionReasonCode;
                        }

                        PriorAuthEffectiveDate = string.Empty;

                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E != null &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E?.DTP01_DateTimeQualifier != null &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E?.DTP01_DateTimeQualifier == "7")
                        {
                            PriorAuthEffectiveDate = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationEffectiveDate_2000E?.DTP02_DateTimePeriodFormatQualifier; // Need to confirm
                        }

                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E != null)
                            ReviewerNote = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.MessageText_2000E.MSG01_FreeFormMessageText;

                        PriorAuthExpirationDate = string.Empty;

                        if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E != null &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E.DTP01_DateTimeQualifier != null &&
                            req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E?.DTP01_DateTimeQualifier == "36")
                        {
                            PriorAuthExpirationDate = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.CertificationExpirationDate_2000E?.DTP02_DateTimePeriodFormatQualifier; // Need to confirm
                        }
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Patient Event details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string ProcedureCode = string.Empty;
                    string RequestedUnits = string.Empty;
                    string RequestedFDOS = string.Empty;
                    string RequestedTDOS = string.Empty;

                    if (req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F != null)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Service details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        if (req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ProfessionalService_2000F != null)
                        {
                            ProcedureCode = req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ProfessionalService_2000F.SV1012_ProcedureCode;
                            RequestedUnits = req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ProfessionalService_2000F.SV1014_ProcedureModifier;
                        }

                        if (req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceDate_2000F != null)
                        {
                            RequestedFDOS = req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceDate_2000F.DTP03_AccidentDate; // Need to confirm
                            RequestedTDOS = req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceDate_2000F.DTP03_AccidentDate; // Need to confirm
                        }

                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Service details. Processing completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }


                    string RejectionInformationRejectionType = string.Empty;
                    string RejectionInformationRejectionReason = string.Empty;
                    string RejectionInformationFollowupAction = string.Empty;
                    string RejectionInformationReviewerNote = string.Empty;

                    DataTable dataTable = new DataTable();
                    dataTable.Columns.AddRange(new DataColumn[5] { new DataColumn("Line",typeof(int)),
                                                               new DataColumn("Rejection Type",typeof(string)),
                                                               new DataColumn("Rejection Reason",typeof(string)),
                                                               new DataColumn("Follow-up Action",typeof(string)),
                                                               new DataColumn("Reviewer's Note",typeof(string))});

                    int count = 1;
                    var umoRejectionDetails2000A = (req.PriorAuthResponse278.BHTContainter.UMODetails_2000A != null &&
                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.RequestValidation_2000A != null &&
                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.RequestValidation_2000A.Count() > 0) ?

                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.RequestValidation_2000A : null;

                    var umoRejectionDetails2010A = (req.PriorAuthResponse278.BHTContainter.UMODetails_2000A != null &&
                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A != null &&
                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMORequestValidation_2010A != null &&
                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMORequestValidation_2010A.Count() > 0) ?

                                                    req.PriorAuthResponse278.BHTContainter.UMODetails_2000A.UMONameDetails_2010A.UMORequestValidation_2010A : null;

                    var RequesterNameRejectionType2010B = (req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B != null &&
                                                           req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B != null &&
                                                           req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterRequestValidation_2010B != null &&
                                                           req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterRequestValidation_2010B.Count() > 0) ?

                                                    req.PriorAuthResponse278.BHTContainter.RequesterDetails_2000B.RequesterNameDetails_2010B.RequesterRequestValidation_2010B : null;

                    var subscriberNameRejection2010C = (req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C != null &&
                                                   req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C != null &&
                                                   req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberRequestValidation_2000C != null &&
                                                   req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberRequestValidation_2000C.Count() > 0) ?

                                                   req.PriorAuthResponse278.BHTContainter.SubscriberDetails_2000C.SubscriberNameDetails_2010C.SubscriberRequestValidation_2000C : null;

                    var patientEventRejectionType2000E = (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null &&
                                                          req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientRequestValidation_2000E != null &&
                                                          req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientRequestValidation_2000E.Count() > 0) ?

                                                          req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.PatientRequestValidation_2000E : null;

                    var serviceDetailsRejection2000F = (req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F != null &&
                                                    req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceRequestValidation_2000F != null &&
                                                    req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceRequestValidation_2000F.Count() > 0) ?

                                                    req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.ServiceRequestValidation_2000F : null;

                    string RejectionReasonCode = string.Empty;
                    string RejectionReasonDescription = string.Empty;

                    string FolloupActionCode = string.Empty;
                    string FollowupActionDescription = string.Empty;
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Rejection Details 2000A. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (umoRejectionDetails2000A != null)
                    {
                        foreach (var item in umoRejectionDetails2000A)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            dataTable.Rows.Add(count++, "Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                        }
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Rejection Details 2000A. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01


                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Rejection Details 2010A. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    if (umoRejectionDetails2010A != null)
                    {
                        foreach (var item in umoRejectionDetails2010A)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            dataTable.Rows.Add(count++, "UMO Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                        }
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Rejection Details 2010A. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Requester Name Rejection Details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (RequesterNameRejectionType2010B != null)
                    {
                        foreach (var item in RequesterNameRejectionType2010B)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            dataTable.Rows.Add(count++, "Requester Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                        }
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Requester Name Rejection Details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Subscriber Name Rejection Details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (subscriberNameRejection2010C != null)
                    {
                        foreach (var item in subscriberNameRejection2010C)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            dataTable.Rows.Add(count++, "Subscriber Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                        }
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Subscriber Name Rejection Details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Patient Event Rejection Details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (patientEventRejectionType2000E != null)
                    {
                        foreach (var item in patientEventRejectionType2000E)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            dataTable.Rows.Add(count++, "Patient Event Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                        }
                    }

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Patient Event Rejection Details. Processing Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E != null &&
                        req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.ProviderDetails_2010EA != null &&
                        req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.ProviderDetails_2010EA.Count() > 0)
                    {
                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Provider Event Details. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        var providerDetails_2010EAList = req.PriorAuthResponse278.BHTContainter.PatientEventDetails_2000E.ProviderDetails_2010EA;

                        if (providerDetails_2010EAList != null)
                        {
                            foreach (var item1 in providerDetails_2010EAList)
                            {
                                if (item1.PatientEventProviderRequestValidation_2010EA != null && item1.PatientEventProviderRequestValidation_2010EA.Count() > 0)
                                {
                                    var PatientEventProviderRequestValidation_2010EAList = item1.PatientEventProviderRequestValidation_2010EA;

                                    if (PatientEventProviderRequestValidation_2010EAList != null)
                                    {
                                        foreach (var item in PatientEventProviderRequestValidation_2010EAList)
                                        {
                                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                                            dataTable.Rows.Add(count++, "Patient Event Provider Request Validation", RejectionReasonDescription, FollowupActionDescription, string.Empty);
                                        }
                                    }
                                }
                            }
                        }

                        log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Provider Event Details. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Service Details Rejection. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    if (serviceDetailsRejection2000F != null)
                    {
                        foreach (var item in serviceDetailsRejection2000F)
                        {
                            RejectionReasonCode = item.AAA03_RejectReasonCode;
                            RejectionReasonDescription = string.IsNullOrEmpty(RejectionReasonCode) ? "" : GetRejectionReasonCodeDescription(RejectionReasonCode, log);

                            FolloupActionCode = item.AAA04_FollowUpActionCode;
                            FollowupActionDescription = string.IsNullOrEmpty(FolloupActionCode) ? "" : GetFollowUpActionCodeDescription(FolloupActionCode, log);

                            string servicelvlmsg = string.Empty;

                            if (req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F != null &&
                                req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.MessageText_2000F != null)
                            {
                                servicelvlmsg = req.PriorAuthResponse278.BHTContainter.ServiceDetails_2000F.MessageText_2000F.MSG01_FreeFormMessageText;
                            }

                            dataTable.Rows.Add(count++, "Service Request Validation", RejectionReasonDescription, FollowupActionDescription, servicelvlmsg);
                        }
                    }
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched Service Details Rejection. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string body = string.Empty;
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("ReqProvMedID", ReqProvMedID.ToString());
                    fields.Add("ReqProvNPI", ReqProvNPI);
                    fields.Add("ReqProvName", ReqLName + " " + ReqFName);

                    fields.Add("RespDT", RespDT);
                    fields.Add("PriorICN", PriorICN);
                    fields.Add("PayerContactName", PayerLName + " " + PayerFName);
                    fields.Add("PayerContactNo", PayerContactNo.ToString());

                    fields.Add("PayerID", PayerID);
                    fields.Add("PayerEmail", PayerEmail.ToString());


                    fields.Add("RecpName", LName + " " + FName);
                    fields.Add("MedBillNo", MedBillNo);
                    fields.Add("PtTNum", PatientTrackingNumber);
                    fields.Add("PriorAuthRDOB", PriorAuthRDOB);


                    fields.Add("PriorAuthNo", PriorAuthNo);
                    fields.Add("PriorAuthStatus", PriorAuthStatus);
                    fields.Add("PriorAuthDate", PriorAuthDate);

                    fields.Add("PriorAuthDecisionReason", PriorAuthDecisionReason);
                    fields.Add("PriorAuthEffectiveDate", PriorAuthEffectiveDate);
                    fields.Add("ReviewerNote", ReviewerNote);
                    fields.Add("PriorAuthExpirationDate", PriorAuthExpirationDate);

                    fields.Add("ProcedureCode", ProcedureCode);

                    fields.Add("RequestedUnits", RequestedUnits);
                    fields.Add("RequestedFDOS", RequestedFDOS);
                    fields.Add("RequestedTDOS", RequestedTDOS);
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched InitServiceDetails. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    fields.Add("ServiceDetails", initServiceDetails(log, dataTable));
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched InitServiceDetails. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched generateTemplateBody. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    body = generateTemplateBody(log, CON.ResponseFileTemplates.PA278, fields);

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched generateTemplateBody. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    parms.Add("PayerName", PayerLName + " " + PayerFName);
                    parms.Add("PatientTrackingNumber", PatientTrackingNumber);
                    parms.Add("ReportType", "".ToString());
                    parms.Add("StatusDate", DateTime.Now.ToString());

                    parms.Add("CREATE_DATE_TIME", DateTime.Now);
                    parms.Add("LAST_MODIFIED_USER", CON.WebApiServiceGuid.PriorAuthService);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                    parms.Add("Created_By_User", CON.WebApiServiceGuid.PriorAuthService);

                    parms.Add("Medicaid_ID", MedBillNo.ToString());
                    parms.Add("DOCUMENT_ID", "".ToString());
                    parms.Add("HTML_BODY", body);
                    parms.Add("REQUEST_ID", requestID);

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched saveTemplateDetails. Started Processing....", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    saveTemplateDetails(log, parms);

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Fetched saveTemplateDetails. Processing Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Finished.", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                }
                else
                {
                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): Error - Request/PriorAuthResponse278/BHTContainter is null", Logging.LogPriority.Error); // TO DO: Remove this once passes on INT01
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        private static string GetRejectionReasonCodeDescription(string reasonCode, Logging log)
        {
            string reasonDescription = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REJECT_REASON_CODE", DbType.String, reasonCode, true));
                DataSet dataSet = DataAccess.ExecuteStoredProcedure("usp_SelectRejectionCodeDescription", parameters, "ReasonCodeDetails");
                if (dataSet != null &&
                    dataSet.Tables != null &&
                    dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows != null &&
                    dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    reasonDescription = row["DESCRIPTION"] != null ? row["DESCRIPTION"].ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }

            return reasonDescription;
        }

        private static string GetFollowUpActionCodeDescription(string reasonCode, Logging log)
        {
            string reasonDescription = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FOLLOWUP_ACTION_CODE", DbType.String, reasonCode, true));
                DataSet dataSet = DataAccess.ExecuteStoredProcedure("usp_SelectFollowupActionCodeDescription", parameters, "FollowUpDetails");
                if (dataSet != null &&
                    dataSet.Tables != null &&
                    dataSet.Tables.Count > 0 &&
                    dataSet.Tables[0].Rows != null &&
                    dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];
                    reasonDescription = row["DESCRIPTION"] != null ? row["DESCRIPTION"].ToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }

            return reasonDescription;
        }


        public static string initServiceDetails(Logging log, DataTable dataTable)
        {
            //string serviceHeader = "<tr><td>Line</td><td>Rejection Type</td><td>Rejection Reason</td><td>Follow-up Action</td><td>Reviewer's Note</td></tr>";
            try
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): initServiceDetails Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                StringBuilder sb = new StringBuilder();

                //Adding HeaderRow.
                sb.Append("<tr>");
                foreach (DataColumn column in dataTable.Columns)
                {
                    sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>" + column.ColumnName + "</th>");
                }
                sb.Append("</tr>");


                //Adding DataRow.
                foreach (DataRow row in dataTable.Rows)
                {
                    sb.Append("<tr>");
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        sb.Append("<td style='width:100px;border: 1px solid #ccc'>" + row[column.ColumnName].ToString() + "</td>");
                    }
                    sb.Append("</tr>");
                }
                log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): initServiceDetails Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
            //return serviceHeader;
        }

        public static string generateTemplateBody(Logging log, string templateName, Dictionary<string, object> fields)
        {
            try
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): generateTemplateBody method Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                string path;
                string body = string.Empty;

                string templateActualPath = AppSettings.Get("TemplateFilePath", string.Empty) + @"\\PriorAuth278Template.txt";

#if DEBUG
                @templateActualPath = @"C:\inetpub\wwwroot\PDMS\OH_PNM_API_INT01\Documents\PriorAuth278Template.txt";
#endif

                log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): generateTemplateBody templateActualPath = " + templateActualPath, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                if (!string.IsNullOrEmpty(templateActualPath))
                {
                    fields.Add("FONTSIZE", "14pt");
                    fields.Add("DISPLAYOMR", "<br />");

                    TemplateEvaluator template = new TemplateEvaluator();

                    if (!string.IsNullOrEmpty(templateActualPath))
                    {
                        path = templateActualPath;
                    }
                    else
                    {
                        path = System.AppDomain.CurrentDomain.BaseDirectory;
                    }
                    template.Load(path);

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): generateTemplateBody - Template loaded", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));

                    log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): generateTemplateBody - Template body evaluated", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                }
                else
                {
                    throw new Exception("PriorAuthAckServiceHelpertemplateActualPath is empty");
                }
                log.CreateLogEntry("PriorAuthAckServiceHelper - Method: processTemplateBody(): generateTemplateBody method Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                return body;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PriorAuthAckServiceHelper" + string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }
    }
}
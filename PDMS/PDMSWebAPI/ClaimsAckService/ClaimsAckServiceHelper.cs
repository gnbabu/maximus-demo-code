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

namespace PDMSWebAPI.ClaimsAckService
{
    public static class ClaimsAckServiceHelper
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
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.ClaimsService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_CLAIMS_ACK_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static ClaimsAckServiceReference.claimAcknowledgementResponse claimAcknowledgementDataRespError(Logging log, string errorCode, string errorDescription, Guid requestID
    , ClaimsAckServiceReference.claimAcknowledgementRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;

                ClaimsAckServiceReference.ResponseHeaderType rt = new ClaimsAckServiceReference.ResponseHeaderType();
                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.ResponseCode = errorCode;
                rt.ResponseTypeSpecified = true;
                rt.ResponseType = ClaimsAckServiceReference.ResponseHeaderTypeResponseType.FAILURE;
                rt.ResponseMessage = errorDescription;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;

                ClaimsAckServiceReference.claimAcknowledgementResponse rtn = new ClaimsAckServiceReference.claimAcknowledgementResponse();
                rtn.ResponseHeader = rt;
                responsePayload = claimsAckParseXMLToStringRes(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return claimAcknowledgementResponseGenericError(log, errorCode, errorDescription, request);
            }
        }

        public static ClaimsAckServiceReference.claimAcknowledgementResponse claimAcknowledgementResponseGenericError(Logging log, string errorCode, string errorDescription,
    ClaimsAckServiceReference.claimAcknowledgementRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;

            ClaimsAckServiceReference.ResponseHeaderType rt = new ClaimsAckServiceReference.ResponseHeaderType();
            rt.SITransactionKey = SITransactionKey;
            rt.ModuleTransactionId = moduleTransactionID;
            rt.AdditionalModuleTransactionId = addnModuleTransactionID;
            rt.ResponseCode = errorCode;
            rt.ResponseTypeSpecified = true;
            rt.ResponseType = ClaimsAckServiceReference.ResponseHeaderTypeResponseType.FAILURE;
            rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
            rt.ResponseMessage = errorDescription;

            ClaimsAckServiceReference.claimAcknowledgementResponse rtn = new ClaimsAckServiceReference.claimAcknowledgementResponse();
            rtn.ResponseHeader = rt;
            return rtn;
        }


        public static ClaimsAckServiceReference.claimAcknowledgementResponse successResponse(Logging log, Guid requestID, ClaimsAckServiceReference.claimAcknowledgementRequest request)
        {
            string SITransactionKey = request.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;

                ClaimsAckServiceReference.ResponseHeaderType rht = new ClaimsAckServiceReference.ResponseHeaderType();
                rht.SITransactionKey = SITransactionKey;
                rht.ModuleTransactionId = moduleTransactionID;
                rht.AdditionalModuleTransactionId = addnModuleTransactionID;
                rht.ResponseTypeSpecified = true;
                rht.ResponseType = ClaimsAckServiceReference.ResponseHeaderTypeResponseType.SUCCESS;
                rht.ResponseCode = "1000";
                rht.ResponseMessage = "Request Received and Processed Successfully";
                ClaimsAckServiceReference.claimAcknowledgementResponse rtn = new ClaimsAckServiceReference.claimAcknowledgementResponse();
                rtn.ResponseHeader = rht;
                responsePayload = claimsAckParseXMLToStringRes(log, rtn);

                saveRequestResponse(log, requestID, SITransactionKey, "Request Received and Processed Successfully", "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return claimAcknowledgementDataRespError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public static void saveRequestResponse(Logging log, Guid requestID, string BusinessFlow, string StateCode, string requestorSystem,
            string moduleTransactionID, string AdditionalModuleTransactionID, string SITransactionKey, string requestPayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                parameters.Add(SqlParms.CreateParameter("BUSINESS_FLOW", DbType.String, BusinessFlow, true));
                parameters.Add(SqlParms.CreateParameter("STATE_CODE", DbType.String, StateCode, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_ID", DbType.String, moduleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, AdditionalModuleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestPayload, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.ClaimsService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_CLAIMS_ACK_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string claimsAckParseXMLToStringReq(Logging log, ClaimsAckServiceReference.claimAcknowledgementRequest req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsAckServiceReference.claimAcknowledgementRequest));
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
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string claimsAckParseXMLToStringRes(Logging log, ClaimsAckServiceReference.claimAcknowledgementResponse req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ClaimsAckServiceReference.claimAcknowledgementResponse));
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
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
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

                DataAccess.ExecuteStoredProcedure("usp_InsertDOCUMENTS_CLAIM_SUBMISSION_277", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static void processTemplateBody(Logging log, Guid requestID, ClaimsAckServiceReference.claimAcknowledgementRequest req)
        {
            try
            {
                if (req != null &&
                    req.Response != null &&
                    req.Response.Count() > 0 &&
                    req.Response[0] != null &&
                    req.Response[0].Partner != null &&
                    req.Response[0].Partner.Outbound != null &&
                    req.Response[0].Partner.Outbound.F5010X214 != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277 != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider != null &&
                    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0] != null)
                {
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string BillProvName = string.Empty;
                    string BillProvNPI = string.Empty;
                    string BillProvMedID = string.Empty;
                    string ICN = string.Empty;

                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider != null)
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched NM1_85_Provider section. Started processing..", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                        BillProvName = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider?.NameLastOrgName_91?.ToString();
                        //ICN = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider.NameLastOrgName_91.ToString();

                        if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider.IDCd_100 != null)
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Started Processing..", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                            BillProvNPI = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider?.IDCd_100.IDCd?.ToString();
                            BillProvMedID = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider.IDCd_100.IDCdQual_0x0027_r;
                            string BillProvNPIORMedID = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider.NM1_85_Provider.IDCd_100.IDCd;

                            if (!string.IsNullOrEmpty(BillProvMedID) && BillProvMedID != "?")
                            {
                                switch (BillProvMedID.ToUpper())
                                {
                                    case "XX":

                                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. BillProvMedID: " + BillProvMedID, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                        List<SqlParameter> npiParam = new List<SqlParameter>();
                                        npiParam.Add(SqlParms.CreateParameter("NPI", DbType.String, BillProvNPIORMedID, true));
                                        DataSet DSRegProviderDetails = DataAccess.ExecuteStoredProcedure("usp_SelectREGProviderDetails", npiParam, "DSRegProviderDetails");

                                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Stored Procedure Executed: usp_SelectREGProviderDetails", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                        if (DSRegProviderDetails != null &&
                                           DSRegProviderDetails.Tables != null &&
                                           DSRegProviderDetails.Tables.Count > 0 &&
                                           DSRegProviderDetails.Tables[0].Rows != null &&
                                           DSRegProviderDetails.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow row = DSRegProviderDetails.Tables[0].Rows[0];
                                            BillProvNPI = row["NPI"] != null ? row["NPI"].ToString() : string.Empty;
                                            BillProvMedID = row["MEDICAID_ID"] != null ? row["MEDICAID_ID"].ToString() : string.Empty;
                                            BillProvName = row["NAME"] != null ? row["NAME"].ToString() : string.Empty;
                                            //ICN = row["NAME"] != null ? row["NAME"].ToString() : string.Empty;
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - NPI: " + BillProvNPI, Logging.LogPriority.Information);
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - MEDICAID_ID: " + BillProvMedID, Logging.LogPriority.Information);
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - NAME: " + BillProvName, Logging.LogPriority.Information);
                                        }
                                        else
                                        {
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Stored Procedure usp_SelectREGProviderDetails data is empty", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                        }
                                        break;
                                    case "SV":
                                    case "FI":

                                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. BillProvMedID: " + BillProvMedID, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                        List<SqlParameter> medIdParam = new List<SqlParameter>();
                                        medIdParam.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, BillProvNPIORMedID, true));
                                        DataSet DSRegDetails = DataAccess.ExecuteStoredProcedure("usp_SelectREGProviderDetails", medIdParam, "DSRegDetails");

                                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Stored Procedure Executed: usp_SelectREGProviderDetails", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                        if (DSRegDetails != null &&
                                           DSRegDetails.Tables != null &&
                                           DSRegDetails.Tables.Count > 0 &&
                                           DSRegDetails.Tables[0].Rows != null &&
                                           DSRegDetails.Tables[0].Rows.Count > 0)
                                        {
                                            DataRow row = DSRegDetails.Tables[0].Rows[0];
                                            BillProvNPI = row["NPI"] != null ? row["NPI"].ToString() : string.Empty;
                                            BillProvMedID = row["MEDICAID_ID"] != null ? row["MEDICAID_ID"].ToString() : string.Empty;
                                            BillProvName = row["NAME"] != null ? row["NAME"].ToString() : string.Empty;
                                            //ICN = row["NAME"] != null ? row["NAME"].ToString() : string.Empty;
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - NPI: " + BillProvNPI, Logging.LogPriority.Information);
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - MEDICAID_ID: " + BillProvMedID, Logging.LogPriority.Information);
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 - NAME: " + BillProvName, Logging.LogPriority.Information);
                                        }
                                        else
                                        {
                                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Stored Procedure usp_SelectREGProviderDetails data is empty", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                        }
                                        break; ;
                                    default: break;
                                }
                            }
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider.NM1_85_Provider.IDCd_100 section. Processing Complete.", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        }

                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched NM1_85_Provider section. Processing Complete", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Null or Empty NM1_85_Provider section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    //ICN
                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id != null &&
                         req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.TRN_114 != null)
                    {
                        ICN = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.TRN_114.TRN;
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Information_Receiver.Information_Receiver_Trace_Id.TRN_114.TRN - ICN: " + ICN, Logging.LogPriority.Information);
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Null or empty Information_Receiver_Status_Information section for ICN", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    //ICN = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Patient_Level.REF_1K_Payors_Claim_Number.ReferenceID_193.RefID[0].Ref_0x0023__191;

                    string RespDT = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.GS?.Date_55.ToString()?.ToString();

                    ClaimsAckServiceReference.ResponsePartnerOutboundF5010X214_0x0023_277TransactionInformation_SourceInformationSourceNM1_PR_Payer nmPayer = (ClaimsAckServiceReference.ResponsePartnerOutboundF5010X214_0x0023_277TransactionInformation_SourceInformationSourceNM1_PR_Payer)req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source?.InformationSource?.Item;

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched nmPayer section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    string PayerContactName = (nmPayer != null && nmPayer.NameLastOrgName_91 != null) ? nmPayer.NameLastOrgName_91.ToString() : string.Empty;
                    string PayerID = (nmPayer != null && nmPayer.EntityRelationshipCd_101 != null) ? nmPayer.EntityRelationshipCd_101.ToString() : string.Empty;
                    //string PayerContactName = string.Empty;
                    //string PayerID = string.Empty;
                    string PayerEmail = string.Empty;
                    string PayerContactNo = string.Empty;

                    if (!string.IsNullOrEmpty(PayerID) && PayerID != "?")
                    {
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("MEDICAID_ID", DbType.String, PayerID, true));
                        DataSet DSRegAddressDetails = DataAccess.ExecuteStoredProcedure("usp_SelectREG_ADDRESSDetails", param, "DSRegAddressDetails");

                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed Stored Procedure usp_SelectREG_ADDRESSDetails with value MEDICAID_ID as " + PayerID, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                        if (DSRegAddressDetails != null &&
                           DSRegAddressDetails.Tables != null &&
                           DSRegAddressDetails.Tables.Count > 0 &&
                           DSRegAddressDetails.Tables[0].Rows != null &&
                           DSRegAddressDetails.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = DSRegAddressDetails.Tables[0].Rows[0];
                            PayerEmail = row["EMAIL1"] != null ? row["EMAIL1"].ToString() : string.Empty;
                            PayerContactNo = row["PHONE1"] != null ? row["PHONE1"].ToString() : string.Empty;
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched  - Payer Email : " + PayerEmail, Logging.LogPriority.Information);
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched  - Payer Contact : " + PayerContactNo, Logging.LogPriority.Information);

                        }
                        else
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed Stored Procedure usp_SelectREG_ADDRESSDetails. Returned empty/null records", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): PayerID is null/empty or ?. ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    //ERRORS AND EXCEPTIONS
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): ERRORS AND EXCEPTIONS -Start ", Logging.LogPriority.Information);
                    
                    string provider_category_status_code = string.Empty;
                    string provider_category_status_code_desc = string.Empty;
                    string provider_status_code = string.Empty;
                    string provider_status_code_desc = string.Empty;
                    string receiver_status_code = string.Empty;
                    string receiver_status_code_desc = string.Empty;
                    string receiver_category_status_code = string.Empty;
                    string receiver_category_status_code_desc = string.Empty;

                    //Receiver Error Status - Information_Receiver_Status_Information
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): ERRORS AND EXCEPTIONS Fetched Receiver Error Status section", Logging.LogPriority.Information);
                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information != null)
                    {
                        if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.Other_156[0] != null)
                        {
                            receiver_category_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.Other_156[0].Logical_143;
                            if (!string.IsNullOrEmpty(receiver_category_status_code) && receiver_category_status_code != "?")
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Information_Receiver_Status_Information.Other_156[0].Logical_143 - Receiver Category Status Code: " + receiver_category_status_code, Logging.LogPriority.Information);
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CATEGORY_CODE", DbType.String, receiver_category_status_code, true));
                                DataSet DS_Receiver_Category_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMCategoryStatusDetails", param, "DS_Receiver_Category_Status_Code_Descp");

                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMCategoryStatusDetails with value CLAIM_STATUS_CATEGORY_CODE as " + receiver_category_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                if (DS_Receiver_Category_Status_Code_Descp != null &&
                                   DS_Receiver_Category_Status_Code_Descp.Tables != null &&
                                   DS_Receiver_Category_Status_Code_Descp.Tables.Count > 0 &&
                                   DS_Receiver_Category_Status_Code_Descp.Tables[0].Rows != null &&
                                   DS_Receiver_Category_Status_Code_Descp.Tables[0].Rows.Count > 0)
                                {
                                    DataRow row = DS_Receiver_Category_Status_Code_Descp.Tables[0].Rows[0];
                                    receiver_category_status_code_desc = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                                }
                                else
                                {
                                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectCLAIMCategoryStatusDetails returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                }
                            }
                        }
                        if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.First_149 != null)
                        {
                            receiver_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.First_149.Status_144;
                            if (!string.IsNullOrEmpty(receiver_status_code) && receiver_status_code != "?")
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Information_Receiver_Status_Information.Other_156[0].First_149.Status_144 - Receiver Status Code: " + receiver_status_code, Logging.LogPriority.Information);
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CODE", DbType.String, receiver_status_code, true));
                                DataSet DS_Receiver_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMStatusDetails", param, "DS_Receiver_Status_Code_Descp");

                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMStatusDetails with value CLAIM_STATUS_CODE as " + receiver_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                                if (DS_Receiver_Status_Code_Descp != null &&
                                   DS_Receiver_Status_Code_Descp.Tables != null &&
                                   DS_Receiver_Status_Code_Descp.Tables.Count > 0 &&
                                   DS_Receiver_Status_Code_Descp.Tables[0].Rows != null &&
                                   DS_Receiver_Status_Code_Descp.Tables[0].Rows.Count > 0)
                                {
                                    DataRow row = DS_Receiver_Status_Code_Descp.Tables[0].Rows[0];
                                    receiver_status_code_desc = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                                }
                                else
                                {
                                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectCLAIMStatusDetails returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                }
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Null or empty Information_Receiver_Status_Information section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    //Billing_Provider_Status_Information
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): ERRORS AND EXCEPTIONS Fetched Billing Provider Status section", Logging.LogPriority.Information);
                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0] != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0].First_149 != null)
                    {
                        provider_category_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0].First_149.Logical_143;
                        provider_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0].First_149.Status_144;
                        if (!string.IsNullOrEmpty(provider_category_status_code) && provider_category_status_code != "?")
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0].First_149.Logical_143 - Provider Category Status Code: " + provider_category_status_code, Logging.LogPriority.Information);
                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CATEGORY_CODE", DbType.String, provider_category_status_code, true));
                            DataSet DS_Provider_Category_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMCategoryStatusDetails", param, "DS_Provider_Category_Status_Code_Descp");

                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMCategoryStatusDetails with value CLAIM_STATUS_CATEGORY_CODE as " + provider_category_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                            if (DS_Provider_Category_Status_Code_Descp != null &&
                               DS_Provider_Category_Status_Code_Descp.Tables != null &&
                               DS_Provider_Category_Status_Code_Descp.Tables.Count > 0 &&
                               DS_Provider_Category_Status_Code_Descp.Tables[0].Rows != null &&
                               DS_Provider_Category_Status_Code_Descp.Tables[0].Rows.Count > 0)
                            {
                                DataRow row = DS_Provider_Category_Status_Code_Descp.Tables[0].Rows[0];
                                provider_category_status_code_desc = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectCLAIMCategoryStatusDetails returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }
                        }
                        if (!string.IsNullOrEmpty(provider_status_code) && provider_status_code != "?")
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Provider_of_Service_Trace_Id.Billing_Provider_Status_Information[0].First_149.Status_144 - Provider Status Code: " + provider_status_code, Logging.LogPriority.Information);
                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CODE", DbType.String, provider_status_code, true));
                            DataSet DS_Provider_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMStatusDetails", param, "DS_Provider_Status_Code_Descp");

                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMStatusDetails with value CLAIM_STATUS_CODE as " + provider_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                            if (DS_Provider_Status_Code_Descp != null &&
                               DS_Provider_Status_Code_Descp.Tables != null &&
                               DS_Provider_Status_Code_Descp.Tables.Count > 0 &&
                               DS_Provider_Status_Code_Descp.Tables[0].Rows != null &&
                               DS_Provider_Status_Code_Descp.Tables[0].Rows.Count > 0)
                            {
                                DataRow row = DS_Provider_Status_Code_Descp.Tables[0].Rows[0];
                                provider_status_code_desc = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectCLAIMStatusDetails returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Null or empty Billing_Provider_Status_Information section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): ERRORS AND EXCEPTIONS - End ", Logging.LogPriority.Information);
                    string submitted_charges = string.Empty;
                    string claim_paid_date = string.Empty;
                    string patient_control_number = string.Empty;
                    string paid_amount = string.Empty;

                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0] != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0] != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0] != null)
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetching values under Healthcare_Claim section started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                        submitted_charges = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0].Submitted_152;
                        claim_paid_date = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0].Payment_58;
                        patient_control_number = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Provider_of_Service_Trace_Id.TRN_114.TRN;
                        paid_amount = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0].Paid_153;

                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetching values under Healthcare_Claim section finished", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Null or empty Healthcare_Claim section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    string claim_status_code = string.Empty;
                    string claim_status_code_descp = string.Empty;
                    string claim_category_status_code = string.Empty;
                    string claim_category_status_code_descp = string.Empty;

                    //if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id != null &&
                    //   req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Info_Receiver.QTY_90_Accepted != null)
                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient != null &&
                       req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0] != null &&
                       req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient != null &&
                       req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0] != null &&
                       req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim != null &&
                       req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0] != null)
                    {
                        //claim_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Info_Receiver.QTY_90_Accepted.QtyQual_0x0027_r_160;
                        claim_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0].First_149.Status_144;

                        //TODO: Need to check on this
                        if (!string.IsNullOrEmpty(claim_status_code) && claim_status_code != "?")
                        {
                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CODE", DbType.String, claim_status_code, true));
                            DataSet DS_Claim_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMStatusDetails", param, "DS_Claim_Status_Code_Descp");

                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMStatusDetails with value CLAIM_STATUS_CODE as " + claim_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                            if (DS_Claim_Status_Code_Descp != null &&
                               DS_Claim_Status_Code_Descp.Tables != null &&
                               DS_Claim_Status_Code_Descp.Tables.Count > 0 &&
                               DS_Claim_Status_Code_Descp.Tables[0].Rows != null &&
                               DS_Claim_Status_Code_Descp.Tables[0].Rows.Count > 0)
                            {
                                DataRow row = DS_Claim_Status_Code_Descp.Tables[0].Rows[0];
                                claim_status_code_descp = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectProcedureCode_Dynamic returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Info_Receiver.QTY_90_Accepted is empty or null or have value as ?", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }

                    //if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id != null &&
                    //    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information != null &&
                    //    req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.First_149 != null)
                    if (req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0] != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0] != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim != null &&
                        req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0] != null)
                    {
                        //claim_category_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Information_Receiver_Trace_Id.Information_Receiver_Status_Information.First_149.Status_144;
                        claim_category_status_code = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider[0].Patient[0].Patient[0].Healthcare_Claim[0].First_149.Logical_143;
                        if (!string.IsNullOrEmpty(claim_category_status_code) && claim_category_status_code != "?")
                        {
                            List<SqlParameter> param = new List<SqlParameter>();
                            param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CATEGORY_CODE", DbType.String, claim_category_status_code, true));
                            DataSet DS_Category_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMCategoryStatusDetails", param, "DS_Category_Status_Code_Descp");

                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Executed stored procedure usp_SelectCLAIMCategoryStatusDetails with value CLAIM_STATUS_CATEGORY_CODE as " + claim_category_status_code, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                            if (DS_Category_Status_Code_Descp != null &&
                               DS_Category_Status_Code_Descp.Tables != null &&
                               DS_Category_Status_Code_Descp.Tables.Count > 0 &&
                               DS_Category_Status_Code_Descp.Tables[0].Rows != null &&
                               DS_Category_Status_Code_Descp.Tables[0].Rows.Count > 0)
                            {
                                DataRow row = DS_Category_Status_Code_Descp.Tables[0].Rows[0];
                                claim_category_status_code_descp = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Stored procedure usp_SelectCLAIMCategoryStatusDetails returned empty or null data ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }
                        }
                        //claim_category_status_code_descp = "TBD"; //Claim category status description reference table is yet to be created. This will be updated once table is ready
                    }
                    else
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Empty or null Information_Receiver_Status_Information.First_149 section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }


                    DataTable dataTable = new DataTable();
                    dataTable.Columns.AddRange(new DataColumn[9] { new DataColumn("Service Line",typeof(int)),
                                                               new DataColumn("Service Code Type",typeof(string)),
                                                               new DataColumn("Service Code",typeof(string)),
                                                               new DataColumn("Charged Amount",typeof(string)),
                                                               new DataColumn("Paid Amount",typeof(string)),
                                                               new DataColumn("Category Code",typeof(string)),
                                                               new DataColumn("Category Code Description",typeof(string)),
                                                               new DataColumn("Status Code",typeof(string)),
                                                               new DataColumn("Status Code Description",typeof(string))});

                    var serviceList = req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider;

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Information_Receiver.Provider section", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetched Information_Receiver.Provider section count is " + req.Response[0].Partner.Outbound.F5010X214._0x0023_277.Transaction.Information_Source.Information_Receiver.Provider?.Count(), Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    int count = 1;

                    string serviceCodeType = string.Empty;
                    string serviceCode = string.Empty;
                    string chargedAmount = string.Empty;
                    string paidAmount = string.Empty;
                    string categoryCode = string.Empty;
                    string categoryCodeDescription = string.Empty;
                    string statusCode = string.Empty;
                    string statusCodeDescription = string.Empty;

                    foreach (var item in serviceList)
                    {
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Fetching service list item in a loop. Loop Count: " + count, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                        serviceCodeType = string.Empty;
                        serviceCode = string.Empty;
                        chargedAmount = string.Empty;
                        paidAmount = string.Empty;
                        categoryCode = string.Empty;
                        categoryCodeDescription = string.Empty;
                        statusCode = string.Empty;
                        statusCodeDescription = string.Empty;

                        if (item.Patient != null &&
                            item.Patient[0] != null &&
                            item.Patient[0].Patient != null &&
                            item.Patient[0].Patient[0] != null &&
                            item.Patient[0].Patient[0].Service_Line != null &&
                            item.Patient[0].Patient[0].Service_Line[0] != null &&
                            item.Patient[0].Patient[0].Service_Line[0].SVC != null)
                        {
                            if (item.Patient[0].Patient[0].Service_Line[0].SVC.Adjudicated != null)
                            {
                                serviceCodeType = item.Patient[0].Patient[0].Service_Line[0].SVC.Adjudicated.ProdServiceIDQual_0x0027_r_216;
                                serviceCode = item.Patient[0].Patient[0].Service_Line[0].SVC.Adjudicated.ProdServiceID_217;
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Section item.Patient[0].Patient[0].Service_Line[0].SVC.Adjudicated is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }

                            chargedAmount = (item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151 != null &&
                                             item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151.Count() > 0 &&
                                             !item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151[0].Equals(null)) ?

                                            item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151[0].ToString() : string.Empty;

                            paidAmount = (item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151 != null &&
                                          item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151.Count() > 1 &&
                                          !item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151[1].Equals(null)) ?

                                          item.Patient[0].Patient[0].Service_Line[0].SVC.M_0x0027_Amt_151[1].ToString() : string.Empty;
                        }
                        else
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Section item.Patient[0].Patient[0].Service_Line[0].SVC is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        }

                        if (item.Patient != null &&
                           item.Patient[0] != null &&
                           item.Patient[0].Patient != null &&
                           item.Patient[0].Patient[0] != null &&
                           item.Patient[0].Patient[0].Service_Line != null &&
                           item.Patient[0].Patient[0].Service_Line[0] != null &&
                           item.Patient[0].Patient[0].Service_Line[0].Service_Line_Status_Info != null &&
                           item.Patient[0].Patient[0].Service_Line[0].Service_Line_Status_Info[0] != null)
                        {
                            categoryCode = item.Patient[0].Patient[0].Service_Line[0].Service_Line_Status_Info[0].First_149.Logical_143;
                            categoryCodeDescription = "";
                            if (!string.IsNullOrEmpty(categoryCode) && categoryCode != "?")
                            {
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CATEGORY_CODE", DbType.String, categoryCode, true));
                                DataSet DS_Category_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMCategoryStatusDetails", param, "DS_Category_Status_Code_Descp");
                                if (DS_Category_Status_Code_Descp != null &&
                                   DS_Category_Status_Code_Descp.Tables != null &&
                                   DS_Category_Status_Code_Descp.Tables.Count > 0 &&
                                   DS_Category_Status_Code_Descp.Tables[0].Rows != null &&
                                   DS_Category_Status_Code_Descp.Tables[0].Rows.Count > 0)
                                {
                                    DataRow row = DS_Category_Status_Code_Descp.Tables[0].Rows[0];

                                    categoryCodeDescription = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;

                                }
                                else
                                {
                                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): categoryCode parameter with Stored procedure usp_SelectProcedureCode_Dynamic result is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                }
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): categoryCode field is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }

                            statusCode = item.Patient[0].Patient[0].Service_Line[0].Service_Line_Status_Info[0].First_149.Status_144;
                            statusCodeDescription = "";
                            if (!string.IsNullOrEmpty(statusCode) && statusCode != "?")
                            {
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("CLAIM_STATUS_CODE", DbType.String, statusCode, true));
                                DataSet DS_Status_Code_Descp = DataAccess.ExecuteStoredProcedure("usp_SelectCLAIMStatusDetails", param, "DS_Status_Code_Descp");
                                if (DS_Status_Code_Descp != null &&
                                   DS_Status_Code_Descp.Tables != null &&
                                   DS_Status_Code_Descp.Tables.Count > 0 &&
                                   DS_Status_Code_Descp.Tables[0].Rows != null &&
                                   DS_Status_Code_Descp.Tables[0].Rows.Count > 0)
                                {
                                    DataRow row = DS_Status_Code_Descp.Tables[0].Rows[0];

                                    statusCodeDescription = row["SHORT_DESC"] != null ? row["SHORT_DESC"].ToString() : string.Empty;

                                }
                                else
                                {
                                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): statusCode parameter with Stored procedure usp_SelectProcedureCode_Dynamic result is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                                }
                            }
                            else
                            {
                                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): statusCode field is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                            }
                        }
                        else
                        {
                            log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Section item.Patient[0].Patient[0].Service_Line[0].Service_Line_Status_Info[0] is null or empty in Service list loop Count: " + count + " ", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                        }

                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Started adding data to dataTable", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                        dataTable.Rows.Add(count++,
                                       serviceCodeType,
                                      serviceCode,
                                      chargedAmount,
                                      paidAmount,
                                      categoryCode,
                                      categoryCodeDescription,
                                      statusCode,
                                      statusCodeDescription);
                        log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Finished adding data to dataTable", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                    }


                    string body = string.Empty;
                    Dictionary<string, object> fields = new Dictionary<string, object>();
                    fields.Add("BillProvMedID", BillProvMedID);
                    fields.Add("BillProvNPI", BillProvNPI);
                    fields.Add("BillProvName", BillProvName);
                    fields.Add("RespDT", RespDT);
                    fields.Add("ClaimICN", ICN);
                    fields.Add("PayerContactName", PayerContactName);
                    fields.Add("PayerContactNo", PayerContactNo);
                    fields.Add("PayerID", PayerID);
                    fields.Add("PayerEmail", PayerEmail);
                    fields.Add("ICN", ICN);
                    fields.Add("SubCharges", submitted_charges);
                    fields.Add("CPaidDate", claim_paid_date);
                    fields.Add("PtCNum", patient_control_number);
                    fields.Add("PaidAmt", paid_amount);
                    fields.Add("CSCCode", claim_category_status_code);
                    fields.Add("CSCDesp", claim_category_status_code_descp);
                    fields.Add("CSCode", claim_status_code);
                    fields.Add("CSCCDesp", claim_status_code_descp);
                    fields.Add("RLECCode", receiver_category_status_code);
                    fields.Add("RLECDesc", receiver_category_status_code_desc);
                    fields.Add("RLECode", receiver_status_code);
                    fields.Add("RLEDesc", receiver_status_code_desc);
                    fields.Add("PLECCode", provider_category_status_code);
                    fields.Add("PLECDesc", provider_category_status_code_desc);
                    fields.Add("PLECode", provider_status_code);
                    fields.Add("PLEDesc", provider_status_code_desc);
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): initServiceDetails Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    fields.Add("ServiceDetails", initServiceDetails(log, dataTable));

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): initServiceDetails Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    body = generateTemplateBody(log, CON.ResponseFileTemplates.CA277, fields);

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    Dictionary<string, object> parms = new Dictionary<string, object>();
                    parms.Add("PayerName", PayerContactName.ToString());
                    parms.Add("ICN", ICN.ToString());
                    parms.Add("PatientControlNumber", patient_control_number.ToString());
                    parms.Add("ReportType", "".ToString());

                    parms.Add("LAST_MODIFIED_USER", CON.WebApiServiceGuid.ClaimsService);
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now);
                    parms.Add("CREATE_DATE_TIME", DateTime.Now);
                    parms.Add("Created_By_User", CON.WebApiServiceGuid.ClaimsService);
                    parms.Add("Medicaid_ID", BillProvMedID.ToString());

                    parms.Add("StatusDate", DateTime.Now.ToString());
                    parms.Add("HTML_BODY", body);
                    parms.Add("REQUEST_ID", requestID);

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): saveTemplateDetails Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    saveTemplateDetails(log, parms);

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): saveTemplateDetails Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Finished", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                }
                else
                {
                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): Response is null or empty", Logging.LogPriority.Error);
                    throw new Exception("ClaimsAckServiceHelper - Method: processTemplateBody(): Response is null or empty");
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string initServiceDetails(Logging log, DataTable dataTable)
        {
            //string serviceHeader = "<tr><td>Service Line</td><td>Service Code Type</td><td>Service Code</td><td>Charged Amount</td><td>Paid Amount</td><td>Category Code</td><td>Category Code Description</td><td>Status Code</td><td>Status Code Description</td></tr>";           

            try
            {
                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): initServiceDetails Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

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

                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): initServiceDetails Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                return sb.ToString();
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string generateTemplateBody(Logging log, string templateName, Dictionary<string, object> fields)
        {
            try
            {
                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody Started", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                string path;
                string body = string.Empty;

                string templateActualPath = AppSettings.Get("TemplateFilePath", string.Empty) + @"/Claims277CATemplate.txt";
                
                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody templateActualPath = " + templateActualPath, Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

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

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody - Template loaded", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                    body = template.Eval(TemplateFieldAccessors.DictionaryAccessor(fields));

                    log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody - Template body evaluated", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01
                }
                else
                {
                    throw new Exception("ClaimsAckServiceHelper - Method: processTemplateBody(): templateActualPath is empty");
                }

                log.CreateLogEntry("ClaimsAckServiceHelper - Method: processTemplateBody(): generateTemplateBody Completed", Logging.LogPriority.Information); // TO DO: Remove this once passes on INT01

                return body;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }
    }
}
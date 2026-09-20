using Corp.Core.Libraries;
using Corp.Core.Libraries.AcknowledgmentService;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.PartialProviderManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class PartialProviderManagementService : IPartialProviderManagement
    {
        private string appPDMSDataExchangeUserId = "5D0689A8-D885-4211-B9FD-56757474AB4D";
        private bool errorAck = false;
        Logging Log;
        private string ExceptionMessage = string.Empty;
        private string LogMessage = string.Empty;
        private string applicationType = string.Empty;
        private int PayloadRegID = 0;
        private string EnrollmentBusinessStatusReasonCode = string.Empty;
        static List<string> m_ApplicationStatusIDs = new List<string>();
        static List<string> m_ApplicationStatusIDsOkToValidateData = new List<string>();
        private List<string> validOwnerTitles = new List<string>();
        public PartialProviderManagementService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(Guid.NewGuid(), "PDMSWebAPI:PartialProviderManagementService");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:PartialProviderManagementService");
            }
        }

        public submitPartialProviderResponse SubmitPartialProvider(submitPartialProviderRequest partialRequest)
        {
            //Log.CreateLogEntry("Calling Inbound Partial Provider ", Logging.LogPriority.Information);

            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsPartialProviderMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }

            int ProviderInformationID = 0;
            int RegID = 0, PayloadApplicationID = 0, PayloadAddnApplicationID = 0;
            int moduleTransactionID = 0, CurrentStepID = 0;
            string PayloadApplicationStatus = string.Empty, PayloadApplicationLegalStatus = string.Empty;
            bool enableCR344 = Convert.ToBoolean(AppSettings.Get("EnableCR344", "false"));
            bool enableCR387 = Convert.ToBoolean(AppSettings.Get("EnableCR387", "false"));
            bool enableCR386 = Convert.ToBoolean(AppSettings.Get("EnableCR386", "false"));
            MessageHeader Header = partialRequest.MessageHeader;
            SubmitPartialProviderRequestPayload Payload = partialRequest.Payload;

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(SubmitPartialProviderRequestPayload));
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            var stream2 = new StringWriter();
            var writer = XmlWriter.Create(stream2, settings);
            x.Serialize(writer, Payload, emptyNs);
            string xml = stream2.ToString();

            bool sendAcknowledgment = Header.AdditionalModuleTransactionId.Contains("{PDMS}") ? true : false;
            bool isError = false;
            PartialProviderResponse[] resp = new PartialProviderResponse[100];
            int cnt_resp = 0;

            submitPartialProviderResponse rtn = new submitPartialProviderResponse();
            rtn.SITransactionKey = Header.SITransactionKey;
            rtn.ModuleTransactionId = Header.ModuleTransactionId;
            rtn.AdditionalModuleTransactionId = sendAcknowledgment ? Header.AdditionalModuleTransactionId.Substring(6) : Header.AdditionalModuleTransactionId;
            if (Int32.TryParse(rtn.ModuleTransactionId, out moduleTransactionID)) { }
            int transactionID = 0;
            bool isClosure = false;
            string facilityNumber = string.Empty;
            string DDdesignation = string.Empty;
            bool legalStatus = false;
            bool updateClosedPartialApprovalLegalStatusReceived = false;
            int WorkflowEventTypeID = -1;
            int WaiverTypeID = 0, WaiverServiceUpdateTypeID = 0;
            int ApplicationTypeID = 0;
            int ProcessID = 0;
            int WorkflowID = 0;
            string mmisProviderTypeId = string.Empty;
            string ClosureEffectiveDate = string.Empty;
            string proposedEffectiveDate = string.Empty;
            int RegistrationStatusTypeID = 0;

            DataSet dsReg = new DataSet();
            try
            {
                transactionID = Convert.ToInt32(Header.ModuleTransactionId);
                string reqSystem = (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM) ? "PSM" : (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW) ? "PCW" : string.Empty;
                if (Payload.ProviderInformation.ProviderApplication != null)
                {
                    foreach (PartialProviderApplication lst in Payload.ProviderInformation.ProviderApplication)
                    {
                        if (string.IsNullOrEmpty(lst.ApplicationType))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ApplicationType field cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ApplicationType cannot be blank", Logging.LogPriority.Error);
                        }
                        else
                        {
                            DataSet dsApp = DataAccess.ExecuteStoredProcedure("usp_SelectAdditionalApplicationType");
                            applicationType = lst.ApplicationType;
                            //string reqSystem = (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM) ? "PSM" : (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW) ? "PCW" : string.Empty;
                            bool isValidAppType = dsApp.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("ADDITIONAL_APPLICATION_TYPE_NAME").ToLower() == applicationType.ToLower() &&
                                                      r.Field<string>("SOURCE_SYSTEM") == reqSystem)
                                          .Any();

                            if (!isValidAppType)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - Application Type not configured in PNM.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - Application Type not configured in PNM", Logging.LogPriority.Error);
                            }

                        }
                        if (applicationType != "Manual Update" && applicationType != "Bulk Update")
                        {
                            if (string.IsNullOrEmpty(lst.ApplicationStatus))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ApplicationStatus field cannot be blank.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - ApplicationStatus cannot be blank", Logging.LogPriority.Error);
                            }
                            else
                            {
                                List<SqlParameter> par = new List<SqlParameter>();
                                par.Add(SqlParms.CreateParameter("Sender", DbType.String, "PartialProvider", false));
                                DataSet dsApp = DataAccess.ExecuteStoredProcedure("usp_SelectAllApplicationStatus", par, "AppStatus");

                                bool isValidAppType = dsApp.Tables[0].AsEnumerable()
                                              .Where(r => r.Field<string>("APPLICATION_STATUS_DESC").ToLower() == lst.ApplicationStatus.ToLower())
                                              .Any();

                                if (!isValidAppType)
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - Application Status not configured in PNM.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - Application Status not configured in PNM", Logging.LogPriority.Error);
                                }

                            }
                        }

                    }
                }
                else
                {
                    resp[cnt_resp] = new PartialProviderResponse();
                    resp[cnt_resp].ResponseCode = "3002"; //error
                    resp[cnt_resp].ResponseType = "Failure";
                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderApplication node cannot be blank.";
                    cnt_resp++;
                    isError = true;
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - ProviderApplication node cannot be blank..", Logging.LogPriority.Error);
                }

                if (Payload.ProviderInformation.ProviderAlternateIdentifiers != null)
                {
                    foreach (PartialProviderAlternateIdList lst in Payload.ProviderInformation.ProviderAlternateIdentifiers)
                    {
                        if (!IsApplicationStatus(PayloadApplicationStatus) && !string.IsNullOrEmpty(lst.AlternateIdType) && string.IsNullOrEmpty(lst.AlternateId))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - in the ProviderAlternateIdentifiers node - AlternateID cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - AlternateID cannot be blank", Logging.LogPriority.Error);
                        }
                        if (lst.AlternateIdType == "REG")
                            PayloadRegID = string.IsNullOrEmpty(lst.AlternateId) ? 0 : Convert.ToInt32(lst.AlternateId);
                        if (lst.AlternateIdType == "MRD")
                            facilityNumber = string.IsNullOrEmpty(lst.AlternateId) ? string.Empty : lst.AlternateId.ToString();
                        if (lst.AlternateIdType == "DES")
                            DDdesignation = string.IsNullOrEmpty(lst.AlternateId) ? string.Empty : lst.AlternateId.ToString();
                        if (lst.AlternateIdType == "Legal Status" && lst.AlternateId == "LGL" && enableCR386) //CR386 OHPNM-13730 
                            legalStatus = true;
                    }
                }

                if (PayloadRegID == 0)
                {
                    resp[cnt_resp] = new PartialProviderResponse();
                    resp[cnt_resp].ResponseCode = "3002"; //error
                    resp[cnt_resp].ResponseType = "Failure";
                    resp[cnt_resp].ResponseMessage = "Request has errors - Alternate ID with type REG cannot be blank.";
                    cnt_resp++;
                    isError = true;
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Alternate ID with type REG cannot be Blank", Logging.LogPriority.Error);
                }
                else
                {
                    List<SqlParameter> par = new List<SqlParameter>();
                    par.Add(SqlParms.CreateParameter("RegID", DbType.String, PayloadRegID, false));
                    dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", par, "reg");

                    if (!Methods.HasRows(dsReg))
                    {
                        resp[cnt_resp] = new PartialProviderResponse();
                        resp[cnt_resp].ResponseCode = "3002"; //error
                        resp[cnt_resp].ResponseType = "Failure";
                        resp[cnt_resp].ResponseMessage = "Request has errors - RegID doesnt exist in PNM.";
                        cnt_resp++;
                        isError = true;
                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - RegID doesnt exist in PNM", Logging.LogPriority.Error);
                    }
                    else
                    {
                        if (dsReg != null && dsReg.Tables[0] != null && dsReg.Tables[0].Rows.Count > 0)
                        {
                            WorkflowEventTypeID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["WORKFLOW_EVENT_TYPE_ID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["WORKFLOW_EVENT_TYPE_ID"].ToString());
                            WaiverTypeID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["WaiverTypeID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["WaiverTypeID"].ToString());
                            ApplicationTypeID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["ApplicationTypeID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["ApplicationTypeID"].ToString());
                            ProcessID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["ProcessID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["ProcessID"].ToString());
                            mmisProviderTypeId = Methods.GetStringValue(dsReg.Tables[0].Rows[0], "MMISProviderTypeID");
                            WorkflowID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["WorkflowID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["WorkflowID"].ToString());
                            CurrentStepID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString());
                            WaiverServiceUpdateTypeID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["WaiverServiceUpdateTypeID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["WaiverServiceUpdateTypeID"].ToString());
                            RegistrationStatusTypeID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["RegistrationStatusTypeID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["RegistrationStatusTypeID"].ToString());
                        }
                    }
                }
                if (applicationType != "Manual Update" && applicationType != "Bulk Update")
                {
                    if (transactionID == 0)
                    {
                        resp[cnt_resp] = new PartialProviderResponse();
                        resp[cnt_resp].ResponseCode = "3002"; //error
                        resp[cnt_resp].ResponseType = "Failure";
                        resp[cnt_resp].ResponseMessage = "Request has errors -  Module Transaction ID cannot be blank or Zero.";
                        cnt_resp++;
                        isError = true;
                        Log.CreateLogEntry("Error in Inbound Partial Provider - Module Transaction ID cannot be blank or Zero.", Logging.LogPriority.Error);
                    }
                    else
                    {
                        //Log.CreateLogEntry("Inbound Partial Provider  Processing for Transaction id - " + transactionID.ToString(), Logging.LogPriority.Information);
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("TransactionID", DbType.Int32, transactionID, true));
                        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectTransactionDetailsByTransactionID", param, "TransactionDetails");

                        if (!Methods.HasRows(ds))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - Module Transaction ID does not exist in PNM.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - transaction ID does not exist", Logging.LogPriority.Error);
                        }
                        else
                        {
                            RegID = Convert.ToInt32(ds.Tables[0].Rows[0]["TQREG_ID"]);
                            if (RegID != PayloadRegID)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - module transaction ID does not belong to this registration.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - module transaction ID does not belong to this registration.", Logging.LogPriority.Error);
                            }

                            if (Payload.ProviderInformation.ProviderReviews != null)
                            {
                                foreach (PartialProviderReviewsList lstReview in Payload.ProviderInformation.ProviderReviews)
                                {
                                    proposedEffectiveDate = string.IsNullOrEmpty(lstReview.ProposedEffectiveDate) ? string.Empty : lstReview.ProposedEffectiveDate;
                                    ClosureEffectiveDate = string.IsNullOrEmpty(lstReview.EffectiveDate) ? string.Empty : lstReview.EffectiveDate;
                                }
                            }

                            if (RegistrationStatusTypeID == Constants.RegistrationStatusTypeId.Deleted)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Application was previously expired, withdrawn or cancelled. Provider needs to reapply.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - Application was previously expired, withdrawn or cancelled. Provider needs to reapply.", Logging.LogPriority.Error);
                            }

                            if (Payload.ProviderInformation.ProviderApplication != null)
                            {
                                foreach (PartialProviderApplication lst in Payload.ProviderInformation.ProviderApplication)
                                {
                                    PayloadApplicationID = string.IsNullOrEmpty(lst.ApplicationId) ? 0 : Convert.ToInt32(lst.ApplicationId);
                                    PayloadAddnApplicationID = string.IsNullOrEmpty(lst.AdditionalApplicationId) ? 0 : Convert.ToInt32(lst.AdditionalApplicationId);
                                    PayloadApplicationStatus = string.IsNullOrEmpty(lst.ApplicationStatus) ? string.Empty : lst.ApplicationStatus.ToString();
                                    PayloadApplicationLegalStatus = string.IsNullOrEmpty(lst.ApplicationLegalStatus) ? string.Empty : lst.ApplicationLegalStatus.ToString();

                                    if (PayloadApplicationID == 0)
                                    {
                                        resp[cnt_resp] = new PartialProviderResponse();
                                        resp[cnt_resp].ResponseCode = "3002"; //error
                                        resp[cnt_resp].ResponseType = "Failure";
                                        resp[cnt_resp].ResponseMessage = "Request has errors - ApplicationID cannot be blank.";
                                        cnt_resp++;
                                        isError = true;
                                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - ApplicationID cannot be blank", Logging.LogPriority.Error);
                                    }
                                    else
                                    {
                                        List<SqlParameter> param1 = new List<SqlParameter>();
                                        param1.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, PayloadRegID, true));
                                        param1.Add(SqlParms.CreateParameter("APPLICATION_ID", DbType.Int32, PayloadApplicationID, true));
                                        param1.Add(SqlParms.CreateParameter("ADDITIONAL_APPLICATION_ID", DbType.Int32, PayloadAddnApplicationID, true));
                                        param1.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, Header.RequestorSystem.ToString(), true));
                                        DataSet dsAppln = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationID_byRegID", param1, "RegAppln");
                                        bool isGood = Convert.ToBoolean(dsAppln.Tables[0].Rows[0]["Result"]);
                                        int pnmAppID = Convert.ToInt32(dsAppln.Tables[0].Rows[0]["PNM_APPLICATION_ID"].ToString());
                                        string pnmAddAppStatus = dsAppln.Tables[0].Rows[0]["PNM_ADD_APP_STATUS"].ToString();

                                        if ((applicationType == "Closure") && ((PayloadApplicationStatus.ToLower() == "closed as denied" || PayloadApplicationStatus.ToLower() == "expired"
                                                || PayloadApplicationStatus.ToLower() == "withdrawn by provider"
                                                || PayloadApplicationStatus.ToLower() == "approved" || PayloadApplicationStatus.ToLower() == "closed as complete")
                                              || IsClosureInitiatedByOperator(PayloadRegID, mmisProviderTypeId)))
                                        {
                                            PayloadApplicationID = pnmAppID;
                                        }
                                        // OHPNM-13730 CR386 changes as it is now in DEC release
                                        if (enableCR386 && PayloadApplicationStatus.ToLower() == "closed partial approval" && PayloadApplicationStatus.ToLower() == pnmAddAppStatus.ToLower()
                                            && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM && legalStatus)
                                        {
                                            // OHPNM-13730 this is an "updated closed partial approval" payload with a legal status sent in the AlternateIdType "LGL" area; we need to:
                                            // Do not reject or queue the payload , save the payload, update DODD Legal status and send success response (i.e. Stage the payload, Update the legal status and Send success response)
                                            updateClosedPartialApprovalLegalStatusReceived = true;
                                        }

                                        // OHPNM-13730 - don't need to check for incorrect application id - "Do not queue this payload or reject it for incorrect application Id or registration not in a workflow."
                                        if (!updateClosedPartialApprovalLegalStatusReceived)
                                        {
                                            if (!isGood)
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3002"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request has errors - Application ID does not belong to this registration.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Application ID does not belong to this registration", Logging.LogPriority.Error);
                                            }
                                            else if (PayloadApplicationID != pnmAppID)
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3002"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Application ID not correct. The current PNM application id is - " + pnmAppID.ToString();
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Application ID not correct", Logging.LogPriority.Error);
                                            }
                                        }
                                        if(Header.RequestorSystem.ToString() == "PCW" && pnmAddAppStatus.ToLower() == "recommended certification" && PayloadApplicationStatus.ToLower() == "withdrawn/expired")
                                        {
                                            resp[cnt_resp] = new PartialProviderResponse();
                                            resp[cnt_resp].ResponseCode = "3002"; //error
                                            resp[cnt_resp].ResponseType = "Failure";
                                            resp[cnt_resp].ResponseMessage = "Request has errors - Application ID already has Recommended Certification status.";
                                            cnt_resp++;
                                            isError = true;
                                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Application ID already has Recommended Certification status.", Logging.LogPriority.Error);
                                        }
                                    }
                                    if (string.IsNullOrEmpty(PayloadApplicationStatus))
                                    {
                                        resp[cnt_resp] = new PartialProviderResponse();
                                        resp[cnt_resp].ResponseCode = "3002"; //error
                                        resp[cnt_resp].ResponseType = "Failure";
                                        resp[cnt_resp].ResponseMessage = "Request has errors - ApplicationStatus field cannot be blank.";
                                        cnt_resp++;
                                        isError = true;
                                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ApplicationStatus cannot be blank", Logging.LogPriority.Error);
                                    }
                                    else if (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW)
                                    {
                                        if (dsReg != null && dsReg.Tables[0] != null && dsReg.Tables[0].Rows.Count > 0)
                                        {
                                            CurrentStepID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString());

                                            if (CurrentStepID == 0)
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3002"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request has errors - Registration not in active WF in PNM.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  -  Registration not in active WF in PNM", Logging.LogPriority.Error);
                                            }
                                        }
                                    }
                                    else if (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM)
                                    {
                                        //List<SqlParameter> sqlParms = new List<SqlParameter>();
                                        //sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, RegID, false));
                                        //DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
                                        dsReg.Tables[0].TableName = "RegData";
                                        DataRow drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                                        int regServiceLocationID = Methods.GetIntValue(drReg, "REG_SERVICE_LOCATION_ID");
                                        int currentTaskID = Methods.GetIntValue(drReg, "CurrentTaskID");
                                        string currentTaskName = Methods.GetString("CurrentTaskName", drReg);
                                        DateTime? changeEffectiveDate = Methods.GetDateValue(drReg, "ChangeEffectiveDate");

                                        if (applicationType == "Closure" && (!enableCR387 || (enableCR387 && !string.IsNullOrEmpty(facilityNumber))))
                                        {
                                            isClosure = true;

                                            if ((PayloadApplicationStatus.ToLower() == "approved" || PayloadApplicationStatus.ToLower() == "closed as complete") && string.IsNullOrEmpty(ClosureEffectiveDate))
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3001"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request cannot be processed - Invalid Closure Effective Date.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Invalid Closure Effective Date.", Logging.LogPriority.Error);
                                            }

                                            if (PayloadApplicationStatus.ToLower() == "pending review"
                                            || PayloadApplicationStatus.ToLower() == "sanction review" || PayloadApplicationStatus.ToLower() == "waiting on agreement"
                                            || PayloadApplicationStatus.ToLower() == "capital funds review" || PayloadApplicationStatus.ToLower() == "in review"
                                            || PayloadApplicationStatus.ToLower() == "pending effective date")
                                            {
                                                bool isValidClosureDate = IsClosureDateValid(proposedEffectiveDate, RegID, changeEffectiveDate);

                                                if (!isValidClosureDate)
                                                {
                                                    resp[cnt_resp] = new PartialProviderResponse();
                                                    resp[cnt_resp].ResponseCode = "3001"; //error
                                                    resp[cnt_resp].ResponseType = "Failure";
                                                    resp[cnt_resp].ResponseMessage = "Request cannot be processed - Invalid Closure Proposed Effective Date.";
                                                    cnt_resp++;
                                                    isError = true;
                                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Invalid Closure Proposed Effective Date.", Logging.LogPriority.Error);

                                                }
                                            }

                                        }

                                        // OHPNM-13730 this is an "updated closed partial approval" payload with a legal status sent in the AlternateIdType "LGL" area; we need to:
                                        // Do not reject or queue the payload , save the payload, update DODD Legal status and send success response (i.e. Stage the payload, Update the legal status and Send success response)
                                        // i.e. we don't need to do this workflow step check if they are updating legal status via closed partial approval - "Do not queue this payload or reject it for incorrect application Id or registration not in a workflow."
                                        if (!updateClosedPartialApprovalLegalStatusReceived)
                                        {
                                            if (dsReg != null && dsReg.Tables[0] != null && dsReg.Tables[0].Rows.Count > 0)
                                            {
                                                CurrentStepID = string.IsNullOrEmpty(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString()) ? 0 : Convert.ToInt32(dsReg.Tables[0].Rows[0]["CurrentStepID"].ToString());
                                                if (isClosure && ((PayloadApplicationStatus.ToLower() == "closed as denied" || PayloadApplicationStatus.ToLower() == "expired"
                                                || PayloadApplicationStatus.ToLower() == "withdrawn by provider"
                                                || PayloadApplicationStatus.ToLower() == "approved" || PayloadApplicationStatus.ToLower() == "closed as complete")
                                                   || IsClosureInitiatedByOperator(PayloadRegID, mmisProviderTypeId)))
                                                {
                                                    CurrentStepID = 1;
                                                }
                                                if (CurrentStepID == 0)
                                                {
                                                    resp[cnt_resp] = new PartialProviderResponse();
                                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                                    resp[cnt_resp].ResponseType = "Failure";
                                                    resp[cnt_resp].ResponseMessage = "Request has errors - Registration not in active WF in PNM.";
                                                    cnt_resp++;
                                                    isError = true;
                                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  -  Registration not in active WF in PNM", Logging.LogPriority.Error);
                                                }
                                            }
                                        }

                                        if (PayloadApplicationStatus.ToLower() == "pending external medicaid approval" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM)
                                        {
                                            if (applicationType.ToLower() == "add designation" && string.IsNullOrEmpty(DDdesignation))
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3001"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request has errors -No Designation information.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - No Designation information.", Logging.LogPriority.Error);

                                            }
                                            if (!string.IsNullOrEmpty(DDdesignation))
                                            {
                                                List<SqlParameter> sqlParms = new List<SqlParameter>();
                                                sqlParms.Add(SqlParms.CreateParameter("DD_DESIGNATION", DbType.String, DDdesignation, false));
                                                string result = Convert.ToString(DataAccess.ExecuteStoredProcedure("usp_VerifyDODD_Designation", sqlParms, "Result", SqlDbType.VarChar, 100));
                                                if (result == "N")
                                                {
                                                    resp[cnt_resp] = new PartialProviderResponse();
                                                    resp[cnt_resp].ResponseCode = "3001"; //error
                                                    resp[cnt_resp].ResponseType = "Failure";
                                                    resp[cnt_resp].ResponseMessage = "Request has errors -Designation should be either Pro or Lic or OP.";
                                                    cnt_resp++;
                                                    isError = true;
                                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Designation should be either Pro or Lic or OP.", Logging.LogPriority.Error);
                                                }
                                            }
                                        }

                                        if (currentTaskName == Constants.RegistrationTaskName.ReceiveWaiverAcknowledgementFromSIDODD)
                                        {
                                            List<SqlParameter> sqlParmsTR = new List<SqlParameter>();
                                            sqlParmsTR.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, RegID, false));
                                            sqlParmsTR.Add(SqlParms.CreateParameter("MEDICAID_PK", DbType.Int32, regServiceLocationID, false));
                                            DataSet dsRegTR = DataAccess.ExecuteStoredProcedure("usp_SelectTRANSACTION_QUEUE", sqlParmsTR, "TRData");
                                            dsRegTR.Tables[0].TableName = "TRData";
                                            DataRow drRegTR = Methods.HasRows(dsRegTR) ? dsRegTR.Tables[0].Rows[0] : null;
                                            string SIAcknowledgment = Methods.GetString("SI_ACK_RESPONSE_CODE", drRegTR);
                                            string MITSAcknowledgement = Methods.GetString("ACK_RESPONSE_CODE", drRegTR);

                                            if (string.IsNullOrEmpty(MITSAcknowledgement) || (!string.IsNullOrEmpty(MITSAcknowledgement) && !MITSAcknowledgement.Equals(Constants.ResponseCodes.MITS_Acknowledgment_SUCCESS)))
                                            {
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3001"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request cannot be processed as the PSM Acknowledgement is not received/processed.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Request cannot be processed as the PSM Acknowledgement is not received/processed.", Logging.LogPriority.Error);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Payload.ProviderInformation.ProviderServices != null && (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || (applicationType == "Manual Update" || applicationType == "Bulk Update")))
                {
                    //DataSet dsSvc = DataAccess.ExecuteStoredProcedure("usp_SelectWaiverServices"); OHPNM-18762 to be enabled later after ODA fixes their service names.
                    foreach (PartialProviderServicesList lst in Payload.ProviderInformation.ProviderServices)
                    {
                        if (string.IsNullOrEmpty(lst.ProviderService))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services - ProviderService cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " -  Provider Services - ProviderService cannot be blank.", Logging.LogPriority.Error);
                        }
                        //else
                        //{
                        //    bool isValidService = dsSvc.Tables[0].AsEnumerable()
                        //                     .Where(r => r.Field<string>("SERVICE_NAME").ToLower() == lst.ProviderService.ToLower())
                        //                     .Any();

                        //    if (!isValidService)
                        //    {
                        //        resp[cnt_resp] = new PartialProviderResponse();
                        //        resp[cnt_resp].ResponseCode = "3002"; //error
                        //        resp[cnt_resp].ResponseType = "Failure";
                        //        resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services - Service Name not configured in PNM.";
                        //        cnt_resp++;
                        //        isError = true;
                        //        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " -  Provider Services - Service Name not configured in PNM.", Logging.LogPriority.Error);
                        //    }
                        //}

                        if (string.IsNullOrEmpty(lst.EffectiveDate) || string.IsNullOrEmpty(lst.EndDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services - Effective or End Date cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Provider Services - Effective or End Date cannot be blank.", Logging.LogPriority.Error);
                        }
                        if (!string.IsNullOrEmpty(lst.EffectiveDate) && !(string.IsNullOrEmpty(lst.EndDate)))
                        {
                            DateTime effectiveDate;
                            DateTime endDate;

                            if (!DateTime.TryParse(lst.EffectiveDate, out effectiveDate))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services - Effective Date is not a valid date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Provider Services -  Effective Date is not a valid date.", Logging.LogPriority.Error);
                            }
                            if (!DateTime.TryParse(lst.EndDate, out endDate))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services -  End Date is not a valid date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Provider Services -  End Date is not a valid date.", Logging.LogPriority.Error);
                            }

                            if (endDate < effectiveDate)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - Provider Services - End Date cannot be lesser than Effective Date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - Provider Services - End Date cannot be lesser than Effective Date.", Logging.LogPriority.Error);
                            }
                        }


                    }
                }
                if (Payload.ProviderInformation.ProviderOwnerships != null && (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || (applicationType == "Manual Update" || applicationType == "Bulk Update")))
                {
                    foreach (PartialProviderOwnershipList lst in Payload.ProviderInformation.ProviderOwnerships)
                    {
                        PartialOwnerOwnerIndividual lstInd = new PartialOwnerOwnerIndividual();
                        PartialOwnerOwnerOrganization lstOrg = new PartialOwnerOwnerOrganization();

                        if (!string.IsNullOrEmpty(lst.OwnershipDetails.OwnerType))
                        {
                            if (lst.OwnershipDetails.Item != null)
                            {
                                if (lst.OwnershipDetails.OwnerType == "RI" || lst.OwnershipDetails.OwnerType == "I" || lst.OwnershipDetails.OwnerType == "SI" || lst.OwnershipDetails.OwnerType == "SUI") // individual
                                {
                                    if (lstInd.GetType() != lst.OwnershipDetails.Item.GetType())
                                    {
                                        resp[cnt_resp] = new PartialProviderResponse();
                                        resp[cnt_resp].ResponseCode = "3002"; //error
                                        resp[cnt_resp].ResponseType = "Failure";
                                        resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerIndividual node missing for OwnerType = I.";
                                        cnt_resp++;
                                        isError = true;
                                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerIndividual node missing for OwnerType = I", Logging.LogPriority.Error);
                                    }
                                    else
                                    {
                                        lstInd = (PartialOwnerOwnerIndividual)lst.OwnershipDetails.Item;
                                        if (lstInd.FirstName == null && lstInd.LastName == null)
                                        {
                                            resp[cnt_resp] = new PartialProviderResponse();
                                            resp[cnt_resp].ResponseCode = "3002"; //error
                                            resp[cnt_resp].ResponseType = "Failure";
                                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerIndividual node - FirstName and LastName tags are missing.";
                                            cnt_resp++;
                                            isError = true;
                                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerIndividual node - FirstName and LastName tags are missing.", Logging.LogPriority.Error);
                                        }
                                        else if (string.IsNullOrEmpty(lstInd.FirstName) && string.IsNullOrEmpty(lstInd.LastName))
                                        {
                                            resp[cnt_resp] = new PartialProviderResponse();
                                            resp[cnt_resp].ResponseCode = "3002"; //error
                                            resp[cnt_resp].ResponseType = "Failure";
                                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerIndividual node - FirstName and LastName cannot be blank.";
                                            cnt_resp++;
                                            isError = true;
                                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerIndividual node - FirstName and LastName cannot be blank..", Logging.LogPriority.Error);
                                        }
                                        if (lstInd.Title != null && !string.IsNullOrEmpty(lstInd.Title))
                                        {
                                            // check title for individual
                                            if (!isValidTitle(lstInd.Title))
                                            {
                                                // title is not valid
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3002"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerIndividual node - Title is not a valid type.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerIndividual node - Title is not a valid type.", Logging.LogPriority.Error);
                                            }
                                        }
                                    }

                                }
                                else if (lst.OwnershipDetails.OwnerType == "O" || lst.OwnershipDetails.OwnerType == "RO" || lst.OwnershipDetails.OwnerType == "SO" || lst.OwnershipDetails.OwnerType == "SUO" || lst.OwnershipDetails.OwnerType == "OP")
                                {
                                    if (lstOrg.GetType() != lst.OwnershipDetails.Item.GetType())
                                    {
                                        resp[cnt_resp] = new PartialProviderResponse();
                                        resp[cnt_resp].ResponseCode = "3002"; //error
                                        resp[cnt_resp].ResponseType = "Failure";
                                        resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerOrganization node missing for OwnerType = O.";
                                        cnt_resp++;
                                        isError = true;
                                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerOrganization node missing for OwnerType = O.", Logging.LogPriority.Error);
                                    }
                                    else
                                    {
                                        lstOrg = (PartialOwnerOwnerOrganization)lst.OwnershipDetails.Item;
                                        if (lstOrg.OwnerOrgName == null)
                                        {
                                            resp[cnt_resp] = new PartialProviderResponse();
                                            resp[cnt_resp].ResponseCode = "3002"; //error
                                            resp[cnt_resp].ResponseType = "Failure";
                                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerOrganization node - OrganizationName tag is missing.";
                                            cnt_resp++;
                                            isError = true;
                                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerOrganization node - OrganizationName tag is missing.", Logging.LogPriority.Error);
                                        }
                                        else if (string.IsNullOrEmpty(lstOrg.OwnerOrgName))
                                        {
                                            resp[cnt_resp] = new PartialProviderResponse();
                                            resp[cnt_resp].ResponseCode = "3002"; //error
                                            resp[cnt_resp].ResponseType = "Failure";
                                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerOrganization node - OrganizationName cannot be blank.";
                                            cnt_resp++;
                                            isError = true;
                                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerOrganization node - OrganizationName cannot be blank.", Logging.LogPriority.Error);
                                        }
                                        if (lstOrg.Title != null && !string.IsNullOrEmpty(lstOrg.Title))
                                        {
                                            // checking title for org
                                            if (!isValidTitle(lstOrg.Title))
                                            {
                                                // org title is not valid
                                                resp[cnt_resp] = new PartialProviderResponse();
                                                resp[cnt_resp].ResponseCode = "3002"; //error
                                                resp[cnt_resp].ResponseType = "Failure";
                                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerOrganization node - Title is not a valid type.";
                                                cnt_resp++;
                                                isError = true;
                                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerOrganization node - Title is not a valid type.", Logging.LogPriority.Error);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - Invalid Owner Type.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - Invalid Owner Type.", Logging.LogPriority.Error);
                                }
                                if (string.IsNullOrEmpty(lst.OwnershipDetails.EffectiveDate))
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - EffectiveDate cannot be blank.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - EffectiveDate cannot be blank.", Logging.LogPriority.Error);
                                }
                                else if (!IsValidSqlDatetime(lst.OwnershipDetails.EffectiveDate))
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - EffectiveDate is not a valid Date.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - EffectiveDate is not a valid Date.", Logging.LogPriority.Error);
                                }

                                if (string.IsNullOrEmpty(lst.OwnershipDetails.EndDate))
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - EndDate cannot be blank.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - EndDate cannot be blank.", Logging.LogPriority.Error);
                                }
                                else if (!IsValidSqlDatetime(lst.OwnershipDetails.EndDate))
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - End Date is not a valid Date.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - End Date is not a valid Date.", Logging.LogPriority.Error);
                                }
                            }
                            else
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerOrganization or OwnerIndivdual node is missing.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerOrganization or OwnerIndivdual node is missing.", Logging.LogPriority.Error);
                            }
                        }
                        else
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerType field cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerType field cannot be blank.", Logging.LogPriority.Error);
                        }
                        if (lst.OwnerAddress == null)
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderOwnerships - OwnerAddress cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderOwnerships - OwnerAddress cannot be blank.", Logging.LogPriority.Error);
                        }
                    }
                }

                if (Payload.ProviderInformation.ProviderAddress != null && (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || applicationType == "Manual Update" || applicationType == "Bulk Update"))
                {
                    int addrCnt = 0;
                    DataSet dsStates = DataAccess.ExecuteStoredProcedure("usp_SelectStates");
                    DataSet dsCounty = DataAccess.ExecuteStoredProcedure("usp_Get_County");
                    foreach (PartialProviderAddress lst in Payload.ProviderInformation.ProviderAddress)
                    {
                        if (lst.AddressType == null || string.IsNullOrEmpty(lst.AddressType))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - AddressType field cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - AddressType field cannot be blank.", Logging.LogPriority.Error);
                        }
                        else if (applicationType == "Initial" || (applicationType != "Initial" && lst.AddressType == "P"))  // if application type is not initial only validate the Billing Address details
                        {
                            if (lst.AddressNameTypeIndicator == null || string.IsNullOrEmpty(lst.AddressNameTypeIndicator))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - AddressNameTypeIndicator field cannot be blank.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - AddressNameTypeIndicator field cannot be blank.", Logging.LogPriority.Error);
                            }
                            else if (string.IsNullOrEmpty(lst.AddressLine1) || string.IsNullOrEmpty(lst.City) || string.IsNullOrEmpty(lst.CountyCode) || string.IsNullOrEmpty(lst.State) || string.IsNullOrEmpty(lst.ZipCode5))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - AddressLine1 or City or State or ZipCode5 or CountyCode fields cannot be blank.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - AddressNameTypeIndicator field cannot be blank.", Logging.LogPriority.Error);
                            }
                            if (!string.IsNullOrEmpty(lst.State))
                            {
                                //string reqSystem = (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM) ? "PSM" : (Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW) ? "PCW" : string.Empty;
                                bool isValidState = dsStates.Tables[0].AsEnumerable()
                                              .Where(r => r.Field<string>("StateId").ToLower() == lst.State.Trim().ToLower())
                                              .Any();

                                if (!isValidState)
                                {
                                    resp[cnt_resp] = new PartialProviderResponse();
                                    resp[cnt_resp].ResponseCode = "3002"; //error
                                    resp[cnt_resp].ResponseType = "Failure";
                                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - State not configured in PNM.";
                                    cnt_resp++;
                                    isError = true;
                                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - State not configured in PNM.", Logging.LogPriority.Error);
                                }
                                if (!string.IsNullOrEmpty(lst.CountyCode))
                                {
                                    bool isValidCounty = dsCounty.Tables[0].AsEnumerable()
                                             .Where(r => r.Field<string>("STATE_ABBREVIATION").ToLower() == lst.State.Trim().ToLower() &&
                                                      r.Field<string>("MMIS_COUNTY_CODE").ToLower() == lst.CountyCode.ToLower())
                                             .Any();

                                    if (!isValidCounty)
                                    {
                                        resp[cnt_resp] = new PartialProviderResponse();
                                        resp[cnt_resp].ResponseCode = "3002"; //error
                                        resp[cnt_resp].ResponseType = "Failure";
                                        resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - County Code not configured in PNM.";
                                        cnt_resp++;
                                        isError = true;
                                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - County Code not configured in PNM.", Logging.LogPriority.Error);
                                    }
                                }
                            }
                            if (applicationType == "Initial" && ((PayloadApplicationStatus.ToLower() == "recommended certification" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW)
                                || (PayloadApplicationStatus.ToLower() == "pending external medicaid approval" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM)))
                            {
                                if (lst.AddressType == "S" || lst.AddressType == "P" || lst.AddressType == "H" || lst.AddressType == "M")
                                {
                                    addrCnt++;
                                }
                            }
                        }
                    }
                    if (applicationType == "Initial" && ((PayloadApplicationStatus.ToLower() == "recommended certification" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW)
                                || (PayloadApplicationStatus.ToLower() == "pending external medicaid approval" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM))
                                && addrCnt < 4)
                    {
                        resp[cnt_resp] = new PartialProviderResponse();
                        resp[cnt_resp].ResponseCode = "3002"; //error
                        resp[cnt_resp].ResponseType = "Failure";
                        resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - S/P/M/H address types are required on Initial Applications.";
                        cnt_resp++;
                        isError = true;
                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - S/P/M/H address types are required on Initial Applications.", Logging.LogPriority.Error);
                    }

                }
                if (Payload.ProviderInformation.ProviderAddress == null &&
                    ((PayloadApplicationStatus.ToLower() == "recommended certification" && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW)
                      || (PayloadApplicationStatus.ToLower() == "pending external medicaid approval"
                          && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM
                          && (applicationType.ToLower() == "initial" || applicationType.ToLower() == "renewal" || applicationType.ToLower() == "demographic update - billing address"))))
                {
                    resp[cnt_resp] = new PartialProviderResponse();
                    resp[cnt_resp].ResponseCode = "3002"; //error
                    resp[cnt_resp].ResponseType = "Failure";
                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress node cannot be blank.";
                    cnt_resp++;
                    isError = true;
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress node cannot be blank..", Logging.LogPriority.Error);
                }

                if (Payload.ProviderInformation.ProviderManagedEmployees != null && (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || applicationType == "Manual Update" || applicationType == "Bulk Update"))
                {
                    foreach (PartialProviderManagedEmployeesListProviderManagedEmployee lst in Payload.ProviderInformation.ProviderManagedEmployees)
                    {
                        if ((string.IsNullOrEmpty(lst.ManagedEmployee.FirstName) && string.IsNullOrEmpty(lst.ManagedEmployee.LastName)) ||
                                string.IsNullOrEmpty(lst.ManagedEmployee.SSN) || string.IsNullOrEmpty(lst.ManagedEmployee.ManageEmployeesType))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - FirstName and LastName, SSN, ManageEmployeesType fields cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - FirstName and LastName, SSN, ManageEmployeesType fields cannot be blank.", Logging.LogPriority.Error);
                        }
                        else if (string.IsNullOrEmpty(lst.ManagedEmployee.EffectiveDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - EffectiveDate cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - EffectiveDate cannot be blank.", Logging.LogPriority.Error);
                        }
                        else if (lst.ManagedEmployeeAddress == null)
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - ManagedEmployeeAddress fields cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - ManagedEmployeeAddress cannot be blank.", Logging.LogPriority.Error);
                        }
                        if (string.IsNullOrEmpty(lst.ManagedEmployee.ManageEmployeesInd))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - ManageEmployeesInd cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - ManageEmployeesInd cannot be blank.", Logging.LogPriority.Error);
                        }
                        else if (lst.ManagedEmployee.ManageEmployeesInd != "Y" && lst.ManagedEmployee.ManageEmployeesInd != "N")
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - ManageEmployeesInd should be Y/N.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - ManageEmployeesInd should be Y/N.", Logging.LogPriority.Error);
                        }
                        if (string.IsNullOrEmpty(lst.ManagedEmployee.EndDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - EndDate cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - EndDate cannot be blank.", Logging.LogPriority.Error);
                        }
                        if (!IsValidSqlDatetime(lst.ManagedEmployee.EffectiveDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - EffectiveDate is not a valid Date.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - EffectiveDate is not a valid Date.", Logging.LogPriority.Error);
                        }
                        if (!IsValidSqlDatetime(lst.ManagedEmployee.EndDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderManagedEmployee - EndDate is not a valid Date.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderManagedEmployee - EndDate is not a valid Date.", Logging.LogPriority.Error);
                        }
                    }
                }

                if (Payload.ProviderInformation.OwnerRelationship != null && IsApplicationStatusOkToValidateData(PayloadApplicationStatus))
                {
                    foreach (PartialOwnerRelationshipList lst in Payload.ProviderInformation.OwnerRelationship)
                    {
                        if (string.IsNullOrEmpty(lst.Item) || string.IsNullOrEmpty(lst.Item1) || string.IsNullOrEmpty(lst.RelationType)
                            || string.IsNullOrEmpty(lst.EffectiveDate) || string.IsNullOrEmpty(lst.EndDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - OwnerRelationship - Owner1SSN, Owner2SSN, RelationType,EffectiveDate or EndDate cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - OwnerRelationship - Owner1SSN, Owner2SSN, RelationType,EffectiveDate or EndDate cannot be blank.", Logging.LogPriority.Error);
                        }
                    }
                }

                if (Payload.ProviderInformation.ProviderDemographics != null)
                {
                    if (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || applicationType == "Manual Update" || applicationType == "Bulk Update")
                    {
                        if (string.IsNullOrEmpty(Payload.ProviderInformation.ProviderDemographics.EntityType))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics - EntityType field cannot be blank.";
                            cnt_resp++;
                            isError = true;
                        }
                        else
                        {
                            if (Payload.ProviderInformation.ProviderDemographics.EntityType == "O" && string.IsNullOrEmpty(Payload.ProviderInformation.ProviderDemographics.OrganizationBusinessName))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics - EntityType is O then OrganizationBusinessName field cannot be blank.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics - EntityType is O then OrganizationBusinessName field cannot be blank.", Logging.LogPriority.Error);
                            }
                            if (Payload.ProviderInformation.ProviderDemographics.EntityType == "I" && (string.IsNullOrEmpty(Payload.ProviderInformation.ProviderDemographics.FirstName) || string.IsNullOrEmpty(Payload.ProviderInformation.ProviderDemographics.LastName)))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics - EntityType is I then FirstName or LastName fields cannot be blank.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics - EntityType is I then FirstName or LastName fields cannot be blank.", Logging.LogPriority.Error);
                            }
                        }
                        string strEffectiveDate = Payload.ProviderInformation.ProviderDemographics.EffectiveDate;
                        string strEndDate = Payload.ProviderInformation.ProviderDemographics.EndDate;

                        if (!string.IsNullOrEmpty(strEffectiveDate) && !(string.IsNullOrEmpty(strEndDate)))
                        {
                            if (!IsValidSqlDatetime(strEffectiveDate))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics -Effective Date is not a valid date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics -Effective Date is not a valid date.", Logging.LogPriority.Error);
                            }
                            if (!IsValidSqlDatetime(strEndDate))
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics -End Date is not a valid date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics -End Date is not a valid date.", Logging.LogPriority.Error);
                            }
                            DateTime effectiveDate = DateTime.Parse(strEffectiveDate);
                            DateTime endDate = DateTime.Parse(strEndDate);
                            if (endDate < effectiveDate)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics -End Date cannot be lesser than Effective Date.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics -End Date cannot be lesser than Effective Date.", Logging.LogPriority.Error);
                            }
                        }
                    }
                }
                else
                {
                    resp[cnt_resp] = new PartialProviderResponse();
                    resp[cnt_resp].ResponseCode = "3002"; //error
                    resp[cnt_resp].ResponseType = "Failure";
                    resp[cnt_resp].ResponseMessage = "Request has errors - ProviderDemographics node cannot be blank.";
                    cnt_resp++;
                    isError = true;
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderDemographics node cannot be blank..", Logging.LogPriority.Error);
                }

                //OHPNM-11873 - validation for language - effective date
                if (Payload.ProviderInformation.ProviderLanguage != null && (IsApplicationStatusOkToValidateData(PayloadApplicationStatus) || applicationType == "Manual Update" || applicationType == "Bulk Update"))
                {
                    DataSet dsLang = DataAccess.ExecuteStoredProcedure("usp_SelectLANGUAGES_SPOKEN");
                    foreach (PartialProviderLanguageList lst in Payload.ProviderInformation.ProviderLanguage)
                    {
                        if (string.IsNullOrEmpty(lst.Language))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderLanguage - Language cannot be blank.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderLanguage - Language cannot be blank.", Logging.LogPriority.Error);
                        }
                        else
                        {
                            bool isValidLang = dsLang.Tables[0].AsEnumerable()
                                                  .Where(r => r.Field<string>("MMIS_LANGUAGE_ID").ToLower() == lst.Language.Trim().ToLower())
                                                  .Any();

                            if (!isValidLang)
                            {
                                resp[cnt_resp] = new PartialProviderResponse();
                                resp[cnt_resp].ResponseCode = "3002"; //error
                                resp[cnt_resp].ResponseType = "Failure";
                                resp[cnt_resp].ResponseMessage = "Request has errors - ProviderLanguage - Language not configured in PNM.";
                                cnt_resp++;
                                isError = true;
                                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderLanguage - Language not configured in PNM.", Logging.LogPriority.Error);
                            }
                        }

                        if (string.IsNullOrEmpty(lst.EffectiveDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderLanguage EffectiveDate field cannot be blank.";     //OHPNM-14433 - fix the response message
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderLanguage EffectiveDate cannot be blank", Logging.LogPriority.Error);
                        }
                        if (!IsValidSqlDatetime(lst.EffectiveDate))
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderLanguage EffectiveDate is not a valid date.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderLanguage EffectiveDate is not a valid date.", Logging.LogPriority.Error);
                        }
                    }
                }

                //OHPNM - 13211 CR387 - For Waiver faclities DODD will not send facility number. So commenting the validation.
                //Add Validation for closure

                if ((PayloadApplicationStatus.ToLower() == "pending review" || PayloadApplicationStatus.ToLower() == "closed as complete")
                     && isClosure && string.IsNullOrEmpty(facilityNumber) && !enableCR387)
                {
                    resp[cnt_resp] = new PartialProviderResponse();
                    resp[cnt_resp].ResponseCode = "3002"; //error
                    resp[cnt_resp].ResponseType = "Failure";
                    resp[cnt_resp].ResponseMessage = "Request has errors - facility ID is blank for Closure.";
                    cnt_resp++;
                    isError = true;
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + "  - facility ID is blank for Closure.", Logging.LogPriority.Error);


                }

                if (Payload.ProviderInformation.ProviderAddress != null && Payload.ProviderInformation.ProviderAddress.Count() > 0)
                {
                    foreach (PartialProviderAddress lst in Payload.ProviderInformation.ProviderAddress)
                    {
                        bool isExist = LookupTableController.DoesStateAbbrevExist(lst.State);

                        if (!isExist)
                        {
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "3002"; //error
                            resp[cnt_resp].ResponseType = "Failure";
                            resp[cnt_resp].ResponseMessage = "Request has errors - ProviderAddress - State not configured in PNM.";
                            cnt_resp++;
                            isError = true;
                            Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - ProviderAddress - State not configured in PNM.", Logging.LogPriority.Error);
                        }
                    }
                }

                if (!isError)
                {
                    ///////////////////////////////////////////////////////////////////////////
                    // No errors have been found; stage the payload now

                    // Insert STG_ProviderInformation
                    ProviderInformationID = InsertSTG_ProviderInformation(transactionID);
                    rtn.ODSProviderDemographicsId = ProviderInformationID.ToString();
                    //Insert STG_ProviderDemographics
                    InsertSTG_ProviderDemographics(ProviderInformationID, Payload);
                    //Insert Provider Type
                    InsertSTG_ProviderType(ProviderInformationID, Payload);
                    //Insert STG_ProviderReviews
                    InsertSTG_ProviderReviews(ProviderInformationID, Payload);
                    //Insert STG_ProviderAddress
                    InsertSTG_ProviderAddress(ProviderInformationID, Payload);
                    //Insert STG_ProviderApplication
                    Insertstg_ProviderApplication(ProviderInformationID, Payload);
                    //Insert STG_ProviderAlternameIdentifier
                    InsertSTG_ProviderAlternateIdentifier(ProviderInformationID, Payload);
                    //Insert STG_ProviderBusinessStatus
                    InsertSTG_ProviderBusinessStatus(ProviderInformationID, Payload);
                    //Insert STG_ServiceLocation, STG_License, STG_ProviderSpecialty, STG_ProviderTaxonomy, STG_ProviderCertification, STG_ProviderRestriction, STG_SiteVisit and STG_ProviderTraining
                    InsertSTG_ServiceLocation(ProviderInformationID, Payload);
                    //Insert STG_ProviderLanguage
                    InsertSTG_ProviderLanguage(ProviderInformationID, Payload);
                    // Insert STG_ProviderServices
                    InsertSTG_ProviderServices(ProviderInformationID, Payload);
                    //Insert STG_OwnerRelationship
                    InsertSTG_OwnerRelationship(ProviderInformationID, Payload);
                    //Insert STG_ProviderOwnership
                    InsertSTG_OwnerShipDetails(ProviderInformationID, Payload);
                    //Insert STG_ProviderManagedEmployee
                    InsertSTG_ManagedEmployee(ProviderInformationID, Payload);
                    // Insert STG_ProviderDocuments
                    string DocResponse = InsertSTG_ProviderDocuments(ProviderInformationID, RegID, Payload, PayloadRegID, PayloadApplicationID,
                    PayloadAddnApplicationID, moduleTransactionID);

                    // Complete inserting Staging Tables                                                        

                    // Insert or Update PNM Reg tables
                    if (DocResponse == "Success")
                    {
                        try
                        {

                            // CR386 OHPNM-13730 - if in active workflow and we receive a DODD manual update with EnrollmentBusinessStatusReasonCode as Not Revoked, we need to send the success response without processing the payload; Also we need to stage the payload and save it.
                            if (enableCR386 && CurrentStepID > 0 && WaiverServiceUpdateTypeID == Constants.WaiverServiceUpdateType.DODD && applicationType.ToLower() == "manual update" && EnrollmentBusinessStatusReasonCode != "Revoked")
                            {
                                // OHPNM-13730 - PNM will ignore DODD Manual Updates when provider is in a Workflow to prevent incorrect terminations
                                // need this if condition to skip the other conditions
                                // in active workflow for dodd manual update; skip all until good response part
                            }
                            else if (enableCR386 && updateClosedPartialApprovalLegalStatusReceived)
                            {
                                // OHPNM-13730 this is an "updated closed partial approval" payload with a legal status sent in the AlternateIdType "LGL" area; we need to:
                                // Do not reject or queue the payload , save the payload, update DODD Legal status and send success response (i.e. Stage the payload, Update the legal status and Send success response)
                                // this else statement allows us to skip the logic in the next few conditions and gets us right to the saving of the payload and sending a success response back
                                // first, we need to update the legal status

                                // the comment "Do not queue this payload or reject it for incorrect application Id" sounds like I shouldn't worry about the application id being wrong or null,
                                // so I should just update the legal status for all REG_ADDITIONAL_APPLICATION rows for this provider.
                                List<SqlParameter> paramLegalStatus = new List<SqlParameter>();
                                paramLegalStatus.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, PayloadRegID, false));
                                paramLegalStatus.Add(SqlParms.CreateParameter("LEGAL_STATUS", DbType.String, PayloadApplicationLegalStatus, false));
                                paramLegalStatus.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                paramLegalStatus.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                paramLegalStatus.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, false));
                                DataSet dsAppLegalStatus = DataAccess.ExecuteStoredProcedure("usp_UpdateRegAdditionalApplication", paramLegalStatus, "RegAppLegalStatus");
                            }
                            else if ((WorkflowEventTypeID == Constants.WorkflowEventType.NewReg && ApplicationTypeID == Constants.ApplicationType.Waiver
                                 && (WaiverTypeID == Constants.WaiverApplicationTypeID.ODA || WaiverTypeID == Constants.WaiverApplicationTypeID.DODD || WaiverTypeID == Constants.WaiverApplicationTypeID.NonMedicaidDODD))
                                 || Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW || ((applicationType == "Manual Update" || applicationType == "Bulk Update" || applicationType.ToLower() == "chop") && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM))
                            {
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("TransactionID", DbType.Int32, transactionID, true));
                                parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                parameters.Add(SqlParms.CreateParameter("RequestorSystem", DbType.String, Header.RequestorSystem.ToString(), true));
                                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, PayloadRegID, true));
                                parameters.Add(SqlParms.CreateParameter("ModifiedDate", DbType.DateTime, DateTime.Now, true));
                                parameters.Add(SqlParms.CreateParameter("ModifiedUser", DbType.Guid, appPDMSDataExchangeUserId, true));
                                parameters.Add(SqlParms.CreateParameter("LogThreadNumber", DbType.Guid, Log.ThreadId, true));
                                DataAccess.ExecuteStoredProcedure("usp_TransferSTGtoREG_PartialProvider", parameters);
                            }
                            else
                            {
                                string dd_contract_number = "";
                                foreach (PartialProviderAlternateIdList lst in Payload.ProviderInformation.ProviderAlternateIdentifiers)
                                {
                                    if (lst.AlternateIdType == "CON")
                                    {
                                        dd_contract_number = lst.AlternateId;
                                        break;
                                    }
                                }

                                //closure app start
                                bool isOperatorInitiated = false;
                                bool ProcessPayload = false;
                                int operatorRegId = 0;
                                int CurrStepID = 0;
                                if (isClosure)
                                {
                                    if (!string.IsNullOrEmpty(facilityNumber)) //OHPNM-13211
                                    {
                                        isOperatorInitiated = IsClosureInitiatedByOperator(PayloadRegID, mmisProviderTypeId);

                                        if (isOperatorInitiated)
                                        {
                                            ProcessPayload = true;
                                        }
                                        else
                                        {
                                            if (PayloadApplicationStatus.ToLower() == "closed as denied" || PayloadApplicationStatus.ToLower() == "expired"
                                                || PayloadApplicationStatus.ToLower() == "withdrawn by provider"
                                                || PayloadApplicationStatus.ToLower() == "approved" || PayloadApplicationStatus.ToLower() == "closed as complete")
                                            {
                                                operatorRegId = GetFacilityRegistrationByFacilityNumber(facilityNumber);
                                                if (operatorRegId > 0)
                                                {
                                                    DataSet dsOperator = GetRegidInfoByRegId(operatorRegId);
                                                    if (dsOperator != null && dsOperator.Tables[0] != null && dsOperator.Tables[0].Rows.Count > 0)
                                                    {
                                                        WorkflowID = Methods.GetIntValue(dsOperator.Tables[0].Rows[0], "WorkflowID");
                                                        CurrStepID = Methods.GetIntValue(dsOperator.Tables[0].Rows[0], "CurrentStepID");
                                                    }

                                                    ProcessPayload = WorkflowID == Constants.WorkflowType.RiskAlertClosure ? true : false;
                                                    //PayloadRegID = ProcessPayload ? operatorRegId : PayloadRegID;
                                                }
                                            }
                                            else
                                                ProcessPayload = false;
                                        }
                                    }
                                }

                                // closure app end

                                // If the payload received is for the additional application that the WF is processing now then process the payload here.
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("ProcessID", DbType.String, ProcessID, false));
                                DataSet dsProcess = DataAccess.ExecuteStoredProcedure("usp_WF_SelectProcessParameters", parameters, "ProcessDS");
                                int ProcessAdditionalAppID = 0;
                                if (dsProcess != null && dsProcess.Tables[0] != null && dsProcess.Tables[0].Rows.Count > 0)
                                {
                                    DataRow myRow = dsProcess.Tables[0].Rows[0];

                                    for (int j = 0; j < dsProcess.Tables[0].Columns.Count; j++)
                                    {
                                        if (dsProcess.Tables[0].Columns[j].ColumnName.ToString() == "ADDITIONAL_APPLICATION_ID")
                                        {
                                            if (!string.IsNullOrEmpty(myRow.ItemArray[j].ToString()))
                                            {
                                                ProcessAdditionalAppID = Convert.ToInt32(myRow.ItemArray[j].ToString());
                                                break;
                                            }
                                        }
                                    }
                                }
                                if ((ProcessAdditionalAppID != 0 && ProcessAdditionalAppID == PayloadAddnApplicationID)
                                    || (isClosure && ProcessPayload)) //Process the Payload
                                {
                                    parameters = new List<SqlParameter>();
                                    parameters.Add(SqlParms.CreateParameter("TransactionID", DbType.Int32, transactionID, true));
                                    parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                    parameters.Add(SqlParms.CreateParameter("RequestorSystem", DbType.String, Header.RequestorSystem.ToString(), true));
                                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, PayloadRegID, true));
                                    parameters.Add(SqlParms.CreateParameter("ModifiedDate", DbType.DateTime, DateTime.Now, true));
                                    parameters.Add(SqlParms.CreateParameter("ModifiedUser", DbType.Guid, appPDMSDataExchangeUserId, true));
                                    parameters.Add(SqlParms.CreateParameter("LogThreadNumber", DbType.Guid, Log.ThreadId, true));
                                    DataAccess.ExecuteStoredProcedure("usp_TransferSTGtoREG_PartialProvider", parameters);
                                }
                                else //if (!ProcessPayload)
                                {
                                    parameters = new List<SqlParameter>();
                                    parameters.Add(SqlParms.CreateParameter("TransactionID", DbType.Int32, transactionID, true));
                                    parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                    parameters.Add(SqlParms.CreateParameter("RequestorSystem", DbType.String, Header.RequestorSystem, true));
                                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.String, PayloadRegID, true));
                                    parameters.Add(SqlParms.CreateParameter("Application_Id", DbType.String, PayloadApplicationID, true));
                                    parameters.Add(SqlParms.CreateParameter("waiver_transaction_status_id", DbType.Int32, Constants.WaiverTransactionStatusType.NotProcessed, true));
                                    parameters.Add(SqlParms.CreateParameter("DODD_contract_number", DbType.String, dd_contract_number, true));
                                    parameters.Add(SqlParms.CreateParameter("ModifiedDate", DbType.DateTime, DateTime.Now, true));
                                    parameters.Add(SqlParms.CreateParameter("ModifiedUser", DbType.Guid, appPDMSDataExchangeUserId, true));
                                    parameters.Add(SqlParms.CreateParameter("DD_facility_number", DbType.String, facilityNumber, true));
                                    parameters.Add(SqlParms.CreateParameter("WorkflowID", DbType.Int32, WorkflowID, true));
                                    DataAccess.ExecuteStoredProcedure("insertWAIVER_TRANSACTION_QUEUECustom", parameters);
                                }
                            }
                            resp[cnt_resp] = new PartialProviderResponse();
                            resp[cnt_resp].ResponseCode = "1001";
                            resp[cnt_resp].ResponseMessage = "Success";
                            resp[cnt_resp].ResponseDetails = "Success";
                            resp[cnt_resp].ResponseType = "Success";

                            rtn.Response = resp;
                            // Insert the request and response
                            SavePartialReqRes(Header, xml, rtn);

                            if (sendAcknowledgment)
                            {
                                SendAcknowledgement(Header, rtn);
                            }
                            if (applicationType == "Manual Update" && (EnrollmentBusinessStatusReasonCode == "Revoke" || EnrollmentBusinessStatusReasonCode == "Revoked") && Header.RequestorSystem == InqMessageHeaderRequestorSystem.PSM)
                            {
                                string provName = Payload.ProviderInformation.ProviderDemographics.EntityType == "O" ? Payload.ProviderInformation.ProviderDemographics.OrganizationBusinessName
                                                                           : Payload.ProviderInformation.ProviderDemographics.FirstName + " " + Payload.ProviderInformation.ProviderDemographics.LastName;
                                SendEmailComplianceSpecialist(PayloadRegID, provName);
                            }
                            //Log.CreateLogEntry("Inbound Partial Provider complete", Logging.LogPriority.Information);
                            return rtn;
                        }
                        catch (Exception ex)
                        {
                            ExceptionMessage = !string.IsNullOrEmpty(ExceptionMessage) ? ExceptionMessage : "Error while processing the request";
                            LogMessage = "SP - " + ex.Message;
                            throw ex;
                        }

                    }
                    else
                    {   // Errored while saving documents
                        resp[cnt_resp] = new PartialProviderResponse();
                        resp[cnt_resp].ResponseCode = "3002"; //error
                        resp[cnt_resp].ResponseMessage = DocResponse;
                        resp[cnt_resp].ResponseType = "Failure";
                        rtn.Response = resp;
                        SavePartialReqRes(Header, xml, rtn);
                        if (sendAcknowledgment)
                        {
                            SendAcknowledgement(Header, rtn);
                        }
                        Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - " + DocResponse, Logging.LogPriority.Error);
                        return rtn;
                    }
                }
                else
                { // errored with validations
                    rtn.Response = resp;
                    SavePartialReqRes(Header, xml, rtn);
                    if (sendAcknowledgment)
                    {
                        SendAcknowledgement(Header, rtn);
                    }
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString(), Logging.LogPriority.Error);
                    return rtn;
                }
            }
            catch (Exception ex)
            {
                resp[cnt_resp] = new PartialProviderResponse();
                resp[cnt_resp].ResponseCode = "3002"; //error
                resp[cnt_resp].ResponseMessage = !string.IsNullOrEmpty(ExceptionMessage) ? ExceptionMessage : "Error while processing the request";
                resp[cnt_resp].ResponseType = "Failure";
                cnt_resp++;
                rtn.Response = resp;
                SavePartialReqRes(Header, xml, rtn);
                if (sendAcknowledgment && !errorAck)
                {
                    SendAcknowledgement(Header, rtn);
                }
                LogMessage = !string.IsNullOrEmpty(LogMessage) ? LogMessage : ex.Message + ex.StackTrace.ToString();
                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + transactionID.ToString() + " - " + LogMessage, Logging.LogPriority.Error);
                return rtn;
            }

            //Log.CreateLogEntry("Inbound Partial Provider complete", Logging.LogPriority.Information);
            //return rtn;            
        }

        private void SendAcknowledgement(MessageHeader Header, submitPartialProviderResponse rtn)
        {
            // Need to call Acknowledgement service if only the request is from PUB-SUB - Header.AdditionalModuleTransactionId contains string {PDMS}            
            try
            {
                errorAck = false;
                //Log.CreateLogEntry("Inbound Partial Provider Start Calling Acknowldegement Service.", Logging.LogPriority.Information);                 

                Corp.Core.Libraries.AcknowledgmentService.MessageHeader ackMsgHeader = new Corp.Core.Libraries.AcknowledgmentService.MessageHeader();
                ackMsgHeader.BusinessFlow = "PartialProviderManagement";
                ackMsgHeader.RequestorSystem = "PNM";
                ackMsgHeader.RequestorSystemId = "PNM";
                ackMsgHeader.TargetSystem = Header.RequestorSystem == InqMessageHeaderRequestorSystem.PCW ? "PCW" : "PSM";
                ackMsgHeader.ModuleTransactionId = Header.ModuleTransactionId;
                ackMsgHeader.AdditionalModuleTransactionId = rtn.AdditionalModuleTransactionId;
                ackMsgHeader.SITransactionKey = Header.SITransactionKey;
                ackMsgHeader.TimeStamp = ProviderManagementHelper.GetStringDateTime(DateTime.Now);

                Corp.Core.Libraries.AcknowledgmentService.Response[] ackReqResponse = new Corp.Core.Libraries.AcknowledgmentService.Response[100];
                int i = 0;
                foreach (PartialProviderResponse resp in rtn.Response)
                {
                    if (resp != null && resp.ResponseMessage != null)
                    {
                        ackReqResponse[i] = new Corp.Core.Libraries.AcknowledgmentService.Response();
                        ackReqResponse[i].SequenceNumber = i.ToString();
                        ackReqResponse[i].ResponseCode = resp.ResponseCode.ToString();
                        ackReqResponse[i].ResponseType = resp.ResponseType.ToString();
                        ackReqResponse[i].ResponseMessage = resp.ResponseMessage.ToString();
                        i++;
                    }
                    else
                        break;
                }

                vendorAcknowledgmentRequest1 ackRequest = new vendorAcknowledgmentRequest1();
                ackRequest.MessageHeader = ackMsgHeader;
                ackRequest.Response = ackReqResponse;

                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(vendorAcknowledgmentRequest1));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, ackRequest, emptyNs);
                string ackRequestxml = stream2.ToString();
                var ackresponse = string.Empty;

                try
                {
                    //Log.CreateLogEntry("Inbound Partial Provider sending acknowledgement request.-" + ackRequestxml.ToString(), Logging.LogPriority.Information);

                    AcknowledgmentReqRes ack = new AcknowledgmentReqRes();
                    ackresponse = ack.AcknowledgmentTargetVendorResponse(ackRequest);
                    //Log.CreateLogEntry("Inbound Partial Provider receive Acknowldegement Service response.-" + ackresponse.ToString(), Logging.LogPriority.Information);

                }
                catch (Exception ex)
                {
                    ExceptionMessage = "Error in receiving response from Acknowledgement Service";
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + Header.ModuleTransactionId.ToString() + " receiving response from Acknowledgement service: " + ex.Message, Logging.LogPriority.Error);
                    throw ex;
                }
                try
                {
                    // Insert into table for Acknowlegdement queue to process
                    int transID = string.IsNullOrEmpty(Header.ModuleTransactionId) ? 0 : Convert.ToInt32(Header.ModuleTransactionId);
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(SqlParms.CreateParameter("MODULE_TRANSACTION_ID", DbType.Int32, transID, true));
                    parameters1.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, rtn.AdditionalModuleTransactionId, true));
                    parameters1.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, "PNM", true));
                    parameters1.Add(SqlParms.CreateParameter("TARGET_SYSTEM", DbType.String, Header.RequestorSystem.ToString(), true));
                    parameters1.Add(SqlParms.CreateParameter("IS_ACK_SENT", DbType.Boolean, true, true));
                    parameters1.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                    parameters1.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, true));
                    parameters1.Add(SqlParms.CreateParameter("PARTIAL_REQ_SITransactionKey", DbType.String, Header.SITransactionKey, true));
                    parameters1.Add(new SqlParameter("ACK_REQUEST_XML", SqlDbType.Xml)
                    {
                        Value = ackRequestxml
                    });
                    parameters1.Add(SqlParms.CreateParameter("ACK_RAW_RESPONSE", DbType.String, ackresponse, true));
                    DataAccess.ExecuteStoredProcedure("insertINBOUND_PARTIAL_PROVIDER_ACKNOWLEDGMENT_DTLS", parameters1);

                    //Log.CreateLogEntry("Inbound Partial Provider Save Acknowledgement Response", Logging.LogPriority.Information);
                }
                catch (Exception ex)
                {
                    ExceptionMessage = "Error processing the request";
                    Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + Header.ModuleTransactionId.ToString() + " saving Acknowledgement details: " + ex.Message, Logging.LogPriority.Error);
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                errorAck = true;
                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + Header.ModuleTransactionId.ToString() + " sending Acknowledgement : " + ex.Message, Logging.LogPriority.Error);
                //ExceptionMessage = rtn.ResponseMessage;
                throw ex;
            }

        }



        private void SavePartialReqRes(MessageHeader Header, string requestPayload, submitPartialProviderResponse rtn)
        {
            //string xmlPayload = string.Empty;
            //xml = (responseCode == "3002" || responseCode == "3001") ? requestPayload : string.Empty;
            try
            {
                var doc = XDocument.Parse(requestPayload);
                var nodeToRemove = doc.Descendants().Where(o => o.Name.LocalName == "ContentBase64binary");
                nodeToRemove.Remove();
                requestPayload = doc.ToString();


                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(submitPartialProviderResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, rtn, emptyNs);
                string responsePayload = stream2.ToString();

                string apptype = (applicationType == "Manual Update" || applicationType == "Bulk Update") ? applicationType : string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("MODULE_TRANSACTION_ID", DbType.Int32, Header.ModuleTransactionId, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, Header.SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, Header.RequestorSystem.ToString(), true));
                parameters.Add(SqlParms.CreateParameter("SUBSCRIBER_SYSTEM", DbType.String, "PNM", true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, Header.AdditionalModuleTransactionId, true));
                parameters.Add(SqlParms.CreateParameter("ODSProviderDemographicsId", DbType.String, rtn.ODSProviderDemographicsId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, PayloadRegID, true));
                parameters.Add(SqlParms.CreateParameter("BULK_MANUAL_UPDATE", DbType.String, apptype, true));
                parameters.Add(new SqlParameter("REQUEST_PAYLOAD", SqlDbType.Xml)
                {
                    Value = requestPayload
                });
                parameters.Add(new SqlParameter("RESPONSE_PAYLOAD", SqlDbType.Xml)
                {
                    Value = responsePayload
                });
                DataAccess.ExecuteStoredProcedure("insertINBOUND_PARTIAL_PROVIDER_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                ExceptionMessage = "Error processing the request";
                Log.CreateLogEntry("Error in Inbound Partial Provider for TransactionID - " + Header.ModuleTransactionId.ToString() + " saving INBOUND_PARTIAL_PROVIDER_REQ_RES: " + ex.Message, Logging.LogPriority.Error);
                throw ex;
            }

        }
        private int InsertSTG_ProviderInformation(int transID)
        {
            try
            {
                int dataDirection = 2; // Response Received
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("TransactionQueueID", DbType.Int32, transID, true));
                parameters.Add(SqlParms.CreateParameter("Data_Direction_id", DbType.Int32, dataDirection, true));
                int ProviderInformationID = Convert.ToInt32(DataAccess.ExecuteScalar("insertSTG_ProviderInformation", parameters));
                return ProviderInformationID;
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the ProviderInformation node";
                LogMessage = "ProviderInformation - " + ex.Message;
                throw ex;
            }

        }
        private void InsertSTG_ProviderDemographics(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderDemographics != null)
                {
                    List<SqlParameter> parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                    parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.RecordStatusCode, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderId", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProviderId, true));
                    parameters.Add(SqlParms.CreateParameter("TaxId", DbType.String, Payload.ProviderInformation.ProviderDemographics.TaxId, true));
                    parameters.Add(SqlParms.CreateParameter("NationalProviderIdentifier", DbType.String, Payload.ProviderInformation.ProviderDemographics.NationalProviderIdentifier, true));
                    parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, Payload.ProviderInformation.ProviderDemographics.FirstName, true));
                    parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, Payload.ProviderInformation.ProviderDemographics.MiddleName, true));
                    parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, Payload.ProviderInformation.ProviderDemographics.LastName, true));
                    parameters.Add(SqlParms.CreateParameter("Prefix", DbType.String, Payload.ProviderInformation.ProviderDemographics.Prefix, true));
                    parameters.Add(SqlParms.CreateParameter("Suffix", DbType.String, Payload.ProviderInformation.ProviderDemographics.Suffix, true));
                    parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, Payload.ProviderInformation.ProviderDemographics.SSN, true));
                    parameters.Add(SqlParms.CreateParameter("Gender", DbType.String, Payload.ProviderInformation.ProviderDemographics.Gender, true));
                    parameters.Add(SqlParms.CreateParameter("EntityType", DbType.String, Payload.ProviderInformation.ProviderDemographics.EntityType, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderName", DbType.String, Payload.ProviderInformation.ProviderDemographics.OrganizationBusinessName, true));
                    parameters.Add(SqlParms.CreateParameter("LegalProviderName", DbType.String, Payload.ProviderInformation.ProviderDemographics.LegalProviderName, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderBusinessName", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProviderBusinessName, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderTaxName", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProviderTaxName, true));
                    parameters.Add(SqlParms.CreateParameter("TeachingIndicator", DbType.String, Payload.ProviderInformation.ProviderDemographics.TeachingIndicator, true));
                    parameters.Add(SqlParms.CreateParameter("OwnershipCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.OwnershipCode, true));
                    parameters.Add(SqlParms.CreateParameter("PracticeTypeCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.PracticeTypeCode, true));
                    parameters.Add(SqlParms.CreateParameter("ProviderProfitStatusCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProviderProfitStatusCode, true));
                    parameters.Add(SqlParms.CreateParameter("NewPatientIndicator", DbType.String, Payload.ProviderInformation.ProviderDemographics.NewPatientIndicator, true));
                    parameters.Add(SqlParms.CreateParameter("EnrollmentType", DbType.String, Payload.ProviderInformation.ProviderDemographics.EnrollmentType, true));
                    parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, Payload.ProviderInformation.ProviderDemographics.EffectiveDate, true));
                    parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, Payload.ProviderInformation.ProviderDemographics.EndDate, true));
                    parameters.Add(SqlParms.CreateParameter("DateOfBirth", DbType.DateTime, Payload.ProviderInformation.ProviderDemographics.DateOfBirth, true));
                    parameters.Add(SqlParms.CreateParameter("DateOfDeath", DbType.DateTime, Payload.ProviderInformation.ProviderDemographics.DateOfDeath, true));
                    parameters.Add(SqlParms.CreateParameter("Race", DbType.String, Payload.ProviderInformation.ProviderDemographics.Race, true));
                    parameters.Add(SqlParms.CreateParameter("MedicareInd", DbType.String, Payload.ProviderInformation.ProviderDemographics.MedicareInd, true));
                    parameters.Add(SqlParms.CreateParameter("PCPStatusCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.PCPStatusCode, true));
                    parameters.Add(SqlParms.CreateParameter("PCPTermDate", DbType.String, Payload.ProviderInformation.ProviderDemographics.PCPTermDate, true));
                    parameters.Add(SqlParms.CreateParameter("BirthCountry", DbType.String, Payload.ProviderInformation.ProviderDemographics.BirthCountry, true));
                    parameters.Add(SqlParms.CreateParameter("BirthCity", DbType.String, Payload.ProviderInformation.ProviderDemographics.BirthCity, true));
                    parameters.Add(SqlParms.CreateParameter("BirthState", DbType.String, Payload.ProviderInformation.ProviderDemographics.BirthState, true));
                    parameters.Add(SqlParms.CreateParameter("PaymentCode", DbType.String, string.Empty, true));     //Removed from latest wsdl 10/30
                    parameters.Add(SqlParms.CreateParameter("ProvRiskLevel", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProvRiskLevel, true));
                    parameters.Add(SqlParms.CreateParameter("ProvIHSAssoCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProvIHSAssoCode, true));
                    parameters.Add(SqlParms.CreateParameter("ProvDemographicsSrcKey", DbType.String, Payload.ProviderInformation.ProviderDemographics.ProvDemographicsSrcKey, true));
                    parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                    parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                    parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                    parameters.Add(SqlParms.CreateParameter("PaymentTypeCode", DbType.String, Payload.ProviderInformation.ProviderDemographics.PaymentTypeCode, true));
                    DataAccess.ExecuteStoredProcedure("insertSTG_ProviderDemographics", parameters);
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderDemographics";
                LogMessage = "ProviderDemographics - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderType(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderType != null)
                {
                    foreach (PartialProviderTypeList lst in Payload.ProviderInformation.ProviderType)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("ProviderTypeCode", DbType.String, lst.ProviderTypeCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("ProvTypeSrcKey", DbType.String, lst.ProvTypeSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderType", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderType";
                LogMessage = "ProviderType - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderReviews(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderReviews != null)
                {
                    foreach (PartialProviderReviewsList lst in Payload.ProviderInformation.ProviderReviews)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("ProviderReviewID", DbType.String, lst.ProviderReviewID, true));
                        parameters.Add(SqlParms.CreateParameter("ProposedEffectiveDate", DbType.DateTime, lst.ProposedEffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("ProviderReviewTypeCode", DbType.String, lst.ProviderReviewTypeCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("Provrvwsrccode", DbType.String, lst.Provrvwsrccode, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderReviews", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderReview";
                LogMessage = "ProviderReview - " + ex.Message;
                throw ex;
            }

        }
        private void InsertSTG_ProviderAddress(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderAddress != null)
                {
                    foreach (SubmitPartialProviderInformationAddress lst in Payload.ProviderInformation.ProviderAddress)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("AddressType", DbType.String, lst.AddressType, true));
                        parameters.Add(SqlParms.CreateParameter("AddressLine1", DbType.String, lst.AddressLine1, true));
                        parameters.Add(SqlParms.CreateParameter("AddressLine2", DbType.String, lst.AddressLine2, true));
                        parameters.Add(SqlParms.CreateParameter("City", DbType.String, lst.City, true));
                        parameters.Add(SqlParms.CreateParameter("State", DbType.String, lst.State, true));
                        parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lst.CountyCode, true));
                        parameters.Add(SqlParms.CreateParameter("ZipCode5", DbType.String, lst.ZipCode5, true));
                        parameters.Add(SqlParms.CreateParameter("ZipCode4", DbType.String, lst.ZipCode4, true));
                        parameters.Add(SqlParms.CreateParameter("Country", DbType.String, lst.Country, true));
                        parameters.Add(SqlParms.CreateParameter("BorderStateIndicator", DbType.String, lst.BorderStateIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("AddressNameTypeIndicator", DbType.String, lst.AddressNameTypeIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgAddressName", DbType.String, lst.ProvOrgAddressName, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgAddressFirstName", DbType.String, lst.ProvOrgAddressFirstName, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgAddressMiddleName", DbType.String, lst.ProvOrgAddressMiddleName, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgAddressLastName", DbType.String, lst.ProvOrgAddressLastName, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber1", DbType.String, lst.ProvOrgPhoneNumber1, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt1", DbType.String, lst.ProvOrgPhoneNumberExt1, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber2", DbType.String, lst.ProvOrgPhoneNumber2, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt2", DbType.String, lst.ProvOrgPhoneNumberExt2, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber1", DbType.String, lst.ProvOrgFaxNumber1, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber2", DbType.String, lst.ProvOrgFaxNumber2, true));
                        parameters.Add(SqlParms.CreateParameter("ProvAddressContactName", DbType.String, lst.ProvAddressContactName, true));
                        parameters.Add(SqlParms.CreateParameter("ProvEmail", DbType.String, lst.ProvEmail, true));
                        parameters.Add(SqlParms.CreateParameter("ProvAddressLongitude", DbType.String, lst.Longitude, true));
                        parameters.Add(SqlParms.CreateParameter("ProvAddressLatitude", DbType.String, lst.Latitude, true));
                        parameters.Add(SqlParms.CreateParameter("HandicapAccessibilityIndicator", DbType.String, lst.HandicapAccessibilityIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("ProvAddressSrcKey", DbType.String, lst.ProvAddressSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("reg_address_id", DbType.Int32, 0, true));      // Ask dhivya about this
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderAddress", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderAddress";
                LogMessage = "ProviderAddress - " + ex.Message;
                throw ex;
            }
        }
        private void Insertstg_ProviderApplication(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderApplication != null)
                {
                    foreach (PartialProviderApplication lst in Payload.ProviderInformation.ProviderApplication)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("ApplicationId", DbType.String, lst.ApplicationId, true));
                        parameters.Add(SqlParms.CreateParameter("AdditionalApplicationId", DbType.String, lst.AdditionalApplicationId, true));
                        parameters.Add(SqlParms.CreateParameter("ApplicationCreatedDate", DbType.DateTime, lst.ApplicationCreatedDate, true));
                        parameters.Add(SqlParms.CreateParameter("ApprovalDate", DbType.DateTime, lst.ApprovalDate, true));
                        parameters.Add(SqlParms.CreateParameter("StatePlanEnrollCode", DbType.String, lst.StatePlanEnrollCode, true));
                        parameters.Add(SqlParms.CreateParameter("EnrollMethodCode", DbType.String, lst.EnrollMethodCode, true));
                        parameters.Add(SqlParms.CreateParameter("ProvEnrollStatusCode", DbType.String, lst.ProvEnrollStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lst.CountyCode, true));
                        parameters.Add(SqlParms.CreateParameter("Non_MedicaidApplicationProviderType", DbType.String, lst.NonMedicaidApplicationProviderType, true));
                        parameters.Add(SqlParms.CreateParameter("Non_ApplicationIdAdditional", DbType.String, string.Empty, true)); //Removed from latest wsdl 10/30
                        parameters.Add(SqlParms.CreateParameter("ApplicationType", DbType.String, lst.ApplicationType, true));
                        parameters.Add(SqlParms.CreateParameter("ApplicationStatus", DbType.String, lst.ApplicationStatus, true));
                        parameters.Add(SqlParms.CreateParameter("ApplicationLegalStatus", DbType.String, lst.ApplicationLegalStatus, true));
                        parameters.Add(SqlParms.CreateParameter("ApplicationFeeConfirmation", DbType.String, lst.ApplicationFeeConfirmation, true));
                        parameters.Add(SqlParms.CreateParameter("SingleSignOnAssignedID", DbType.String, lst.SingleSignOnAssignedID, true));
                        parameters.Add(SqlParms.CreateParameter("SingleSignOnSelectedID", DbType.String, lst.SingleSignOnSelectedID, true));
                        parameters.Add(SqlParms.CreateParameter("AdditionalUserID", DbType.String, lst.AdditionalUserID, true));
                        parameters.Add(SqlParms.CreateParameter("SingleSignOnRole", DbType.String, lst.SingleSignOnRole, true));
                        parameters.Add(SqlParms.CreateParameter("EmailAddress", DbType.String, lst.EmailAddress, true));
                        parameters.Add(SqlParms.CreateParameter("ProvApplicationSrcKey", DbType.String, lst.ProvApplicationSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderApplication", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderApplication";
                LogMessage = "ProviderApplication - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderAlternateIdentifier(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderAlternateIdentifiers != null)
                {
                    foreach (PartialProviderAlternateIdList lst in Payload.ProviderInformation.ProviderAlternateIdentifiers)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("AlternateIdType", DbType.String, lst.AlternateIdType, true));
                        parameters.Add(SqlParms.CreateParameter("AlternateId", DbType.String, lst.AlternateId, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("ProvAlternateIdSrcKey", DbType.String, lst.ProvAlternateIdSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderAlternateIdentifier", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderAlternateIdentifier";
                LogMessage = "ProviderAlternateIdentifier - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderBusinessStatus(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderBusinessStatus != null)
                {
                    foreach (PartialProviderBusinessStatusList lst in Payload.ProviderInformation.ProviderBusinessStatus)
                    {
                        EnrollmentBusinessStatusReasonCode = lst.EnrollmentBusinessStatusReasonCode;
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("BusinessStatusCode", DbType.String, lst.BusinessStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("EnrollmentBusinessStatusReasonCode", DbType.String, lst.EnrollmentBusinessStatusReasonCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("ProvBsnsStatusSrcKey", DbType.String, lst.ProvBsnsStatusSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderBusinessStatus", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderBusinessStatus";
                LogMessage = "ProviderBusinessStatus - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ServiceLocation(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderServiceLocation != null)
                {
                    int cntr = 0;
                    foreach (PartialProviderServiceLocation lst in Payload.ProviderInformation.ProviderServiceLocation)
                    {

                        //List<SqlParameter> parameters = new List<SqlParameter>();
                        //parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        //parameters.Add(SqlParms.CreateParameter("SLRecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        //parameters.Add(SqlParms.CreateParameter("LocationName", DbType.String, lst.LocationName, true));
                        //parameters.Add(SqlParms.CreateParameter("LocationCode", DbType.String, lst.LocationCode, true));
                        //parameters.Add(SqlParms.CreateParameter("OrgStateOwnedIndicator", DbType.String, lst.OrgStateOwnedIndicator, true));
                        //parameters.Add(SqlParms.CreateParameter("SLEffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        //parameters.Add(SqlParms.CreateParameter("SLEndDate", DbType.DateTime, lst.EndDate, true));
                        //parameters.Add(SqlParms.CreateParameter("ProvSvcLocationSrcKey", DbType.String, lst.ProvSvcLocationSrcKey, true));
                        //Address Details
                        if (lst.ServiceLocationAddress != null)
                        {
                            foreach (PartialProviderServiceLocationAddress lst1 in lst.ServiceLocationAddress)
                            {
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                parameters.Add(SqlParms.CreateParameter("AddressRecordStatusCode", DbType.String, lst1.RecordStatusCode, true));
                                parameters.Add(SqlParms.CreateParameter("AddressType", DbType.String, lst1.AddressType, true));
                                parameters.Add(SqlParms.CreateParameter("AddressLine1", DbType.String, lst1.AddressLine1, true));
                                parameters.Add(SqlParms.CreateParameter("AddressLine2", DbType.String, lst1.AddressLine2, true));
                                parameters.Add(SqlParms.CreateParameter("City", DbType.String, lst1.City, true));
                                parameters.Add(SqlParms.CreateParameter("State", DbType.String, lst1.State, true));
                                parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lst1.CountyCode, true));
                                parameters.Add(SqlParms.CreateParameter("ZipCode5", DbType.String, lst1.ZipCode5, true));
                                parameters.Add(SqlParms.CreateParameter("ZipCode4", DbType.String, lst1.ZipCode4, true));
                                parameters.Add(SqlParms.CreateParameter("Country", DbType.String, lst1.Country, true));
                                parameters.Add(SqlParms.CreateParameter("BorderStateIndicator", DbType.String, lst1.BorderStateIndicator, true));
                                parameters.Add(SqlParms.CreateParameter("AddressNameTypeIndicator", DbType.String, lst1.AddressNameTypeIndicator, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgAddressName", DbType.String, lst1.ProvOrgAddressName, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgAddressFirstName", DbType.String, lst1.ProvOrgAddressFirstName, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgAddressMiddleName", DbType.String, lst1.ProvOrgAddressMiddleName, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgAddressLastName", DbType.String, lst1.ProvOrgAddressLastName, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber1", DbType.String, lst1.ProvOrgPhoneNumber1, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt1", DbType.String, lst1.ProvOrgPhoneNumberExt1, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber2", DbType.String, lst1.ProvOrgPhoneNumber2, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt2", DbType.String, lst1.ProvOrgPhoneNumberExt2, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber1", DbType.String, lst1.ProvOrgFaxNumber1, true));
                                parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber2", DbType.String, lst1.ProvOrgFaxNumber2, true));
                                parameters.Add(SqlParms.CreateParameter("ProvAddressContactName", DbType.String, lst1.ProvAddressContactName, true));
                                parameters.Add(SqlParms.CreateParameter("ProvEmail", DbType.String, lst1.ProvEmail, true));
                                parameters.Add(SqlParms.CreateParameter("ProvAddressLongitude", DbType.String, lst1.Longitude, true));
                                parameters.Add(SqlParms.CreateParameter("ProvAddressLatitude", DbType.String, lst1.Latitude, true));
                                parameters.Add(SqlParms.CreateParameter("HandicapAccessibilityIndicator", DbType.String, lst1.HandicapAccessibilityIndicator, true));
                                parameters.Add(SqlParms.CreateParameter("AddrEffectiveDate", DbType.DateTime, lst1.EffectiveDate, true));
                                parameters.Add(SqlParms.CreateParameter("AddrEndDate", DbType.DateTime, lst1.EndDate, true));
                                parameters.Add(SqlParms.CreateParameter("ProvAddressSrcKey", DbType.String, lst1.SvcLctnAdrsSrcKey, true));
                                parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                                parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                                parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                parameters.Add(SqlParms.CreateParameter("reg_address_id", DbType.Int32, 0, true));
                                DataAccess.ExecuteStoredProcedure("insertSTG_ServiceLocation_Custom", parameters);
                            }
                        }

                        // License, Provider Certification, Specialty, Taxonomy, Restriction, SiteVisit and Training under the ServiceLocation node are shown only for the first service location.
                        if (cntr == 0)
                        {
                            // Insert Licenses
                            if (lst.ProviderLicense != null)
                            {
                                foreach (PartialProviderServiceLocationLicense lstLicense in lst.ProviderLicense)
                                {
                                    List<SqlParameter> parameters = new List<SqlParameter>();
                                    parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                    parameters.Add(SqlParms.CreateParameter("LicenseType", DbType.String, lstLicense.LicenseType, true));
                                    parameters.Add(SqlParms.CreateParameter("LicenseNumber", DbType.String, lstLicense.LicenseNumber, true));
                                    parameters.Add(SqlParms.CreateParameter("LicenseeStateCode", DbType.String, lstLicense.LicenseeStateCode, true));
                                    parameters.Add(SqlParms.CreateParameter("LicenseIssueBoardNotes", DbType.String, lstLicense.LicenseIssueBoardNotes, true));
                                    parameters.Add(SqlParms.CreateParameter("Status", DbType.String, lstLicense.Status, true));
                                    parameters.Add(SqlParms.CreateParameter("StatusReason", DbType.String, lstLicense.StatusReason, true));
                                    parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lstLicense.CountyCode, true));
                                    parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lstLicense.EffectiveDate, true));
                                    parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lstLicense.EndDate, true));
                                    parameters.Add(SqlParms.CreateParameter("ProvLicenseSrcKey", DbType.String, lstLicense.ProvLicenseSrcKey, true));
                                    parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                                    parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                    parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                                    parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                    int stgProviderLicenseID = Convert.ToInt32(DataAccess.ExecuteScalar("insertSTG_License", parameters));

                                    // Insert License Certifications
                                    if (lstLicense.LicenseCertifications != null)
                                    {
                                        foreach (PartialLicenseCertificationsList lstLicCert in lstLicense.LicenseCertifications)
                                        {
                                            parameters = new List<SqlParameter>();
                                            parameters.Add(SqlParms.CreateParameter("LicenseID", DbType.Int32, stgProviderLicenseID, true));
                                            parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lstLicCert.RecordStatusCode, true));
                                            parameters.Add(SqlParms.CreateParameter("CertificationId", DbType.String, lstLicCert.CertificationId, true));
                                            parameters.Add(SqlParms.CreateParameter("CertificationOrg", DbType.String, lstLicCert.CertifyingOrg, true));
                                            parameters.Add(SqlParms.CreateParameter("CertificationFocus", DbType.String, lstLicCert.CertificationFocus, true));
                                            parameters.Add(SqlParms.CreateParameter("CertificationSpeciality", DbType.String, lstLicCert.CertificationSpeciality, true));
                                            parameters.Add(SqlParms.CreateParameter("CertifyingOrg", DbType.String, lstLicCert.CertifyingOrg, true));
                                            parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lstLicCert.EffectiveDate, true));
                                            parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lstLicCert.EndDate, true));
                                            parameters.Add(SqlParms.CreateParameter("ProvLicenseCertSrcKey", DbType.String, lstLicCert.ProvLicenseCertSrcKey, true));
                                            //parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                                            //parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                            //parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                                            //parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                            DataAccess.ExecuteStoredProcedure("insertSTG_LicenseCertifications", parameters);
                                        }
                                    }
                                }
                            }
                            if (lst.SiteVisits != null)
                            {
                                //Insert Site Visit
                                foreach (PartialVisitList lstSiteVisit in lst.SiteVisits)
                                {
                                    List<SqlParameter> parameters = new List<SqlParameter>();
                                    parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                    parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lstSiteVisit.RecordStatusCode, true));
                                    parameters.Add(SqlParms.CreateParameter("SiteVisitResult", DbType.String, lstSiteVisit.SiteVisitResult, true));
                                    parameters.Add(SqlParms.CreateParameter("SiteVisitText", DbType.String, lstSiteVisit.SiteVisitText, true));
                                    parameters.Add(SqlParms.CreateParameter("SiteVisitID", DbType.String, lstSiteVisit.SiteVisitID, true));
                                    parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lstSiteVisit.EffectiveDate, true));
                                    parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lstSiteVisit.EndDate, true));
                                    parameters.Add(SqlParms.CreateParameter("Provvstsrckey", DbType.String, lstSiteVisit.Provvstsrckey, true));
                                    DataAccess.ExecuteStoredProcedure("insertSTG_SiteVisit", parameters);
                                }
                            }
                            if (lst.ProviderFacility != null)
                            {
                                // Insert the Facility Number from ProviderFacilities
                                foreach (PartialProviderFacilityList lstFac in lst.ProviderFacility)
                                {
                                    if (!string.IsNullOrEmpty(lstFac.FacilityId))
                                    {
                                        List<SqlParameter> parameters = new List<SqlParameter>();
                                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lstFac.RecordStatusCode, true));
                                        parameters.Add(SqlParms.CreateParameter("FacilityID", DbType.String, lstFac.FacilityId, true));
                                        parameters.Add(SqlParms.CreateParameter("ProvFacilitysrckey", DbType.String, lstFac.ProvFacilitySrcKey, true));
                                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderFacilityCustom", parameters);
                                    }
                                }
                            }
                        }

                        cntr++;
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderServiceLocation";
                LogMessage = "ProviderServiceLocation - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderLanguage(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderLanguage != null)
                {
                    foreach (PartialProviderLanguageList lst in Payload.ProviderInformation.ProviderLanguage)
                    {

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("LanguageCode", DbType.String, lst.Language, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("Provlngsrccode", DbType.String, lst.Provlngsrccode, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderLanguage", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderLanguauge";
                LogMessage = "ProviderLanguauge - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_ProviderServices(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderServices != null)
                {
                    foreach (PartialProviderServicesList lst in Payload.ProviderInformation.ProviderServices)
                    {

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("ProviderserviceID", DbType.String, lst.ProviderServiceID, true));
                        parameters.Add(SqlParms.CreateParameter("ProviderService", DbType.String, lst.ProviderService, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("Provsvcsrckey", DbType.String, lst.Provsvcsrckey, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_ProviderServices", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderServices";
                LogMessage = "ProviderServices - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_OwnerRelationship(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.OwnerRelationship != null)
                {
                    foreach (PartialOwnerRelationshipList lst in Payload.ProviderInformation.OwnerRelationship)
                    {
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("Owner1FieldName", DbType.String, lst.ItemElementName, true));
                        parameters.Add(SqlParms.CreateParameter("Owner1SSN", DbType.String, lst.Item, true));
                        parameters.Add(SqlParms.CreateParameter("Owner2FieldName", DbType.String, lst.Item1ElementName, true));
                        parameters.Add(SqlParms.CreateParameter("Owner2SSN", DbType.String, lst.Item1, true));
                        parameters.Add(SqlParms.CreateParameter("RelationType", DbType.String, lst.RelationType, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrRltnshpSrcKey", DbType.String, lst.OwnrRltnshpSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        DataAccess.ExecuteStoredProcedure("insertSTG_OwnerRelationship_Custom", parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - OwnerRelationship";
                LogMessage = "OwnerRelationship - " + ex.Message;
                throw ex;
            }
        }
        private void InsertSTG_OwnerShipDetails(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderOwnerships != null)
                {
                    foreach (PartialProviderOwnershipList lst in Payload.ProviderInformation.ProviderOwnerships)
                    {

                        PartialOwnerOwnerIndividual lstInd = new PartialOwnerOwnerIndividual();  //= (PartialOwnerIndividual)lst.OwnershipDetails.Item;
                        PartialOwnerOwnerOrganization lstOrg = new PartialOwnerOwnerOrganization();   // (PartialOwnerOrganization)lst.OwnershipDetails.Item;

                        List<SqlParameter> parameters = new List<SqlParameter>();

                        if (lst.OwnershipDetails.OwnerType == "RI" || lst.OwnershipDetails.OwnerType == "I" || lst.OwnershipDetails.OwnerType == "SI" || lst.OwnershipDetails.OwnerType == "SUI") // individual
                        {
                            lstInd = (PartialOwnerOwnerIndividual)lst.OwnershipDetails.Item;
                            lstOrg.OrgTaxIdNumber = "";
                            lstOrg.OwnerOrgName = "";
                            lstOrg.Title = "";

                            // title for individual owner 
                            parameters.Add(SqlParms.CreateParameter("Title", DbType.String, lstInd.Title, true));
                        }
                        else if (lst.OwnershipDetails.OwnerType == "O" || lst.OwnershipDetails.OwnerType == "RO" || lst.OwnershipDetails.OwnerType == "SO" || lst.OwnershipDetails.OwnerType == "SUO" || lst.OwnershipDetails.OwnerType == "OP")
                        {
                            lstOrg = (PartialOwnerOwnerOrganization)lst.OwnershipDetails.Item;
                            lstInd.FirstName = "";
                            lstInd.MiddleName = "";
                            lstInd.LastName = "";
                            lstInd.DateOfBirth = "";
                            lstInd.SSN = "";
                            lstInd.Title = "";

                            // title for organizational owner
                            parameters.Add(SqlParms.CreateParameter("Title", DbType.String, lstOrg.Title, true));
                        }

                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerType", DbType.String, lst.OwnershipDetails.OwnerType, true));
                        parameters.Add(SqlParms.CreateParameter("OwnPercentage", DbType.String, lst.OwnershipDetails.OwnPercentage, true));
                        parameters.Add(SqlParms.CreateParameter("SanctionInd", DbType.String, lst.OwnershipDetails.SanctionInd, true));
                        parameters.Add(SqlParms.CreateParameter("SanctionExplain", DbType.String, lst.OwnershipDetails.SanctionExplain, true));
                        parameters.Add(SqlParms.CreateParameter("CriminalOffenseInd", DbType.String, lst.OwnershipDetails.CriminalOffenseInd, true));
                        parameters.Add(SqlParms.CreateParameter("CriminalOffenseExplain", DbType.String, lst.OwnershipDetails.CriminalOffenseExplain, true));
                        parameters.Add(SqlParms.CreateParameter("LicenseSuspendInd", DbType.String, lst.OwnershipDetails.LicenseSuspendInd, true));
                        parameters.Add(SqlParms.CreateParameter("LicenseSuspendExplain", DbType.String, lst.OwnershipDetails.LicenseSuspendExplain, true));
                        parameters.Add(SqlParms.CreateParameter("PenaltyInd", DbType.String, lst.OwnershipDetails.PenaltyInd, true));
                        parameters.Add(SqlParms.CreateParameter("PenaltyExplain", DbType.String, lst.OwnershipDetails.PenaltyExplain, true));
                        parameters.Add(SqlParms.CreateParameter("CntrlIntrstOthrProvEntityInd", DbType.String, lst.OwnershipDetails.CntrlIntrstOthrProvEntityInd, true));
                        parameters.Add(SqlParms.CreateParameter("CntrlIntrstOthrProvEntityExplain", DbType.String, lst.OwnershipDetails.CntrlIntrstOthrProvEntityExplain, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrTrnxInd", DbType.String, lst.OwnershipDetails.OwnrSubcntrctrTrnxInd, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrDetails", DbType.String, lst.OwnershipDetails.OwnrSubcntrctrDetails, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrBsnssTrnxInd", DbType.String, lst.OwnershipDetails.OwnrSubcntrctrBsnssTrnxInd, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrBsnssTrnxDetails", DbType.String, lst.OwnershipDetails.OwnrSubcntrctrBsnssTrnxDetails, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerOutOfStateResidentIndicator", DbType.String, lst.OwnershipDetails.OwnerOutOfStateResidentIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerOutOfStateResidentExplain", DbType.String, lst.OwnershipDetails.OwnerOutOfStateResidentExplain, true));
                        parameters.Add(SqlParms.CreateParameter("ProvOwnrDtlSrcKey", DbType.String, lst.OwnershipDetails.ProvOwnrDtlSrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("FingerprintStatusIndicator", DbType.String, lst.OwnershipDetails.FingerprintStatusIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.OwnershipDetails.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.OwnershipDetails.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.OwnershipDetails.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerAffiliationType", DbType.String, lst.OwnershipDetails.OwnerAffiliationType, true));
                        parameters.Add(SqlParms.CreateParameter("RelationshipInd", DbType.String, lst.OwnershipDetails.RelationshipInd, true));
                        parameters.Add(SqlParms.CreateParameter("TransactionAmount", DbType.Decimal, lst.OwnershipDetails.TransactionAmount, true));
                        parameters.Add(SqlParms.CreateParameter("TransactionDate", DbType.String, lst.OwnershipDetails.TransactionDate, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerStateFederalOffenseInd", DbType.String, lst.OwnershipDetails.OwnerStateFederalOffenseInd, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerStateFederalOffenseExplain", DbType.String, lst.OwnershipDetails.OwnerStateFederalOffenseExplain, true));
                        parameters.Add(SqlParms.CreateParameter("reg_owner_id", DbType.String, 0, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("CreatedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedDate", DbType.DateTime, DateTime.Now, true));
                        parameters.Add(SqlParms.CreateParameter("LastModifiedBy", DbType.Guid, appPDMSDataExchangeUserId, true));
                        //Individual Owner
                        parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, lstInd.FirstName, true));
                        parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, lstInd.MiddleName, true));
                        parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lstInd.LastName, true));
                        parameters.Add(SqlParms.CreateParameter("DateOfBirth", DbType.String, lstInd.DateOfBirth, true));
                        parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, lstInd.SSN, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerIndvSrcKey", DbType.String, lstInd.OwnerIndvSrcKey, true));

                        //Organisational Owner
                        parameters.Add(SqlParms.CreateParameter("OwnerOrgName", DbType.String, lstOrg.OwnerOrgName, true));
                        parameters.Add(SqlParms.CreateParameter("OrgTaxIdNumber", DbType.String, lstOrg.OrgTaxIdNumber, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerOrgSrcKey", DbType.String, lstOrg.OwnerOrgSrcKey, true));

                        foreach (PartialProviderOwnershipListAddress lstOwnerAddr in lst.OwnerAddress)
                        {

                            parameters.Add(SqlParms.CreateParameter("AddrRecordStatusCode", DbType.String, lstOwnerAddr.RecordStatusCode, true));
                            parameters.Add(SqlParms.CreateParameter("AddressType", DbType.String, "Primary", true));
                            parameters.Add(SqlParms.CreateParameter("AddressLine1", DbType.String, lstOwnerAddr.AddressLine1, true));
                            parameters.Add(SqlParms.CreateParameter("AddressLine2", DbType.String, lstOwnerAddr.AddressLine2, true));
                            parameters.Add(SqlParms.CreateParameter("City", DbType.String, lstOwnerAddr.City, true));
                            parameters.Add(SqlParms.CreateParameter("State", DbType.String, lstOwnerAddr.State, true));
                            parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lstOwnerAddr.CountyCode, true));
                            parameters.Add(SqlParms.CreateParameter("ZipCode5", DbType.String, lstOwnerAddr.ZipCode5, true));
                            parameters.Add(SqlParms.CreateParameter("ZipCode4", DbType.String, lstOwnerAddr.ZipCode4, true));
                            parameters.Add(SqlParms.CreateParameter("Country", DbType.String, lstOwnerAddr.Country, true));
                            parameters.Add(SqlParms.CreateParameter("BorderStateIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("AddressNameTypeIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressFirstName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressMiddleName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressLastName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressContactName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvEmail", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressLongitude", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressLatitude", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("HandicapAccessibilityIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("AddrEffectiveDate", DbType.DateTime, lstOwnerAddr.EffectiveDate, true));
                            parameters.Add(SqlParms.CreateParameter("AddrEndDate", DbType.DateTime, lstOwnerAddr.EndDate, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressSrcKey", DbType.String, lstOwnerAddr.ProvOwnrAdrsSrcKey, true));
                            parameters.Add(SqlParms.CreateParameter("reg_address_id", DbType.Int32, 0, true));
                            DataAccess.ExecuteStoredProcedure("insertSTG_ProviderOwnership_Custom", parameters);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderOwnerships";
                LogMessage = "ProviderOwnerships - " + ex.Message;
                throw ex;
            }
        }

        private void InsertSTG_ManagedEmployee(int ProviderInformationID, SubmitPartialProviderRequestPayload Payload)
        {
            try
            {
                if (Payload.ProviderInformation.ProviderManagedEmployees != null)
                {
                    foreach (PartialProviderManagedEmployeesListProviderManagedEmployee lst in Payload.ProviderInformation.ProviderManagedEmployees)
                    {
                        string ManageEmployeesInd = string.IsNullOrEmpty(lst.ManagedEmployee.ManageEmployeesInd) ? "Y" : lst.ManagedEmployee.ManageEmployeesInd;

                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                        parameters.Add(SqlParms.CreateParameter("FirstName", DbType.String, lst.ManagedEmployee.FirstName, true));
                        parameters.Add(SqlParms.CreateParameter("MiddleName", DbType.String, lst.ManagedEmployee.MiddleName, true));
                        parameters.Add(SqlParms.CreateParameter("LastName", DbType.String, lst.ManagedEmployee.LastName, true));
                        parameters.Add(SqlParms.CreateParameter("ManageEmployeesType", DbType.String, lst.ManagedEmployee.ManageEmployeesType, true));
                        parameters.Add(SqlParms.CreateParameter("ManageEmployeesInd", DbType.String, ManageEmployeesInd, true));
                        parameters.Add(SqlParms.CreateParameter("DateOfBirth", DbType.String, lst.ManagedEmployee.DateOfBirth, true));
                        parameters.Add(SqlParms.CreateParameter("SSN", DbType.String, lst.ManagedEmployee.SSN, true));
                        parameters.Add(SqlParms.CreateParameter("SanctionInd", DbType.String, lst.ManagedEmployee.SanctionInd, true));
                        parameters.Add(SqlParms.CreateParameter("SanctionExplain", DbType.String, lst.ManagedEmployee.SanctionExplain, true));
                        parameters.Add(SqlParms.CreateParameter("CriminalOffenseInd", DbType.String, lst.ManagedEmployee.CriminalOffenseInd, true));
                        parameters.Add(SqlParms.CreateParameter("CriminalOffenseExplain", DbType.String, lst.ManagedEmployee.CriminalOffenseExplain, true));
                        parameters.Add(SqlParms.CreateParameter("LicenseSuspendInd", DbType.String, lst.ManagedEmployee.LicenseSuspendInd, true));
                        parameters.Add(SqlParms.CreateParameter("LicenseSuspendExplain", DbType.String, lst.ManagedEmployee.LicenseSuspendExplain, true));
                        parameters.Add(SqlParms.CreateParameter("PenaltyInd", DbType.String, lst.ManagedEmployee.PenaltyInd, true));
                        parameters.Add(SqlParms.CreateParameter("PenaltyExplain", DbType.String, lst.ManagedEmployee.PenaltyExplain, true));
                        parameters.Add(SqlParms.CreateParameter("CntrlIntrstOthrProvEntityInd", DbType.String, lst.ManagedEmployee.CntrlIntrstOthrProvEntityInd, true));
                        parameters.Add(SqlParms.CreateParameter("CntrlIntrstOthrProvEntityExplain", DbType.String, lst.ManagedEmployee.CntrlIntrstOthrProvEntityExplain, true));
                        //parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrTrnxInd", DbType.String, lst.ManagedEmployee.OwnrSubcntrctrTrnxInd, true));
                        //parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrDetails", DbType.String, lst.ManagedEmployee.OwnrSubcntrctrDetails, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrBsnssTrnxInd", DbType.String, lst.ManagedEmployee.OwnrSubcntrctrBsnssTrnxInd, true));
                        parameters.Add(SqlParms.CreateParameter("OwnrSubcntrctrBsnssTrnxDetails", DbType.String, lst.ManagedEmployee.OwnrSubcntrctrBsnssTrnxDetails, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerOutOfStateResidentIndicator", DbType.String, lst.ManagedEmployee.OwnerOutOfStateResidentIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerOutOfStateResidentExplain", DbType.String, lst.ManagedEmployee.OwnerOutOfStateResidentExplain, true));
                        parameters.Add(SqlParms.CreateParameter("ProvMngdEmplySrcKey", DbType.String, lst.ManagedEmployee.ProvMngdEmplySrcKey, true));
                        parameters.Add(SqlParms.CreateParameter("FingerprintStatusIndicator", DbType.String, lst.ManagedEmployee.FingerprintStatusIndicator, true));
                        parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.ManagedEmployee.RecordStatusCode, true));
                        parameters.Add(SqlParms.CreateParameter("EffectiveDate", DbType.DateTime, lst.ManagedEmployee.EffectiveDate, true));
                        parameters.Add(SqlParms.CreateParameter("EndDate", DbType.DateTime, lst.ManagedEmployee.EndDate, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerAffiliationType", DbType.String, lst.ManagedEmployee.OwnerAffiliationType, true));
                        parameters.Add(SqlParms.CreateParameter("RelationshipInd", DbType.String, lst.ManagedEmployee.RelationshipInd, true));
                        parameters.Add(SqlParms.CreateParameter("TransactionAmount", DbType.Decimal, lst.ManagedEmployee.TransactionAmount, true));
                        parameters.Add(SqlParms.CreateParameter("TransactionDate", DbType.String, lst.ManagedEmployee.TransactionDate, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerStateFederalOffenseInd", DbType.String, lst.ManagedEmployee.OwnerStateFederalOffenseInd, true));
                        parameters.Add(SqlParms.CreateParameter("OwnerStateFederalOffenseExplain", DbType.String, lst.ManagedEmployee.OwnerStateFederalOffenseExplain, true));

                        foreach (PartialProviderManagedEmployeesListProviderManagedEmployeeAddress lstMngAddr in lst.ManagedEmployeeAddress)
                        {
                            parameters.Add(SqlParms.CreateParameter("AddressRecordStatusCode", DbType.String, lstMngAddr.RecordStatusCode, true));
                            parameters.Add(SqlParms.CreateParameter("AddressType", DbType.String, "Primary", true));
                            parameters.Add(SqlParms.CreateParameter("AddressLine1", DbType.String, lstMngAddr.AddressLine1, true));
                            parameters.Add(SqlParms.CreateParameter("AddressLine2", DbType.String, lstMngAddr.AddressLine2, true));
                            parameters.Add(SqlParms.CreateParameter("City", DbType.String, lstMngAddr.City, true));
                            parameters.Add(SqlParms.CreateParameter("State", DbType.String, lstMngAddr.State, true));
                            parameters.Add(SqlParms.CreateParameter("CountyCode", DbType.String, lstMngAddr.CountyCode, true));
                            parameters.Add(SqlParms.CreateParameter("ZipCode5", DbType.String, lstMngAddr.ZipCode5, true));
                            parameters.Add(SqlParms.CreateParameter("ZipCode4", DbType.String, lstMngAddr.ZipCode4, true));
                            parameters.Add(SqlParms.CreateParameter("Country", DbType.String, lstMngAddr.Country, true));
                            parameters.Add(SqlParms.CreateParameter("BorderStateIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("AddressNameTypeIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressFirstName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressMiddleName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgAddressLastName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumber2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgPhoneNumberExt2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber1", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvOrgFaxNumber2", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressContactName", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvEmail", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressLongitude", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressLatitude", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("HandicapAccessibilityIndicator", DbType.String, DBNull.Value, true));
                            parameters.Add(SqlParms.CreateParameter("AddrEffectiveDate", DbType.DateTime, lstMngAddr.EffectiveDate, true));
                            parameters.Add(SqlParms.CreateParameter("AddrEndDate", DbType.DateTime, lstMngAddr.EndDate, true));
                            parameters.Add(SqlParms.CreateParameter("ProvAddressSrcKey", DbType.String, lstMngAddr.ProvOwnrAdrsSrcKey, true));
                            parameters.Add(SqlParms.CreateParameter("reg_address_id", DbType.Int32, 0, true));
                            DataAccess.ExecuteStoredProcedure("insertSTG_ProviderManagedEmployee_Custom", parameters);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = "The request has errors in the node - ProviderManagedEmployee";
                LogMessage = "ProviderManagedEmployee - " + ex.Message;
                throw ex;
            }
        }

        private int checkAllContentBase64BinaryPopulated(PartialProviderDocumentsList[] docNode)
        {
            //1 - All Nodes populated
            //2 - Content Base 64 empty
            //0 - docName - docType - docExt empty
            int fileDataExists = 1;
            foreach (PartialProviderDocumentsList lst in docNode)
            {

                if (string.IsNullOrEmpty(lst.DocType) || string.IsNullOrEmpty(lst.DocName) || string.IsNullOrEmpty(lst.DocExt))
                {
                    return 0;
                }

                if (lst.ContentBase64binary == null || string.IsNullOrEmpty(lst.ContentBase64binary) || string.IsNullOrWhiteSpace(lst.ContentBase64binary))
                {
                    return 2;
                }
            }
            return fileDataExists;
        }

        private bool checkAllDocumentsExists(PartialProviderDocumentsList[] docNode, int regID, int pnmAppID, int doddAppID, int moduleTransactionID)
        {
            bool fileDataExists = true;
            foreach (PartialProviderDocumentsList lst in docNode)
            {
                if (string.IsNullOrEmpty(lst.ContentBase64binary))
                {
                    fileDataExists = checkFileExists(lst.DocName, regID, pnmAppID, doddAppID, moduleTransactionID);
                    if (!fileDataExists)
                    {
                        return false;
                    }
                }
            }
            return fileDataExists;
        }

        private int GetAttachmentPartialProviderDocument(string fileName, int regID, int pnmAppID, int doddAppID, int moduleTransactionID)
        {
            int documentID = 0;
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("FileName", DbType.String, fileName, true));
            parameters.Add(SqlParms.CreateParameter("ModuleTransactionID", DbType.Int32, moduleTransactionID, true));
            parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, true));
            parameters.Add(SqlParms.CreateParameter("PNMAppID", DbType.Int32, pnmAppID, true));
            parameters.Add(SqlParms.CreateParameter("DODDAppID", DbType.Int32, doddAppID, true));
            DataSet dsType = DataAccess.ExecuteStoredProcedure("usp_GetAttachmentPartialProviderDocument", parameters, "GetAttachmentPartialProviderDocument");
            if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
            {
                documentID = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_ID"].ToString());
            }
            return documentID;
        }

        private bool checkFileExists(string fileName, int regID, int pnmAppID, int doddAppID, int moduleTransactionID)
        {
            bool fileExists = false;
            int documentID = GetAttachmentPartialProviderDocument(fileName, regID, pnmAppID, doddAppID, moduleTransactionID);
            if (documentID > 0)
            {
                fileExists = true;
            }
            return fileExists;
        }

        private string InsertSTG_ProviderDocuments(int ProviderInformationID, int RegID, SubmitPartialProviderRequestPayload Payload, int regID,
            int pnmAppID, int doddAppID, int moduleTransactionID)
        {
            string rtn = "Success";
            try
            {
                if (Payload.ProviderInformation.ProviderDocuments != null)
                {
                    int documentID = 0;
                    //check all files within the document node
                    if (checkAllContentBase64BinaryPopulated(Payload.ProviderInformation.ProviderDocuments) == 1)
                    {
                        foreach (PartialProviderDocumentsList lst in Payload.ProviderInformation.ProviderDocuments)
                        {
                            int documentTypeId = GetDocumentType(lst.DocType);
                            if (documentTypeId > 0)
                            {
                                string DestinationPath = AppSettings.Get("OnBaseDocUpload-ImportLocalPath", string.Empty);
                                if (System.Diagnostics.Debugger.IsAttached)
                                    DestinationPath = @"D:\Files\OnbaseTest\";
                                string fileName = System.IO.Path.GetFileNameWithoutExtension(lst.DocName) + "." + lst.DocExt;
                                string newFileName = RenameFileMethod(DestinationPath, fileName);

                                documentID = StoreDocumentRecord(fileName, newFileName, lst.DocDescription);

                                if (documentID > 0)
                                {
                                    //byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(lst.ContentBase64binary);
                                    byte[] fileBytes = Convert.FromBase64CharArray(lst.ContentBase64binary.ToCharArray(), 0, lst.ContentBase64binary.Length);
                                    try
                                    {
                                        if (AppSettings.Get("SendUpdatetoOnbasefromPartial", string.Empty) == "true")
                                            SendUpdateToOnbase(newFileName, documentID, fileBytes);
                                        SaveFileToFileStore(newFileName, documentID, fileBytes);
                                        rtn = "Success";
                                    }
                                    catch (Exception ex)
                                    {
                                        rtn = "ProviderDocuments - Error saving the documents";
                                        LogMessage = "ProviderDocuments - " + rtn + "-" + ex.Message;
                                    }

                                }
                                else
                                {
                                    rtn = "The request has errors in the node - ProviderDocuments - Unable to save document.";
                                    // return rtn;
                                }
                            }
                            else
                            {
                                rtn = "The request has errors in the node - ProviderDocuments - Invalid Document Type.";
                                // return rtn;
                            }
                            // insert into STG_ProviderDocuments -- Add DocumentID column to help save record to REG_DOCUMENT_XREF.
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                            parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                            parameters.Add(SqlParms.CreateParameter("DocName", DbType.String, lst.DocName, true));
                            parameters.Add(SqlParms.CreateParameter("DocType", DbType.String, lst.DocType, true));
                            parameters.Add(SqlParms.CreateParameter("DocExt", DbType.String, lst.DocExt, true));
                            parameters.Add(SqlParms.CreateParameter("DocDescription", DbType.String, lst.DocDescription, true));
                            parameters.Add(SqlParms.CreateParameter("Provdocsrckey", DbType.String, lst.Provdocsrckey, true));
                            parameters.Add(SqlParms.CreateParameter("DocumentID", DbType.Int32, documentID, true));
                            DataAccess.ExecuteStoredProcedure("insertSTG_ProviderDocuments", parameters);
                        }
                    }
                    else if (checkAllContentBase64BinaryPopulated(Payload.ProviderInformation.ProviderDocuments) == 2)
                    {
                        //do something different
                        //verify all files exists
                        if (checkAllDocumentsExists(Payload.ProviderInformation.ProviderDocuments, regID, pnmAppID, doddAppID, moduleTransactionID))
                        {
                            foreach (PartialProviderDocumentsList lst in Payload.ProviderInformation.ProviderDocuments)
                            {
                                int documentTypeId = GetDocumentType(lst.DocType);
                                if (documentTypeId > 0)
                                {

                                    if (lst.ContentBase64binary == null || string.IsNullOrEmpty(lst.ContentBase64binary))
                                    {
                                        documentID = GetAttachmentPartialProviderDocument(lst.DocName, regID, pnmAppID, doddAppID, moduleTransactionID);
                                    }
                                    else
                                    {
                                        string DestinationPath = AppSettings.Get("OnBaseDocUpload-ImportLocalPath", string.Empty);
                                        if (System.Diagnostics.Debugger.IsAttached)
                                            DestinationPath = @"D:\Files\OnbaseTest\";
                                        string fileName = System.IO.Path.GetFileNameWithoutExtension(lst.DocName) + "." + lst.DocExt;
                                        string newFileName = RenameFileMethod(DestinationPath, fileName);

                                        documentID = StoreDocumentRecord(fileName, newFileName, lst.DocDescription);

                                        if (documentID > 0)
                                        {
                                            //byte[] fileBytes = System.Text.Encoding.ASCII.GetBytes(lst.ContentBase64binary);
                                            byte[] fileBytes = Convert.FromBase64CharArray(lst.ContentBase64binary.ToCharArray(), 0, lst.ContentBase64binary.Length);
                                            try
                                            {
                                                if (AppSettings.Get("SendUpdatetoOnbasefromPartial", string.Empty) == "true")
                                                    SendUpdateToOnbase(newFileName, documentID, fileBytes);
                                                SaveFileToFileStore(newFileName, documentID, fileBytes);
                                                rtn = "Success";
                                            }
                                            catch (Exception ex)
                                            {
                                                rtn = "ProviderDocuments - Error saving the documents";
                                                LogMessage = "ProviderDocuments - " + rtn + "-" + ex.Message;
                                            }

                                        }
                                        else
                                        {
                                            rtn = "The request has errors in the node - ProviderDocuments - Unable to save document.";
                                            // return rtn;
                                        }
                                    }
                                }
                                else
                                {
                                    rtn = "The request has errors in the node - ProviderDocuments - Invalid Document Type.";
                                    // return rtn;
                                }
                                // insert into STG_ProviderDocuments -- Add DocumentID column to help save record to REG_DOCUMENT_XREF.
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("ProviderInformationID", DbType.Int32, ProviderInformationID, true));
                                parameters.Add(SqlParms.CreateParameter("RecordStatusCode", DbType.String, lst.RecordStatusCode, true));
                                parameters.Add(SqlParms.CreateParameter("DocName", DbType.String, lst.DocName, true));
                                parameters.Add(SqlParms.CreateParameter("DocType", DbType.String, lst.DocType, true));
                                parameters.Add(SqlParms.CreateParameter("DocExt", DbType.String, lst.DocExt, true));
                                parameters.Add(SqlParms.CreateParameter("DocDescription", DbType.String, lst.DocDescription, true));
                                parameters.Add(SqlParms.CreateParameter("Provdocsrckey", DbType.String, lst.Provdocsrckey, true));
                                parameters.Add(SqlParms.CreateParameter("DocumentID", DbType.Int32, documentID, true));
                                DataAccess.ExecuteStoredProcedure("insertSTG_ProviderDocuments", parameters);
                            }
                        }
                        else
                        {
                            rtn = "The request has errors in the node - Provider Documents - Invalid Files";
                        }
                    }
                    else
                    {
                        rtn = "The request has errors in the node - Provider Documents. DocType/DocName/DocExt is missing";
                    }
                }
                else
                {
                    rtn = "Success";
                    // return rtn;
                }
            }
            catch (Exception ex)
            {
                ExceptionMessage = rtn != "Success" ? rtn : "The request has errors in the node - ProviderDocuments";
                LogMessage = "ProviderDocuments - " + ExceptionMessage + "-" + ex.Message;
                throw ex;
            }
            return rtn;
        }

        private int GetDocumentType(string documentType)
        {

            int retVal = 0;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_TYPE_ID"].ToString());
            }
            catch (Exception ex)
            {
                throw ex;


            }

            return retVal;
        }
        private string RenameFileMethod(string dir, string input)
        {
            string rtn = input;
            int idx = 0;
            while (System.IO.File.Exists(dir + rtn))
            {
                idx += 1;
                int pos = input.LastIndexOf(".");
                if (pos == -1) rtn = input + "_" + idx.ToString();
                else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
            }
            return rtn;
        }
        private int StoreDocumentRecord(string name, string filename, string desc)
        {

            int retVal = 0;
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, appPDMSDataExchangeUserId, false));

                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT", parameters));


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return retVal;
        }
        //private int StoreDocumentRecord(string name, string filename, string desc, int regID)
        //{

        //    int docID = -1;
        //    try
        //    {
        //        List<SqlParameter> parameters = new List<SqlParameter>();
        //        parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
        //        parameters.Add(SqlParms.CreateParameter("REG_PAGE_TYPE_ID", DbType.Int32, Constants.RegistrationPageType.Agreements, true));
        //        parameters.Add(SqlParms.CreateParameter("REG_PAGE_SECTION", DbType.String, "OtherDocuments", true));
        //        parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, true));
        //        parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, true));
        //        parameters.Add(SqlParms.CreateParameter("WF_TASKNAME", DbType.String, string.Empty, true));
        //        parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, true));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        //        parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appAdminUserId, true));
        //        parameters.Add(SqlParms.CreateParameter("SCREENING_ACTIVITY_ID", DbType.Int32, 0, true));

        //        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_InsertREG_DOCUMENT", parameters, "Document");
        //        if (ds.Tables[0].Rows.Count > 0)
        //        {
        //            DataRow row = ds.Tables[0].Rows[0];
        //            docID = Convert.ToInt32(row[0].ToString());
        //        }
        //        return docID;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private void SendUpdateToOnbase(string fileName, int documentId, byte[] attachmentBytes)
        {
            try
            {
                OnBaseInterface onBaseInterface = new OnBaseInterface();
                onBaseInterface.SubmitFile(documentId, attachmentBytes, fileName);
            }
            catch (Exception ex)
            {
                //ExceptionMessage = string.Format("Exception while sending SendAttachment file to OnBase:{0},{1},{2}", fileName, documentId, ex.Message);
                string msg = string.Format("Exception while sending SendAttachment file to OnBase:{0},{1},{2}", fileName, documentId, ex.Message + "::" + ex.StackTrace);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;
            }
        }
        private void SaveFileToFileStore(string fileName, int documentID, byte[] attachmentData64Binary)
        {
            try
            {
                string DestinationPath = AppSettings.Get("OnBaseDocUpload-ImportLocalPath", string.Empty);
                if (System.Diagnostics.Debugger.IsAttached)
                    DestinationPath = @"D:\Files\OnbaseTest\";
                fileName = documentID + "_" + fileName;
                string totalFileName = Path.Combine(DestinationPath + fileName);
                File.WriteAllBytes(totalFileName, attachmentData64Binary);
            }
            catch (Exception ex)
            {
                //ExceptionMessage = string.Format("Exception while storing the file in SendAttachment file:{0},{1},{2}", fileName, documentID, ex.Message);
                string msg = string.Format("Exception while storing the file in SendAttachment file:{0},{1},{2}", fileName, documentID, ex.Message + "::" + ex.StackTrace);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;
            }
        }
        private void SendEmailComplianceSpecialist(int regID, string provName)
        {
            try
            {
                Dictionary<string, object> fields = new Dictionary<string, object>();
                fields.Add("ProvName", provName);
                fields.Add("RegID", regID);
                string body = string.Empty;
                string recipients = GetRecipients(19, regID);       //SendToTypeID = 19 -- Send email to Compliance Specialist

                string templateActualPath = string.Empty;
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    templateActualPath = @"C:\Users\CA_OHPNM_DEV_7\source\repos\GIT\ohpnm-src\PDMS\ProviderDataManagementSystemService\Documents\";
                    EMailNotification notify = new EMailNotification(body, "DODD Services Terminated", "ramyagopanapalli1@maximus.com");
                    body = notify.SendActualNotification(templateActualPath + "//DODDServicesEndEmailComplSpec.txt", fields, true);
                }
                else
                {
                    templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                    EMailNotification notify = new EMailNotification(body, "DODD Services Terminated", recipients);
                    body = notify.SendActualNotification(templateActualPath + "//DODDServicesEndEmailComplSpec.txt", fields, true);
                }
                //do not create communication event for this.
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("Error in Inbound Partial Provider in send email to CompSpl: " + ex.Message + ex.StackTrace.ToString(), Logging.LogPriority.Error);
            }
        }
        private string GetRecipients(int sendToTypeID, int regId)
        {
            StringBuilder addrList = new StringBuilder();
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                if (Methods.HasRows(ds))
                {
                    addrList = Methods.AddEmail(ds.Tables[0], "EmailAddress");
                }
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("Error in Inbound Partial Provider in sending email to CompSpl recipients: " + ex.Message + ex.StackTrace.ToString(), Logging.LogPriority.Error);
            }

            return addrList.ToString();
        }
        static List<string> ApplicationStatusIDs()
        {
            if (m_ApplicationStatusIDs.Count == 0)
            {
                m_ApplicationStatusIDs.Add("closed as denied");
                m_ApplicationStatusIDs.Add("closed as disapprove");
                m_ApplicationStatusIDs.Add("withdrawn by provider");
                m_ApplicationStatusIDs.Add("expired");
                m_ApplicationStatusIDs.Add("closed for reapplication");
                m_ApplicationStatusIDs.Add("recommended denial");
                m_ApplicationStatusIDs.Add("withdrawn/expired");
            }
            return m_ApplicationStatusIDs;
        }

        public static bool IsApplicationStatus(string statusName)
        {
            return ApplicationStatusIDs().Contains(statusName.ToLower());
        }

        static List<string> ApplicationStatusIDsOkToValidateData()
        {
            if (m_ApplicationStatusIDsOkToValidateData.Count == 0)
            {
                m_ApplicationStatusIDsOkToValidateData.Add("pending external medicaid approval");
                m_ApplicationStatusIDsOkToValidateData.Add("closed by odm");
                m_ApplicationStatusIDsOkToValidateData.Add("certified");
                m_ApplicationStatusIDsOkToValidateData.Add("closed as legal override");
                m_ApplicationStatusIDsOkToValidateData.Add("closed as adjudication order issued");
                m_ApplicationStatusIDsOkToValidateData.Add("approved");
                m_ApplicationStatusIDsOkToValidateData.Add("closed as complete");
                m_ApplicationStatusIDsOkToValidateData.Add("closed partial approval");
                m_ApplicationStatusIDsOkToValidateData.Add("recommended certification");
            }
            return m_ApplicationStatusIDsOkToValidateData;
        }

        public static bool IsApplicationStatusOkToValidateData(string statusName)
        {
            return ApplicationStatusIDsOkToValidateData().Contains(statusName.ToLower());
        }

        private bool IsClosureDateValid(string closureDate, int regid, DateTime? changeEffectivedate)
        {
            bool retVal = false;

            try
            {
                if (!string.IsNullOrEmpty(closureDate) && changeEffectivedate != null)
                {
                    DateTime dtProposedValid = DateTime.Now.AddYears(1);
                    DateTime entereddT = Convert.ToDateTime(closureDate);
                    bool isValid = (entereddT <= dtProposedValid);
                    if (isValid)
                    {
                        isValid = (entereddT > changeEffectivedate);
                    }

                    retVal = isValid;

                }
            }
            catch (Exception ex)
            {

                string msg = string.Format("Exception while checking the validatity of Closure date:{0}", closureDate);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;

            }

            return retVal;

        }
        private bool IsClosureInitiatedByOperator(int regid, string mmisProviderTypeID)
        {
            bool retVal = false;

            try
            {
                if (!string.IsNullOrEmpty(mmisProviderTypeID))
                {
                    if (mmisProviderTypeID == Constants.MMISProviderType.STATE_OPERATED_ICF_MR || mmisProviderTypeID == Constants.MMISProviderType.NON_STATE_OPERATED_ICF_MR)
                    {
                        retVal = true;
                    }

                }
            }
            catch (Exception ex)
            {

                string msg = string.Format("Exception while checking Closure Initiation by Operator for Registration :{0}", regid);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;

            }

            return retVal;

        }
        private bool IsClosureInitiatedByLicensee(int regid, string mmisProviderTypeID)
        {
            bool retVal = false;

            try
            {
                if ((!string.IsNullOrEmpty(mmisProviderTypeID))
                    &&
                    (mmisProviderTypeID == Constants.MMISProviderType.Licensee))
                {
                    retVal = true;
                }


            }
            catch (Exception ex)
            {

                string msg = string.Format("Exception while checking Closure Initiation by Licensee for Registration :{0}", regid);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;
            }
            return retVal;
        }

        private int GetFacilityRegistrationByFacilityNumber(string facilityNumber)
        {
            int retVal = 0;
            DataSet dsFacility = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FacilityNumber", DbType.String, facilityNumber, false));
                dsFacility = DataAccess.ExecuteStoredProcedure("usp_SelectOperatorReg_byFacility", parameters, "facilityRegistration");
                if (dsFacility != null && dsFacility.Tables[0] != null && dsFacility.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsFacility.Tables[0].Rows[0]["REG_ID"].ToString());
            }
            catch (Exception ex)
            {
                string msg = string.Format("Exception while getting registration by facility number:{0}", facilityNumber);
                Log.CreateLogEntry("Error in Inbound Partial Provider  - " + msg, Logging.LogPriority.Error);
                throw ex;
            }

            return retVal;
        }
        private DataSet GetRegidInfoByRegId(int regId)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
            return dsReg;
        }

        private Boolean isValidTitle(string ownerTitle)
        {
            // load a list of valid titles, but only load it once in case they need to check more than 1 title
            if (validOwnerTitles.Count == 0)
            {
                // no valid owner titles, need to load em
                DataSet dsValidTitles = DataAccess.ExecuteStoredProcedure("usp_SelectManaging_Employee_Title");

                if (Methods.HasRows(dsValidTitles))
                {
                    foreach (DataRow dr in dsValidTitles.Tables[0].Rows)
                    {
                        try
                        {
                            validOwnerTitles.Add(Methods.GetString("Owner_Title", dr));
                        }
                        catch (Exception ex)
                        {
                            ExceptionMessage = !string.IsNullOrEmpty(ExceptionMessage) ? ExceptionMessage : "Error while retrieving valid titles.";
                            LogMessage = "SP - " + ex.Message;
                            throw ex;
                        }
                    }
                }
            }

            foreach (string title in validOwnerTitles)
            {
                if (title == ownerTitle)
                {
                    return true;
                }
            }

            return false;
        }
        private bool IsValidSqlDatetime(string inputDate)
        {
            bool valid = false;
            DateTime inputDateTime = DateTime.MinValue;
            DateTime minSqlDateTime = new DateTime(1753, 1, 1);
            DateTime maxSqlDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            //minDateTime = new DateTime(1753, 1, 1);
            //maxDateTime = new DateTime(9999, 12, 31, 23, 59, 59, 997);

            if (DateTime.TryParse(inputDate, out inputDateTime))
            {
                if (inputDateTime >= minSqlDateTime && inputDateTime <= maxSqlDateTime)
                {
                    valid = true;
                }
            }

            return valid;
        }
    }
}
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWorkflow
{
    public class WFTerminateProvidersInPDE : BaseWorkflowTask, IElapsedTask
    {
        string nextStep = null;

        public WFTerminateProvidersInPDE(int processID, int stepID)
            : base(processID, stepID)
        {
        }

        private Logging log = null;
        override public bool ProcessElapsedTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool toReturn = false;

            int regID;
            Process p = new Process(ProcessID);

            try
            {
                if (!int.TryParse(p.GetProcessParameter(Constants.ProcessParameter.RegistrationID), out regID))
                {
                    throw new Exception(string.Format(
                        Constants.LogString.WorkflowParameterNotNumeric,
                        Assembly.GetExecutingAssembly().GetName().Name,
                        Constants.ProcessParameter.RegistrationID,
                        GetProcessParameter(Constants.ProcessParameter.RegistrationID)));
                }

                //cancel registration if it is not a converted provider
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                int RegProgramStatusTypeID = ObjectControllerHelper.GetInt("RegProgramStatusTypeID", drReg);
                string medicaidId = ObjectControllerHelper.GetString("MedicaidID", drReg);
                int workFlowEventId = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                int workflowId = ObjectControllerHelper.GetInt("WorkflowID", drReg);
                int workflowEventTypeID = ObjectControllerHelper.GetInt("WORKFLOW_EVENT_TYPE_ID", drReg);
                Guid userId = new Guid(Constants.appAdminUserId);
                //get recipients first before cancelling the registration
                string recipients = string.Empty;
                if(workflowId != Constants.WorkflowType.CPC)
                    recipients = GetRecipients(regID, Constants.SendToTypeID.Provider);

                List<SqlParameter> sqlParms1 = new List<SqlParameter>();
                sqlParms1.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                DataSet dsRTPCred = DataAccess.ExecuteStoredProcedure("usp_CheckRegistrationReturnedByCredentialing", sqlParms1, "RegCredData");
                bool isRTP_Frm_Credential = ObjectControllerHelper.HasRows(dsRTPCred) && ObjectControllerHelper.GetString("Return_Status", dsRTPCred.Tables[0].Rows[0]) == "true" ? true : false;

                List<SqlParameter> sqlParms3 = new List<SqlParameter>();
                sqlParms3.Add(SqlParms.CreateParameter("RegID", DbType.String, regID.ToString(), false));
                sqlParms3.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, ProcessID, false));
                DataSet dsRTP = DataAccess.ExecuteStoredProcedure("usp_CheckRegistrationIsRTP", sqlParms3, "RegRTP");
                bool isRTP = ObjectControllerHelper.HasRows(dsRTP) ? true : false;

                log.CreateLogEntry("In WFTerminateProvidersInPDE Reg ID = " + regID.ToString() + ",Process ID = " + ProcessID.ToString() + ",isRTP_Frm_Credential = " + isRTP_Frm_Credential.ToString() + ",isRTP = " + isRTP.ToString(), Logging.LogPriority.Error);
                
                string subject = string.Empty;
                if (isRTP_Frm_Credential)
                {

                    //OHPNM-1914 pschwarz 2/19/2021 only send email if new registration 
                    //Commenting Below Lines 1914 change is for Closure notice not for Credentialing RTP
                    //if (workFlowEventId == Constants.WorkflowEventType.NewReg)

                    DateTime dtRTPStartDate = ObjectControllerHelper.GetDateTime("START_DATE_TIME", dsRTPCred.Tables[0].Rows[0]);
                    int noofDays = Convert.ToInt32((DateTime.Now.Date - dtRTPStartDate.Date).TotalDays);

                    int SecondNoticeDays = Convert.ToInt32(AppSettings.Get("RTPfromCredentialingSecondNoticeDays"));
                    int FinalNoticedays = Convert.ToInt32(AppSettings.Get("RTPfromCredentialingFinalNoticeDays"));
                    int rtpDue = Convert.ToInt32(AppSettings.Get("RTPfromCredentialingDueWindow"));

                    List<SqlParameter> Parms = new List<SqlParameter>();
                    Parms.Add(new SqlParameter("REG_ID", regID.ToString()));
                    DataSet dsCred = DataAccess.ExecuteStoredProcedure("usp_SelectPROVIDER_Credentialing", Parms, "RegCred");

                    string credName = dsCred.Tables[0].Rows.Count > 1 ? "Recredentialing" : "Credentialing";

                    if (noofDays == SecondNoticeDays) //Send Second Reminder
                    {
                        subject = "Provider " + credName + " - Action Required - Second Request";
                        SendEmail("EMAIL_TEMPLATE_CREDENTIAL_RTP_SECOND_REMINDER", regID, subject, recipients);
                    }
                    else if (noofDays == FinalNoticedays) //Send Final Reminder
                    {
                        subject = "Provider " + credName + " - Action Required - Final Request";
                        SendEmail("EMAIL_TEMPLATE_CREDENTIAL_RTP_FINAL_REMINDER", regID, subject, recipients);
                    }
                    else if (noofDays >= rtpDue)
                    {
                        try
                        {
                            subject = "Application Termination Notice";
                            SendEmail("EMAIL_TEMPLATE_CREDENTIAL_FAIL_RTP", regID, subject, recipients);
                            log.CreateLogEntry("EMAIL_TEMPLATE_CREDENTIAL_FAIL_RTP", Logging.LogPriority.Information);
                        }
                        catch(Exception ex1)
                        {
                            log.CreateLogEntry("Error on EMAIL_TEMPLATE_CREDENTIAL_FAIL_RTP :" + ex1.ToString(),
                            Logging.LogPriority.Error);
                        }
                        try
                        {
                            string enrollmentStatusReasonCode = "44";//InActive

                            List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                            sqlParms2.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                            sqlParms2.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, DateTime.Now, false));
                            sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                            sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, false));
                            sqlParms2.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, true, false));
                            sqlParms2.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, Constants.EnrollStatus.INACTIVE.ToString(), false));
                            sqlParms2.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReasonCode, false));        // Enrollment Status reasons TBD
                            sqlParms2.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, true, false));
                            DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", sqlParms2);
                            log.CreateLogEntry("usp_TerminateProvider", Logging.LogPriority.Information);

                            //OHPNM-8701
                            try
                            {
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                parameters.Add(SqlParms.CreateParameter("STATUS_ID", DbType.Int32, CON.CredentilaingStatus.AdministrativeDenial, true));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, true));
                                DataAccess.ExecuteStoredProcedure("usp_UpdateCREDENTIALING_STATUS", parameters);
                            }
                            catch (Exception ex)
                            {
                                throw CoreException.ThrowException(ex);
                            }
                            try
                            {
                                List<SqlParameter> parameters = new List<SqlParameter>();
                                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, true));
                                parameters.Add(SqlParms.CreateParameter("RESULT_ID", DbType.Int32, CON.CredentialingResult.Denied, true));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userId, true));
                                DataAccess.ExecuteStoredProcedure("usp_UpdateCREDENTIALING_RESULT", parameters);
                            }
                            catch (Exception ex)
                            {
                                throw CoreException.ThrowException(ex);
                            }
                            try
                            {
                                string ConvertFrmORPWF = p.GetProcessParameter(Constants.ProcessParameter.IsConvertFrmORPWF);
                                bool IsConvertFrmORPWF = string.IsNullOrEmpty(ConvertFrmORPWF) ? false : Convert.ToBoolean(ConvertFrmORPWF);

                                List<SqlParameter> sql_Parms = new List<SqlParameter>();
                                sql_Parms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                                string orpFlag = DataAccess.ExecuteScalar("usp_GetFlag_ConvertORP", sql_Parms);

                                if (orpFlag == "True" || IsConvertFrmORPWF)
                                {
                                    List<SqlParameter> sqlParams = new List<SqlParameter>();
                                    sqlParams.Add(new SqlParameter("Reg_ID", regID));
                                    sqlParams.Add(new SqlParameter("Flag", false));
                                    DataAccess.ExecuteStoredProcedure("usp_UpdateFlag_ConvertORP", sqlParams, "UpdateORPApplicationFlag");

                                    SetProcessParameter(Constants.ProcessParameter.IsConvertFrmORPWF, "1");
                                }
                            }
                            catch (Exception ex)
                            {
                                throw CoreException.ThrowException(ex);
                            }


                        }
                        catch (Exception ex2)
                        {
                            log.CreateLogEntry("Error on usp_TerminateProvider :" + ex2.ToString(),
                                                        Logging.LogPriority.Error);
                        }
                        try
                        {
                            List<SqlParameter> parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, true));
                            DataAccess.ExecuteStoredProcedure("usp_WF_CancelWorkflowProcess", parameters);
                        }
                        catch (Exception ex)
                        {
                            throw CoreException.ThrowException(new Exception("Process Id: " + ProcessID.ToString() +
                                " - " + ex.Message + " - " + ex.StackTrace));
                        }
                    }
                }
                else if (isRTP)
                {
                    string rtpDueDays = AppSettings.Get("RTPDueWindow");
                    DataTable dtRTP = dsRTP.Tables[0];
                    if (!string.IsNullOrEmpty(rtpDueDays))
                    {
                        int daysRTP = Convert.ToInt32(rtpDueDays);

                        DateTime dtRTPStartDate = ObjectControllerHelper.GetDateTime("START_DATE_TIME", dtRTP.Rows[0]);

                        DateTime dtRTPCal = dtRTPStartDate.AddDays(daysRTP);
                        if (dtRTPCal < DateTime.Now)
                        {
                            if (string.IsNullOrEmpty(medicaidId) && (workFlowEventId == Constants.WorkflowEventType.NewReg || workFlowEventId == Constants.WorkflowEventType.ChangeProviderType))
                            {
                                ProviderController.TerminateProvider(regID, DateTime.Now, userId.ToString(), Constants.EnrollStatus.INACTIVE.ToString(), Constants.EnrollStatusReason.APPLICATIONPENDING, false);
                            }
                            RegistrationController.CancelRegistration(regID, DateTime.Now, userId, ProcessID);
                            if (workFlowEventId == Constants.WorkflowEventType.NewReg || workFlowEventId == Constants.WorkflowEventType.ChangeProviderType) 
                            {
                                subject = AppSettings.Get("StateName", "(State)") + " Medicaid Application Closed";
                                SendEmail("EMAIL_TEMPLATE_REGISTRATION_RTPDUE_NOTICE", regID, subject, recipients);
                            }
                        }
                        else
                        {
                            string rtpReminderDays = AppSettings.Get("RTPReminderWindow");
                            if (!string.IsNullOrEmpty(rtpReminderDays))
                            {
                                int daysRTPReminder = Convert.ToInt32(rtpReminderDays);

                                DateTime dtRTPReminderCal = dtRTPStartDate.AddDays(daysRTPReminder);

                                if (dtRTPReminderCal < DateTime.Now)
                                {
                                    // We passed 20 days, we need to send a reminder (only 1 time though)

                                    bool rtpReminderHasBeenSent = string.IsNullOrEmpty(p.GetProcessParameter(Constants.ProcessParameter.RtpReminderHasBeenSent)) ? false : Convert.ToBoolean(p.GetProcessParameter(Constants.ProcessParameter.RtpReminderHasBeenSent));

                                    if (!rtpReminderHasBeenSent)
                                    {
                                        // RTP reminder has not been sent, send it now and mark it complete in the WF parameters

                                        SetProcessParameter(Constants.ProcessParameter.RtpReminderHasBeenSent, "true");
                                        SaveProcessParameters();

                                        subject = AppSettings.Get("RTPReminderEmailSubject");

                                        if (workflowEventTypeID == Constants.WorkflowEventType.RevalReg)
                                        {
                                            // Use reval version of reminder
                                            SendEmail("EMAIL_TEMPLATE_RTP_REMINDER_REVAL", regID, subject, recipients);
                                        }
                                        else
                                        {
                                            // Use non-reval version of reminder
                                            SendEmail("EMAIL_TEMPLATE_RTP_REMINDER", regID, subject, recipients);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if(workflowId == Constants.WorkflowType.CPC)
                    {
                        recipients = GetRecipients(regID, Constants.SendToTypeID.CPCWorkflowCancelledNotice);
                        RegistrationController.CancelRegistrationCPC(regID, DateTime.Now, userId, ProcessID, workFlowEventId);
                        
                        subject = AppSettings.Get("StateName", "(State)") + " Medicaid Application Closed";
                        SendEmail("EMAIL_TEMPLATE_REGISTRATION_DUE_NOTICE", regID, subject, recipients);
                    }
                    else
                        RegistrationController.CancelRegistration(regID, DateTime.Now, userId, ProcessID);

                    if (workFlowEventId == Constants.WorkflowEventType.NewReg || workFlowEventId == Constants.WorkflowEventType.ChangeProviderType || 
                        (workflowId == Constants.WorkflowType.CMC && (workFlowEventId == Constants.WorkflowEventType.CMCEnroll || workFlowEventId == Constants.WorkflowEventType.CMCReAttest)))// CR313 - Send Cancellation notice on BH Provider Type Change
                    {
                        subject = AppSettings.Get("StateName", "(State)") + " Medicaid Application Closed";
                        SendEmail("EMAIL_TEMPLATE_REGISTRATION_DUE_NOTICE", regID, subject, recipients);
                    }
                }
                //}

                nextStep = "";
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                log.CreateLogEntry("Error in WFTerminateProvidersInPDE :" + ex.ToString(), Logging.LogPriority.Error);
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                {
                    log.CreateLogEntry("Error in WFTerminateProvidersInPDE(Inner Ex) :" + ex.InnerException.Message, Logging.LogPriority.Error);
                }
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        private void SendEmail(string Template_Name, int regID, string subject, string recipients)
        {
            int total = 0;
                
                if (string.IsNullOrEmpty(recipients))
                {                   
                     log.CreateLogEntry("Reg Id: " + regID + " - problems sending print mail in WFTerminateProvidersInPDE",
                              Logging.LogPriority.Error);
                }
                else
                {
                    EMailNotification notify = new EMailNotification(Template_Name, subject, recipients, LogThreadID);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                    {
                        total += 1;
                        if (log != null)
                        {
                            log.CreateLogEntry("Sent email Template_Name - " + Template_Name + "for Reg ID: " + regID + " - in WFTerminateProvidersInPDE",
                                Logging.LogPriority.Error);
                        }
                    }

                    else 
                        log.CreateLogEntry("Reg Id: " + regID + " - problems sending email in WFTerminateProvidersInPDE",
                        Logging.LogPriority.Error);
                }

            //return total;
        }
        private string GetRecipients(int regId, int SendToTypeID)
        {
            string rtn = string.Empty;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, SendToTypeID, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                rtn = AddEmail(rtn, ds, "EmailAddress");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Reg Id: " + regId.ToString() + ", GetRecipients - " + ex.Message, Logging.LogPriority.Error);
            }

            return rtn;
        }
        private string AddEmail(string addressList, DataSet ds, string colName)
        {
            string rtn = addressList;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string email = dr[colName].ToString();
                    if (!string.IsNullOrEmpty(email) && rtn.IndexOf(email) == -1)
                    {
                        if (!string.IsNullOrEmpty(rtn)) rtn += ",";
                        rtn += email;
                    }
                }
            }
            return rtn;
        }
        override public string NextStep()
        {
            return nextStep;
        }
    }
}

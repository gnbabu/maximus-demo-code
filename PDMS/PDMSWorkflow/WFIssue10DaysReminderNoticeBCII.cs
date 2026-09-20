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
    public class WFIssue10DaysReminderNoticeBCII : BaseWorkflowTask, IElapsedTask
    {
        string nextStep = null;

        public WFIssue10DaysReminderNoticeBCII(int processID, int stepID)
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

                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regID, false));
                DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectBCIIDataByRegID", sqlParms, "RegData");

                dsReg.Tables[0].TableName = "RegData";
                DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                DateTime BCInitialNoticeDate = DateTime.Now;
                bool IssueBCReminderNotice = false;
                if (drReg != null)
                {
                    BCInitialNoticeDate = ObjectControllerHelper.GetDateTime("DateOfInitialBCNotice", drReg);
                    IssueBCReminderNotice = ObjectControllerHelper.GetBool("IssueBCReminderNotice", drReg);
                }
                nextStep = "";
                if ((DateTime.Now - BCInitialNoticeDate).TotalDays > 20 && (IssueBCReminderNotice == false))
                {
                    Guid userId = new Guid(Constants.appAdminUserId);
                    string recipients = GetRecipients(regID, Constants.NoticeTypeID.Initial);

                    SendEmail("EMAIL_TEMPLATE_PROVIDER_BACKGROUND_10DAYS_NOTICE", regID, Constants.NoticeTypeID.Initial, recipients);
                    RegistrationController.UpdateReminderBCNoticeDate(regID, true, DateTime.Now, DateTime.Now, CON.appAdminUserId);
                }
                else if ((DateTime.Now - BCInitialNoticeDate).TotalDays > 60)
                {
                    foreach (DataRow row in dsReg.Tables[0].Rows)
                    {
                        int backgroundStatusTypeID = Methods.GetIntValue(row, "BACKGROUND_STATUS_TYPE_ID");
                        int backgroundID = Methods.GetIntValue(row, "REG_BACKGROUND_CHECK_ID");
                        if (backgroundStatusTypeID != Constants.BackgroundStatusType.Yes)
                        {
                            RegistrationController.UpdateBCIIResultByID(backgroundID, regID, CON.BackgroundResultType.FailedToAppear, DateTime.Now, new Guid(CON.appAdminUserId));
                        }
                    }
                    nextStep = "Deny";
                    toReturn = true;
                }

                
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        private void SendEmail(string Template_Name, int regID, int NoticeTypeID, string recipients)
        {
            int total = 0;
            string subject = "Request to Supply Fingerprints";

            DataSet dsEmails = RegistrationController.SelectRegistrationEmails(regID, subject);
            // If we do not have an entry for sending the email then we need to send it
            if (!ObjectControllerHelper.HasRows(dsEmails))
            {
                //string recipients = GetRecipients(regID, NoticeTypeID);
                if (string.IsNullOrEmpty(recipients))
                {
                    PaperNotification notify = new PaperNotification(subject, LogThreadID);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name)) total += 1;
                    else
                        log.CreateLogEntry("Reg Id: " + regID + " - problems sending BackgroundCheck Reminder Notice print mail",
                              Logging.LogPriority.Error);
                }
                else
                {
                    EMailNotification notify = new EMailNotification(Template_Name, subject, recipients, LogThreadID);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        total += 1;

                    else log.CreateLogEntry("Reg Id: " + regID + " - problems sending BackgroundCheck Reminder Notice email",
                        Logging.LogPriority.Error);
                }
            }

            //return total;
        }
        private string GetRecipients(int regId, int noticeTypeID)
        {
            string rtn = string.Empty;
            try
            {
                int sendToTypeID = Constants.SendToTypeID.StateAdmin;
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parameters, "EmailRecipients");
                rtn = AddEmail(rtn, ds, "CONTACT_EMAIL_ADDRESS");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Users", parameters, "EmailRecipients");
                rtn = AddEmail(rtn, ds, "Email");
                if (noticeTypeID == Constants.NoticeTypeID.Final)
                {
                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
                    parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                    rtn = AddEmail(rtn, ds, "EmailAddress");
                }
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

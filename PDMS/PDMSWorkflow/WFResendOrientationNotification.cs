using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Workflow;

namespace PDMSWorkflow
{
    public class WFResendOrientationNotification : BaseWorkflowTask, IElapsedTask
    {
        string nextStep = null;

        public WFResendOrientationNotification(int processID, int stepID)
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

                // send email notification
                SendEmail("EMAIL_TEMPLATE_ORIENTATION", regID, Constants.NoticeTypeID.Initial);

                nextStep = "";
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(LogThreadID, ex, logMsg);
            }
            return toReturn;
        }
        private void SendEmail(string Template_Name, int regID, int NoticeTypeID)
        {
            string subject = "Risk Second Provider Orientation Notice";

            DataSet dsEmails = RegistrationController.SelectRegistrationEmails(regID, subject);
            // If we do not have an entry for sending the email then we need to send it
            if (!ObjectControllerHelper.HasRows(dsEmails))
            {
                string recipients = GetRecipients(regID, NoticeTypeID);
                if (string.IsNullOrEmpty(recipients))
                {
                    PaperNotification notify = new PaperNotification(subject, LogThreadID);
                    if (!(notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name)))
                        log.CreateLogEntry("Reg Id: " + regID + " - problems re-sending Orientation print mail",
                              Logging.LogPriority.Error);
                }
                else
                {
                    EMailNotification notify = new EMailNotification(Template_Name, subject, recipients, LogThreadID);
                    if (!(notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name)))
                        log.CreateLogEntry("Reg Id: " + regID + " - problems re-sending Orientation email",
                            Logging.LogPriority.Error);
                }
            }
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

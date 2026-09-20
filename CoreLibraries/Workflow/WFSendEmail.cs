using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Workflow
{
    /// <summary>
    /// Summary description for WFSendEmail
    /// </summary>
    public class WFSendEmail : BaseWorkflowTask, Workflow.IWorkflowTask
    {
        public WFSendEmail(int processID, int stepID) : base(processID, stepID)
        {
            // TODO: Add constructor logic here
        }

        private static int SendToTypeProvider = 1;

        override public bool ProcessTask()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(LogThreadID, logMsg);

            bool result = false;
            int regID = 0;

            try
            {
                regID = Convert.ToInt32(GetProcessParameter("REGISTRATION_ID"));
                string body = GetTaskParameter("EMAIL_BODY_TEMPLATE");
                string subject = GetTaskParameter("EMAIL_SUBJECT");
                string sendToType = GetTaskParameter("EMAIL_RECIPIENT_TYPE_ID");
                int sendTo = string.IsNullOrWhiteSpace(sendToType) ? SendToTypeProvider : Convert.ToInt32(sendToType);
                string recipients = GetRecipients(sendTo, regID);
                
                log.CreateLogEntry("In ProcessTask of WFSendEmail body = " + body + " Task id = " + TaskID, Logging.LogPriority.Error);

                //if Paper Email Template then send both Paper mail and Email
                string paperTemplates = DataAccess.GetAppSetting("PaperEmailTemplates");
                if (paperTemplates.Contains(body))
                {
                    PaperNotification notify = new PaperNotification(subject, LogThreadID);
                    notify.SendWorkFlowEngineNotification(regID.ToString(), body);

                    if (!string.IsNullOrEmpty(recipients))
                    {
                        EMailNotification enotify = new EMailNotification(body, subject, recipients, LogThreadID);
                        enotify.SendWorkFlowEngineNotification(regID.ToString(), body);
                    }
                    result = true;
                }
                else
                {
                    if (this.RequirePaperNotice(recipients, sendTo, regID))
                    {
                        PaperNotification notify = new PaperNotification(subject, LogThreadID);
                        if (notify.SendWorkFlowEngineNotification(regID.ToString(), body)) result = true;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(recipients))
                        {
							// OHPNM-13860 JLB -- If no valid email recipients found (should only happen very rarely), allow workflow to progress as if emails were sent
							result = true;

							//throw new Exception(string.Format(Constants.LogString.WorkflowError,
							//       "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
							//       "Process Id: " + ProcessID.ToString() + ", Registration Id: " + regID.ToString() +
							//       " - no print mail sent")); 
                        }
                        else
                        {
                            EMailNotification notify = new EMailNotification(body, subject, recipients, LogThreadID);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), body)) result = true;

                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //OHPNM-20275 told to ignore the error and continue with the process to move to the next step
                //So, we are adding the error with stack starce to the step error so that we can still look at the actual error later and get better information to fix it
                var stepID = WorkflowController.StartStep(ProcessID, Constants.appWorkflowUserId);
                var msg = string.Format(Constants.LogString.WorkflowError,
                    "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                    "Process Id: " + ProcessID.ToString() + ", Registration Id: " + regID.ToString() + " - " + ex.ToString());
                WorkflowController.InsertExceptionForProcessTask(msg, ProcessID, stepID);

                result = true;
                //throw new Exception(string.Format(Constants.LogString.WorkflowError,
                //    "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                //    "Process Id: " + ProcessID.ToString() + ", Registration Id: " + regID.ToString() + " - " + ex.ToString()));
            }
            return result;
        }
        override public string NextStep()
        {
            return null;
        }


        public bool FixForProd(int regID, string body, string subject)
        {
            bool result = false;

            //int regID = Convert.ToInt32(GetProcessParameter("REGISTRATION_ID"));
            //string body = GetTaskParameter("EMAIL_BODY_TEMPLATE");
            //string subject = GetTaskParameter("EMAIL_SUBJECT");


            string recipients = GetRecipients(SendToTypeProvider, regID);


            if (this.RequirePaperNotice(recipients, SendToTypeProvider, regID))
            {
                //PaperNotification notify = new PaperNotification(subject, LogThreadID);
                //if (notify.SendWorkFlowEngineNotification(regID.ToString(), body)) result = true;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(recipients))
                {
                    // OHPNM-13860 JLB -- If no valid email recipients found (should only happen very rarely), allow workflow to progress as if emails were sent
                    result = true;

                    //throw new Exception(string.Format(Constants.LogString.WorkflowError,
                    //       "Assembly: " + Assembly.GetExecutingAssembly().GetName().Name,
                    //       "Process Id: " + ProcessID.ToString() + ", Registration Id: " + regID.ToString() +
                    //       " - no print mail sent")); 
                }
                else
                {
                    EMailNotification notify = new EMailNotification(body, subject, recipients, LogThreadID);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), body)) result = true;

                }

            }
            return result;

        }

        private bool RequirePaperNotice(string recipients, int sendTo, int regID)
        {
            bool isPSEmailType = false;
            // If email address is maximus.com or nebraska.com and the communication type is provider, send paper mail
            //List<SqlParameter> sqlParms = new List<SqlParameter>();
            //sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
            //DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegId", sqlParms, "RegData");
            
            //int ProviderTypeID = 0;
            //int ReferralID = 0;

            //if (Methods.HasRows(dsReg))
            //{
            //    DataRow drReg = dsReg.Tables[0].Rows[0];
            //    ProviderTypeID = Methods.GetIntValue(drReg, "ProviderTypeID");
            //    ReferralID = Methods.GetIntValue(drReg, "ReferralID");
            //}

            //bool nfocusProvider = (ReferralID > 0 && (ProviderTypeID == 82 || ProviderTypeID == 128));
            //if (sendTo == SendToTypeProvider || (sendTo == 4 && nfocusProvider))
            if (sendTo == SendToTypeProvider)
            {
                if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                {
                    isPSEmailType = true;
                }
            }
            return isPSEmailType;
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
                throw CoreException.ThrowException(ex);
            }

            return addrList.ToString();
        }


    }
}
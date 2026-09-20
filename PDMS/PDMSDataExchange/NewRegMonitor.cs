using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class NewRegMonitor : BaseJob, IJob
    {
        public NewRegMonitor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;

        override public void ExecuteJob()
        {
            // Default Job - CAQH Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("F34E00AB-D6B6-48BE-AF92-D587A5484E3D"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "F34E00AB-D6B6-48BE-AF92-D587A5484E3D")
            {
                SendOutEmails();
                string TerminateNewRegOverDueRegistrations = AppSettings.Get("TerminateNewRegOverDueRegistrations").ToString();
                if (TerminateNewRegOverDueRegistrations == "true")
                {
                    TerminateRegistrations();
                }
            }
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

        public void SendOutEmails()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry("Checking for providers that need auto-termination");
                int total = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                int NoticeDays = Convert.ToInt32( AppSettings.Get("RegistrationDueNoticeDays"));
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, NoticeDays, false));
                DataSet Noticeds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationDue",parameters,"NoticeRegIds");
                if (ObjectControllerHelper.HasRows(Noticeds))
                {
                    total = SendEmail(Noticeds, "EMAIL_TEMPLATE_REGISTRATION_DUE_NOTICE", NoticeDays, Constants.NoticeTypeID.Initial);
                }
                log.CreateLogEntry(String.Format("Total providers sent registration due emails {0}", total.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
        private int SendEmail(DataSet ds,string Template_Name, int NoticeDays, int NoticeTypeID)
        {
            int total = 0;
            if (ObjectControllerHelper.HasRows(ds))
            {
               // Notification notify = new Notification(this.ThreadId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string subject = AppSettings.Get("StateName", "(State)") + " Medicaid Application Closed";

                    DataSet dsEmails = RegistrationController.SelectRegistrationEmails(ObjectControllerHelper.GetInt("REG_ID", row), subject);
                    // If we do not have an entry for sending the email then we need to send it
                    if (!ObjectControllerHelper.HasRows(dsEmails))
                    {
                        string regID = ObjectControllerHelper.GetInt("REG_ID", row).ToString();

                        string recipients = GetRecipients(Convert.ToInt32(regID), NoticeTypeID);

                        //if Paper Email Template then send both Paper mail and Email
                        string paperTemplates = AppSettings.Get("PaperEmailTemplates", string.Empty); 
                        if (paperTemplates.Contains(Template_Name))
                        {
                            PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                total += 1;
                            else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                                Logging.LogPriority.Error);

                            if (!string.IsNullOrEmpty(recipients))
                            {
                                EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId);
                                if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                    total += 1;
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
                                    Logging.LogPriority.Error);
                            }
                        }
                        else
                        {
                            if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                            {
                                PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                                if (notify.SendWorkFlowEngineNotification(regID, Template_Name)) total += 1;
                                else
                                    log.CreateLogEntry("Reg Id: " + regID + " - Error while generation Print Mail in DC Medicaid Application Closed",
                                    Logging.LogPriority.Error);
                            }
                            else
                            {
                                EMailNotification notify = new EMailNotification(Template_Name, subject, recipients, this.ThreadId);
                                if (notify.SendWorkFlowEngineNotification(regID, Template_Name)) total += 1;
                                //if (notify.SendWorkFlowEngineNotification(recipients, Template_Name, subject,
                                //    ObjectControllerHelper.GetString("REG_ID", row)))
                                //    total += 1;
                                else log.CreateLogEntry("Reg Id: " + ObjectControllerHelper.GetString("REG_ID", row) + " - problems sending DC Medicaid Application Closed",
                                    Logging.LogPriority.Error);
                            }
                        }
                    }
                }
            }
            return total;
        }
        public void TerminateRegistrations()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string EnrollmentStatusCode_Disenroll = Constants.EnrollmentStatusCode.TerminatedTechnical;
            try
            {
                log.CreateLogEntry("Checking for providers that didn't complete the application in 10 days");
                int total = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                
                int NoticeDays = Convert.ToInt32(AppSettings.Get("RegistrationDueNoticeDays"));
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, NoticeDays, false));
                DataSet ClosureOverDueds = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationDue", parameters, "ClosureOverDueRegIds");

                if (ObjectControllerHelper.HasRows(ClosureOverDueds))
                {
                    foreach (DataRow row in ClosureOverDueds.Tables[0].Rows)
                    {
                        
                        int reg_id = ObjectControllerHelper.GetInt("REG_ID", row);
                        int party_id = ObjectControllerHelper.GetInt("PARTY_ID", row);
                        
                        Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);
                        if (party_id > 0)
                        {
                            log.CreateLogEntry("Begin Dis-Enroll RegID:" + reg_id.ToString());
                            parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));
                            parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, DateTime.Now, false));
                            parameters.Add(SqlParms.CreateParameter("COMMENTS", DbType.String, "", false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
                            parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, true, false));
                            parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, EnrollmentStatusCode_Disenroll, false));
                            DataAccess.ExecuteStoredProcedure("usp_DisenrollProvider", parameters);
                            log.CreateLogEntry("End Dis-Enroll RegID:" + reg_id.ToString());
                        }

                        
                    }
                }
                log.CreateLogEntry(String.Format("Total providers disenrolled {0}", total.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
    }
}
using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class LicenseRevalidationMonitor : BaseJob, IJob
    {
        public LicenseRevalidationMonitor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;

        override public void ExecuteJob()
        {
            // Default Job - CAQH Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("5143BEEB-7677-47EB-82D5-C6CFC8434522"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "5143BEEB-7677-47EB-82D5-C6CFC8434522" || jobGuid == "27990615-AD65-4434-9BD2-6B474A772865")
            {
                SendOutEmails();
            }
            if (jobGuid == "892B6BFE-B0C3-402C-89F8-02A12838630A")
            {
                string TerminateExpiredOutOfStateLicenseRegistrations = AppSettings.Get("TerminateExpiredOutOfStateLicenseRegistrations").ToString();
                if (TerminateExpiredOutOfStateLicenseRegistrations == "true")
                {
                    DisEnrollRegistrations();
                }
            }
        }
        
        public void DisEnrollRegistrations()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                log.CreateLogEntry("Starting disenroll for license expiration providers");
                int total = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                int revalidationOverDue = -1;
                
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, revalidationOverDue, false));
                DataSet revalidationOverDueds = DataAccess.ExecuteStoredProcedure("usp_SelectLicenseRevalidationDue", parameters, "revalidationOverDueRegIds");                

                if (ObjectControllerHelper.HasRows(revalidationOverDueds))
                {
                    foreach (DataRow row in revalidationOverDueds.Tables[0].Rows)
                    {
                        int reg_id = ObjectControllerHelper.GetInt("REG_ID", row);
                        int ProcessID = ObjectControllerHelper.GetInt("PROCESS_ID", row);
                        int CurrentStepID = ObjectControllerHelper.GetInt("CURRENT_STEP_ID", row);

                        string license_Status = ObjectControllerHelper.GetString("LICENSE_STATUS", row);
                        string license_SubStatus = ObjectControllerHelper.GetString("LICENSE_SUBSTATUS", row);

                        if (license_Status != null)
                            license_Status = license_Status.ToUpper();
                        if (license_SubStatus != null)
                            license_SubStatus = license_SubStatus.ToUpper();

                        try
                        {
                            string enrollmentStatusReason = "IN"; //LICENSE/CERTIFICATION NOT ACTIVE 

                            if (license_SubStatus == "ABANDONED" && (license_Status == "INACTIVE" || license_Status == "CLOSED"))
                                enrollmentStatusReason = "39"; //LICENSE ABANDONED       
                            else if ((license_SubStatus == "EXPIRED" || license_SubStatus == "LAPSED") && license_Status == "INACTIVE")
                                enrollmentStatusReason = "3"; //LICENSE/CERTIFICATION NOT RENEWED                                  
                            else if (license_SubStatus == "SUSPENDED" && license_Status == "INACTIVE")
                                enrollmentStatusReason = "28"; //LICENSE SUSPEND - LICENSE BRD
                            else if (license_SubStatus == "RETIRED" && license_Status == "INACTIVE")
                                enrollmentStatusReason = "38"; //RETIRED  
                            else if (license_SubStatus == "DECEASED" && license_Status == "CLOSED")
                                enrollmentStatusReason = "19"; //DECEASED 
                            else if ((license_SubStatus == "PERMANENT REVOCATION" && license_Status == "CLOSED") || (license_SubStatus == "REVOCATION" && license_Status == "INACTIVE"))
                                enrollmentStatusReason = "2"; //LICENSE/CERTIFICATION REVOKED  

                            Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);
                           
                            parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));
                            parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, DateTime.Now, false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                            parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
                            parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, true, false));
                            parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, Constants.EnrollStatus.INACTIVE.ToString(), false));
                            parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReason, false));
                            parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, true, false));
                            DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters);

                            if (ProcessID > 0 && CurrentStepID > 0)
                            {
                               
                                List<SqlParameter> param = new List<SqlParameter>();
                                param.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, true));
                                DataAccess.ExecuteStoredProcedure("usp_WF_CancelWorkflowProcess", param);
                            }
                            //Notetext: Provider Terminated and Final Disposition is Terminated
                            SendDisenrollEmail(reg_id, "EMAIL_TEMPLATE_LICENSE_INACTIVE_TERMINATE", Constants.NoticeTypeID.OverDue, ProcessID);
                        }
                        catch (Exception ex)
                        { // not rethrowing so we can disenroll other providers 
                            log.CreateLogEntry("Failed to terminate provider for license expiration with Reg Id: " + reg_id + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
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
                int sendToTypeID = Constants.SendToTypeID.Provider;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
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

        public void SendOutEmails()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry("Checking for providers that need license expiration notice");
                //int initialNoticetotal = 0;
                int finalNoticetotal = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                //int initialNoticeDays = Convert.ToInt32( AppSettings.Get("LicenseDueInitialNoticeDays"));
                //int secondNoticeDays = Convert.ToInt32(AppSettings.Get("LicenseDue2ndNoticeDays"));
                int licenseNoticeDays = Convert.ToInt32(AppSettings.Get("LicenseDueNoticeDays"));

                //parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, initialNoticeDays, false));
                //DataSet InitialNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectLicenseRevalidationDue", parameters, "InitialNoticeRegIds");
                //parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, licenseNoticeDays, false));
                DataSet LicenseNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectLicenseRevalidationDue", parameters, "LicenseNoticeRegIds");

                //if (ObjectControllerHelper.HasRows(InitialNoticeds))
                //{
                //    initialNoticetotal += SendEmail(InitialNoticeds, "EMAIL_TEMPLATE_LICENSE_REVALIDATION_INITIAL_NOTICE", initialNoticeDays, Constants.NoticeTypeID.Initial);
                //}
                //if (ObjectControllerHelper.HasRows(SecondNoticeds))
                //{
                //    finalNoticetotal += SendEmail(SecondNoticeds, "EMAIL_TEMPLATE_LICENSE_REVALIDATION_SECOND_NOTICE", secondNoticeDays, Constants.NoticeTypeID.Final);
                //}
                if (ObjectControllerHelper.HasRows(LicenseNoticeds))
                {
                    finalNoticetotal += SendEmail(LicenseNoticeds, "EMAIL_TEMPLATE_LICENSE_REVALIDATION_INITIAL_NOTICE", licenseNoticeDays, Constants.NoticeTypeID.Final);
                }

               // log.CreateLogEntry(String.Format("Total providers sent license expiration 60 day expiration notice {0}", initialNoticetotal.ToString()));
                log.CreateLogEntry(String.Format("Total providers sent license expiration 30 day expiration notice {0}", finalNoticetotal.ToString()));
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
                //Notification notify = new Notification(this.ThreadId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                    int processID = ObjectControllerHelper.GetInt("PROCESS_ID", row);
                    try
                    {
                        string subject;
                        //int days = ObjectControllerHelper.GetInt("DaysUntilExpiration", row);

                        if (NoticeTypeID == Constants.NoticeTypeID.OverDue)
                            subject = "Termination for Inactive License";
                        else
                            subject = "License is Expiring";

                       
                        string recipients = GetRecipients(regID, NoticeTypeID);

                        //if Paper Email Template then send both Paper mail and Email
                        string paperTemplates = DataAccess.GetAppSetting("PaperEmailTemplates");
                        if (paperTemplates.Contains(Template_Name))
                        {
                            PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                            {
                                total += 1;
                                InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendEmail1");
                            }
                            else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                                Logging.LogPriority.Error);

                            if (!string.IsNullOrEmpty(recipients))
                            {
                                EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId);
                                if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    total += 1;
                                    InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendEmail2");
                                }
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
                                    Logging.LogPriority.Error);
                            }
                        }
                        else
                        {
                            if (Methods.RequirePaperNotice(recipients, AppSettings.Get("PSEmailTypes", string.Empty).Split(',')))
                            {
                                PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                                if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    total += 1;
                                    InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendEmail3");
                                }
                                else
                                    log.CreateLogEntry("Reg Id: " + regID + " - Error while generating paper license expiration notification ",
                                           Logging.LogPriority.Error);
                            }
                            else
                            {
                                EMailNotification notify = new EMailNotification(Template_Name, subject, recipients, this.ThreadId);
                                if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    total += 1;
                                    InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendEmail4");
                                }
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems sending email license expiration notification",
                                    Logging.LogPriority.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    { // not rethrowing so we can send emails to other providers 
                        log.CreateLogEntry("Failed to send notice type = " + NoticeTypeID + " license expiration notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                    }
                }
            }
            return total;
        }
        private void InsertProviderFeedNotes(int regID, string subject, int processID, string method)
        {
            try
            {
                string formattedSubject = " Sent Email - " + subject;

                DataSet ds = RegistrationController.CheckProviderJobNotesExists(regID, Constants.EnrollmentType.Job,
                    Constants.FinalDisposition.Disenrolled, formattedSubject);

                bool noteExists = ObjectControllerHelper.HasRows(ds) &&
                          ds.Tables[0].Rows[0]["ISEXISTS"]?.ToString() == "1";

                if (noteExists) return;

                RegistrationController.InsertProviderFeedNotes(
                        regID, 0, null, formattedSubject, null,
                        Constants.EnrollmentType.Job,
                        Constants.FinalDisposition.Disenrolled,
                        processID
                    );

            }
            catch (Exception ex)
            {
                string errorMessage = $"{method} Reg Id: {regID} - An error has occurred while Inserting Job Provider Notes. Message: {ex.Message ?? string.Empty}";
                log.CreateLogEntry(errorMessage, Logging.LogPriority.Error);
            }

        }
        private void SendDisenrollEmail(int regID, string Template_Name, int NoticeTypeID, int processID)
        {
            try
            {
                int TermEmailsSent = 0;
                string subject = "Termination for Inactive License";

                    string bccList = "";
                    string recipients = "";
                    recipients = GetRecipients(regID, NoticeTypeID);

                    //if Paper Email Template then send both Paper mail and Email
                    string paperTemplates = DataAccess.GetAppSetting("PaperEmailTemplates");
                    if (paperTemplates.Contains(Template_Name))
                    {
                        PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                        if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                    {
                        TermEmailsSent += 1;
                        InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendDisenrollEmail1");
                    }
                    else log.CreateLogEntry("Reg Id: " + regID + " - problems generating LicenseRevalidation paper notification",
                            Logging.LogPriority.Error);

                        if (!string.IsNullOrEmpty(recipients))
                        {
                            EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                            if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        {
                            TermEmailsSent += 1;
                            InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendDisenrollEmail2");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems sending LicenseRevalidation email",
                                Logging.LogPriority.Error);
                        }
                    }
                    else
                    {
                        if (ObjectControllerHelper.RequirePaperNotice(recipients, DataAccess.GetAppSetting("PSEmailTypes").Split(',')))
                        {
                            PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        {
                            TermEmailsSent += 1;
                            InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendDisenrollEmail3");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems generating LicenseRevalidation paper notification",
                                Logging.LogPriority.Error);
                        }
                        else
                        {
                            EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        {
                            TermEmailsSent += 1;
                            InsertProviderFeedNotes(regID, subject, processID, "Job/LicenseRevalidationMonitor/SendDisenrollEmail4");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems sending LicenseRevalidation email",
                                Logging.LogPriority.Error);
                        }
                    }
            }
            catch (Exception ex)
            { // not rethrowing so we can send emails to other providers 
                log.CreateLogEntry("Failed to send notice type = " + NoticeTypeID + " LicenseRevalidation notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }
    }
}
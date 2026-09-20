using Corp.Core.Libraries;
using Corp.Core.Libraries.Helper;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;
namespace MAXIMUS.DataExchange.PDMS
{
    public class RevalidationMonitor : BaseJob, IJob
    {
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }
        public RevalidationMonitor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private Logging log = null;
        private int TermEmailsSent = 0;

        override public void ExecuteJob()
        {
            // Default Job - CAQH Retrieve Return Roster
            this.ExecuteJob(Guid.Parse("57E9F87D-69A1-465D-8856-32C7EBA4F54D"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == "57E9F87D-69A1-465D-8856-32C7EBA4F54D" || jobGuid == "E5D9EDFD-D88F-4FB6-9EDA-7267036FA86D")
            {
                SendOutEmails();
            }
            if (jobGuid == "E8B3B5D8-CBC8-443A-B566-ABC77517E1B9")
            {
                string TerminateRegistrationsPeriodicExactMatch = AppSettings.Get("TerminateRegistrationsPeriodicExactMatch").ToString();
                if (TerminateRegistrationsPeriodicExactMatch == "true")
                {
                    PeriodicScreeningAutoTerminateRegistrations();      // Auto Terminate the registrations that have exact match found in the recent monthly periodic screening event.
                }
            }
            if (jobGuid == "A7D1BA20-0EC1-4842-B891-934A8BB29FC1")
            {
                string TerminateRegistrationseLicenseInactive = AppSettings.Get("TerminateRegistrationseLicenseInactive").ToString();
                if (TerminateRegistrationseLicenseInactive == "true")
                {
                    eLicenseTerminationNotices();      // Send Termination Notices for provider terminated in eLicense Monthly checks.
                }
            }
            if (jobGuid == "D0944C5F-3597-420F-89A8-EF459998324F")
            {
                string TerminateRegistrationsInactiveProvider = AppSettings.Get("TerminateRegistrationsInactiveProvider").ToString();
                if (TerminateRegistrationsInactiveProvider == "true")
                {
                    InactiveProviderTerminationNotices();      // Send Termination Notices for provider terminated in Inactive (No Claim) Process.
                }
            }
            if (jobGuid == "79040F05-BB36-42D5-B14F-8B64D5B797A4")
            {
                string SendTerminationTransactions = AppSettings.Get("SendTerminationTransactions").ToString();
                if (SendTerminationTransactions == "true")
                {
                    SendOutTransactionPayloads();      // Send transactions for all providers terminated through different job processes.
                }
            }
            if (jobGuid == "61827AFC-7F0D-45F8-BA6E-F3BCD96D6FA4")
            {
                string TerminateRevalidationOverDueRegistrations = AppSettings.Get("TerminateRevalidationOverDueRegistrations").ToString();
                if (TerminateRevalidationOverDueRegistrations == "true")
                {
                    DisEnrollRegistrations();
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

        private void GetRecipients(int regId, int noticeTypeID, out string recList, out string bccList)
        {
            string rtn = string.Empty;
            recList = string.Empty;
            bccList = string.Empty;
            try
            {
                int sendToTypeID = Constants.SendToTypeID.Provider;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeID, false));
                parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "EmailRecipients");
                recList = AddEmail(rtn, ds, "EmailAddress");

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Reg Id: " + regId.ToString() + ", GetRecipients - " + ex.Message, Logging.LogPriority.Error);
            }

            //return rtn;
        }


        public void SendOutEmails()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry("Checking for providers that need revalidation");
                //int total = 0;
                int initialNoticetotal = 0;
                int secondNoticetotal = 0;
                int thirdNoticetotal = 0;
                int finalNoticetotal = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                int initialNoticeDays = Convert.ToInt32(AppSettings.Get("RevalidationDueInitialNoticeDays"));
                int secondNoticeDays = Convert.ToInt32(AppSettings.Get("RevalidationDue2ndNoticeDays"));
                int thirdNoticeDays = Convert.ToInt32(AppSettings.Get("RevalidationDue3ndNoticeDays"));
                int FinalNoticeDays = Convert.ToInt32(AppSettings.Get("RevalidationDueFinalNoticeDays"));
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, initialNoticeDays, false));
                DataSet InitialNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationDue", parameters, "InitialNoticeRegIds");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, secondNoticeDays, false));
                DataSet SecondNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationDue", parameters, "SecondNoticeRegIds");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, thirdNoticeDays, false));
                DataSet ThirdNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationDue", parameters, "ThirdNoticeRegIds");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, FinalNoticeDays, false));
                DataSet FinalNoticeds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationDue", parameters, "FinalNoticeRegIds");
                if (ObjectControllerHelper.HasRows(InitialNoticeds))
                {
                    initialNoticetotal = SendEmail(InitialNoticeds, "EMAIL_TEMPLATE_REVALIDATION_INITIAL_NOTICE", initialNoticeDays, Constants.NoticeTypeID.Initial);
                }
                if (ObjectControllerHelper.HasRows(SecondNoticeds))
                {
                    secondNoticetotal += SendEmail(SecondNoticeds, "EMAIL_TEMPLATE_REVALIDATION_SECOND_NOTICE", secondNoticeDays, Constants.NoticeTypeID.Initial);
                }
                if (ObjectControllerHelper.HasRows(ThirdNoticeds))
                {
                    thirdNoticetotal += SendEmail(ThirdNoticeds, "EMAIL_TEMPLATE_REVALIDATION_THIRD_NOTICE", thirdNoticeDays, Constants.NoticeTypeID.Initial);
                }
                if (ObjectControllerHelper.HasRows(FinalNoticeds))
                {
                    finalNoticetotal += SendEmail(FinalNoticeds, "EMAIL_TEMPLATE_REVALIDATION_FINAL_NOTICE", FinalNoticeDays, Constants.NoticeTypeID.Final);
                }
                log.CreateLogEntry(String.Format("Total providers sent revalidation 120 day expiration notice {0}", initialNoticetotal.ToString()));
                log.CreateLogEntry(String.Format("Total providers sent revalidation 90 day expiration notice {0}", secondNoticetotal.ToString()));
                log.CreateLogEntry(String.Format("Total providers sent revalidation 60 day expiration notice {0}", thirdNoticetotal.ToString()));
                log.CreateLogEntry(String.Format("Total providers sent revalidation final notice {0}", finalNoticetotal.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
        private int SendEmail(DataSet ds, string Template_Name, int NoticeDays, int NoticeTypeID)
        {
            int total = 0;
            if (ObjectControllerHelper.HasRows(ds))
            {
                //Notification notify = new Notification(this.ThreadId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                    int processID = ObjectControllerHelper.GetInt("PROCESS_ID", row);
                    string mmisProviderTypeID = ObjectControllerHelper.GetString("MMIS_PROVIDER_TYPE_ID", row);
                    try
                    {
                        string subject;

                        if (NoticeTypeID == Constants.NoticeTypeID.OverDue)
                            subject = "Revalidation Termination";
                        else if (NoticeTypeID == 4)     //Periodic Screening Termination Notices
                            subject = "Federal Exclusions Interfaces Letter";
                        else if (NoticeTypeID == 5)     //eLicense Termination Notices
                            subject = "Termination Due to Inactive License";
                        else if (NoticeTypeID == 6)     //InactiveProvider Termination Notices
                        {
                            string medicaidID = ObjectControllerHelper.GetString("MEDICAID_ID", row);
                            subject = "Termination of Medicaid Provider Agreement Number " + medicaidID.Trim();
                        }
                        else if (NoticeTypeID == 7)
                            subject = "Provider Termination for License Notice";
                        else if (NoticeTypeID == 8)
                            subject = "NPPES – Denial/Termination Letter";
                        else if ((mmisProviderTypeID == "01" || mmisProviderTypeID == "02" || mmisProviderTypeID == "03" ||
                                  mmisProviderTypeID == "28" || mmisProviderTypeID == "86" || mmisProviderTypeID == "88" ||
                                  mmisProviderTypeID == "89") && NoticeDays == 30)
                            subject = "FINAL NOTICE for Revalidation of Time-Limited Provider Agreement and Impact to Payments";
                        else
                            subject = "Revalidation " + NoticeDays + " days Notice";


                        string bccList = "";
                        string recipients = "";
                        GetRecipients(regID, NoticeTypeID, out recipients, out bccList);

                        //if Paper Email Template then send both Paper mail and Email
                        string paperTemplates = DataAccess.GetAppSetting("PaperEmailTemplates");
                        if (paperTemplates.Contains(Template_Name))
                        {
                            PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                            {
                                total += 1;
                                InsertProviderFeedNotes(regID, subject, processID, "InsertProviderFeedNotes1");
                            }
                            else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                                Logging.LogPriority.Error);

                            if (!string.IsNullOrEmpty(recipients))
                            {
                                EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                                if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    total += 1;
                                    InsertProviderFeedNotes(regID, subject, processID, "InsertProviderFeedNotes2");
                                }
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
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
                                    total += 1;

                                    InsertProviderFeedNotes(regID, subject, processID, "InsertProviderFeedNotes3");
                                }
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                                        Logging.LogPriority.Error);
                            }
                            else
                            {
                                EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                                if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    total += 1;
                                    InsertProviderFeedNotes(regID, subject, processID, "InsertProviderFeedNotes4");

                                }
                                else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
                                        Logging.LogPriority.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    { // not rethrowing so we can send emails to other providers 
                        log.CreateLogEntry("Failed to send notice type = " + NoticeTypeID + " Revalidation notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
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
                    Constants.FinalDisposition.Terminated, formattedSubject);

                bool noteExists = ObjectControllerHelper.HasRows(ds) &&
                          ds.Tables[0].Rows[0]["ISEXISTS"]?.ToString() == "1";

                if (noteExists) return;

                RegistrationController.InsertProviderFeedNotes(
                       regID, 0, null, formattedSubject, null,
                       Constants.EnrollmentType.Job,
                       Constants.FinalDisposition.Terminated,
                       processID
                   );

            }
            catch (Exception ex)
            {
                string errorMessage = $"{method} Reg Id: {regID} - An error occurred while inserting Job Provider Notes. Message: {ex.Message ?? string.Empty}";
                log.CreateLogEntry(errorMessage, Logging.LogPriority.Error);
            }

        }
        public void DisEnrollRegistrations()
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string enrollmentStatusReason = Constants.EnrollStatusReason.FAILURETOREVALIDATE.ToString();  // 31-Failure to Re-Validate
            string enrollmentStatusID = string.Empty;
            try
            {
                log.CreateLogEntry("Starting disenroll for revalidation failed providers");
                int total = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                int revalidationOverDue = -1;

                parameters.Add(SqlParms.CreateParameter("Days", DbType.Int32, revalidationOverDue, false));
                DataSet revalidationOverDueds = DataAccess.ExecuteStoredProcedure("usp_SelectRevalidationDue", parameters, "revalidationOverDueRegIds");

                //int emailsSent = SendEmail(revalidationOverDueds, "EMAIL_TEMPLATE_REVALIDATION_OVERDUE_TERMINATE_NOTICE", -1, Constants.NoticeTypeID.OverDue);

                if (ObjectControllerHelper.HasRows(revalidationOverDueds))
                {
                    foreach (DataRow row in revalidationOverDueds.Tables[0].Rows)
                    {
                        int reg_id = ObjectControllerHelper.GetInt("REG_ID", row);
                        int ProcessID = ObjectControllerHelper.GetInt("PROCESS_ID", row);
                        int CurrentStepID = ObjectControllerHelper.GetInt("CURRENT_STEP_ID", row);
                        try
                        {
                            DateTime changeEffectiveDate = ObjectControllerHelper.GetDateTime("change_effective_date", row);
                            DateTime enddate = ObjectControllerHelper.GetDateTime("end_date", row);
                            string enrollmentStatusCode = ObjectControllerHelper.GetString("enrollment_status_code", row);
                            string MMISproviderTypeID = ObjectControllerHelper.GetString("MMIS_PROVIDER_TYPE_ID", row);

                            List<SqlParameter> sqlParms = new List<SqlParameter>();
                            sqlParms.Add(SqlParms.CreateParameter("RegID", DbType.Int32, reg_id, false));
                            DataSet dsReg = DataAccess.ExecuteStoredProcedure("usp_SelectREGISTRATIONByRegID", sqlParms, "RegData");

                            dsReg.Tables[0].TableName = "RegData";
                            DataRow drReg = ObjectControllerHelper.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                            bool requiresCredentialing = ObjectControllerHelper.GetBool("CredentialingRequired", drReg);
                            bool credentialedByDelegate = ObjectControllerHelper.GetBool("DelegateCredentialingRequired", drReg);
                            bool isHospitalBasedProvider = CredentialController.IsHospitalBasedProvider(reg_id);
                            requiresCredentialing = (credentialedByDelegate || isHospitalBasedProvider) ? false : requiresCredentialing;
                            DateTime EndDateTime = ObjectControllerHelper.GetDateTime("EndDate", drReg);

                            parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, reg_id, true));
                            bool isDoDDIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfDODDInitialApplication", parameters));

                            parameters = new List<SqlParameter>();
                            parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, reg_id, true));
                            bool isODAIntialApp = Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfODAInitialApplication", parameters));

                            bool isTerminateReg = true;

                            if (!requiresCredentialing && (isDoDDIntialApp || isODAIntialApp))
                            {
                                isTerminateReg = false; // Do not terminate a Non-credentialed ODM provider in DODD/ODA ADD services WF.
                            }

                            if (isTerminateReg)
                            {
                                if (MMISproviderTypeID == "01" || MMISproviderTypeID == "02" || MMISproviderTypeID == "03" || MMISproviderTypeID == "28" || MMISproviderTypeID == "86" ||
                                MMISproviderTypeID == "88" || MMISproviderTypeID == "89" || MMISproviderTypeID == "59" || MMISproviderTypeID == "74" || MMISproviderTypeID == "10") // SAM635 ADD 59,74 AND 10 TO THIS LIST
                                {
                                    enrollmentStatusID = Constants.EnrollStatus.ACTIVE.ToString();
                                    //Add a record in restriction service for this type
                                    CreateRestrictionServicesRecord(reg_id, EndDateTime);
                                }
                                else
                                {
                                    enrollmentStatusID = Constants.EnrollStatus.INACTIVE.ToString();

                                    Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);

                                    parameters = new List<SqlParameter>();
                                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_id, true));
                                    parameters.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, DateTime.Now, false));
                                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
                                    parameters.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, true, false));
                                    parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, enrollmentStatusID, false));
                                    parameters.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, enrollmentStatusReason, false));
                                    parameters.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, true, false));
                                    DataAccess.ExecuteStoredProcedure("usp_TerminateProvider", parameters);

                                    if (ProcessID > 0 && CurrentStepID > 0)
                                    {
                                        List<SqlParameter> param = new List<SqlParameter>();
                                        param.Add(SqlParms.CreateParameter("PROCESS_ID", DbType.Int32, ProcessID, true));
                                        DataAccess.ExecuteStoredProcedure("usp_WF_CancelWorkflowProcess", param);
                                    }

                                    SendDisenrollEmail(reg_id, "EMAIL_TEMPLATE_REVALIDATION_OVERDUE_TERMINATE_NOTICE", Constants.NoticeTypeID.OverDue, ProcessID);
                                }
                            }
                            else
                            {
                                log.CreateLogEntry("Not Terminate RegID:" + reg_id.ToString());
                            }

                            total++;
                        }
                        catch (Exception ex)
                        { // not rethrowing so we can disenroll other providers 
                            log.CreateLogEntry("Failed to disenroll provider for revalidation with Reg Id: " + reg_id + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                        }
                    }
                }
                log.CreateLogEntry(String.Format("Total provider terminiation notices sent {0}", TermEmailsSent.ToString()));
                log.CreateLogEntry(String.Format("Total providers disenrolled {0}", total.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        public void PeriodicScreeningAutoTerminateRegistrations()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            //string enrollmentStatusReason = Constants.EnrollStatusReason.TERMINATEDFEDEXCLUSIONSYSTEM; // 01 - TERMINATED - FED EXCLUSION (SYSTEM)
            string enrollmentStatusID = string.Empty;
            try
            {
                log.CreateLogEntry("Starting periodic screening auto termination notices");
                int total = 0; int periodicScreeningEventID = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EventType_MonthlyDBChecks", DbType.Int32, 1, true));
                DataSet autoTermds = DataAccess.ExecuteStoredProcedure("usp_SelectReg_Terminated_byMonthlyChecks", parameters, "autoTermRegIds");

                if (ObjectControllerHelper.HasRows(autoTermds))
                {
                    DataSet dsEx, dsExNPI;
                    //Initiate exclusions with the autoterminations
                    dsExNPI = dsEx = autoTermds;
                    //Seperate out between NPPES AND other DB Matches
                    string nppesDeactiveFilter = "NPPES_PK = 1";
                    string nppesActiveFilter = "NPPES_PK = 0";

                    DataView dvNPIdeactive = dsExNPI.Tables[0].DefaultView;
                    dvNPIdeactive.RowFilter = nppesDeactiveFilter;
                    dvNPIdeactive.RowStateFilter = DataViewRowState.CurrentRows;
                    var dsNPPESEx = new DataSet();
                    dsNPPESEx.Tables.Add(dvNPIdeactive.ToTable());
                    int npiinactive = dsNPPESEx.Tables[0].Rows.Count;

                    DataView dvNPIActive = dsEx.Tables[0].DefaultView;
                    dvNPIActive.RowFilter = nppesActiveFilter;
                    dvNPIActive.RowStateFilter = DataViewRowState.CurrentRows;
                    var dsExFiltered = new DataSet();
                    dsExFiltered.Tables.Add(dvNPIActive.ToTable());
                    int npiactive = dsExFiltered.Tables[0].Rows.Count;





                    if (ObjectControllerHelper.HasRows(dsNPPESEx))
                    {
                        int totalDeactiveNPI = 0;
                        totalDeactiveNPI = SendEmail(dsNPPESEx, "EMAIL_TEMPLATE_SCREENING_NPPES_TERMINATION", -1, 8);
                        total = total + totalDeactiveNPI;
                    }
                    if (ObjectControllerHelper.HasRows(dsExFiltered))
                    {
                        int totalEx = 0;
                        totalEx = SendEmail(dsExFiltered, "EMAIL_TEMPLATE_SCREENING_AUTO_TERMINATION", -1, 4);
                        total = total + totalEx;

                    }

                    if (autoTermds.Tables[1].Rows.Count > 0)
                    {
                        periodicScreeningEventID = ObjectControllerHelper.GetInt("PeriodicScreeningEventID", autoTermds.Tables[1].Rows[0]);
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("PeriodicScreeningEventID", DbType.Int32, periodicScreeningEventID, true));
                        DataAccess.ExecuteStoredProcedure("usp_Mark_TerminationEmailsSent_forMonthlyChecks", param);
                    }

                }
                log.CreateLogEntry(String.Format("Total Periodic screening auto terminiation notices sent {0}", total.ToString()));

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }
        public void eLicenseTerminationNotices()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                log.CreateLogEntry("Start sending eLicense termination notices");
                int total = 0; int periodicScreeningEventID = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EventType_MonthlyDBChecks", DbType.Int32, 3, true));
                DataSet autoTermds = DataAccess.ExecuteStoredProcedure("usp_SelectReg_Terminated_byMonthlyChecks", parameters, "autoTermRegIds");
                if (ObjectControllerHelper.HasRows(autoTermds))
                {
                    total = SendEmail(autoTermds, "EMAIL_TEMPLATE_ELICENSE_TERMINATION_NOTICE", -1, 7);
                    if (autoTermds.Tables[1].Rows.Count > 0)
                    {
                        periodicScreeningEventID = ObjectControllerHelper.GetInt("PeriodicScreeningEventID", autoTermds.Tables[1].Rows[0]);
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("PeriodicScreeningEventID", DbType.Int32, periodicScreeningEventID, true));
                        DataAccess.ExecuteStoredProcedure("usp_Mark_TerminationEmailsSent_forMonthlyChecks", param);
                    }

                }

                log.CreateLogEntry(String.Format("Total eLicense termination notices sent {0}", total.ToString()));

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }

        public void InactiveProviderTerminationNotices()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                log.CreateLogEntry("Start sending InactiveProviderFile processed termination notices");
                int total = 0; int periodicScreeningEventID = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("EventType_MonthlyDBChecks", DbType.Int32, 4, true));
                DataSet autoTermds = DataAccess.ExecuteStoredProcedure("usp_SelectReg_Terminated_byMonthlyChecks", parameters, "autoTermRegIds");
                if (ObjectControllerHelper.HasRows(autoTermds))
                {
                    total = SendEmail(autoTermds, "EMAIL_TEMPLATE_INACTIVE_PROVIDER_TERMINATE", -1, 6);
                    if (autoTermds.Tables[1].Rows.Count > 0)
                    {
                        periodicScreeningEventID = ObjectControllerHelper.GetInt("PeriodicScreeningEventID", autoTermds.Tables[1].Rows[0]);
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(SqlParms.CreateParameter("PeriodicScreeningEventID", DbType.Int32, periodicScreeningEventID, true));
                        DataAccess.ExecuteStoredProcedure("usp_Mark_TerminationEmailsSent_forMonthlyChecks", param);
                    }
                }

                log.CreateLogEntry(String.Format("Total InactiveProviderFile processed termination notices sent {0}", total.ToString()));

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }

        }

        private void CreateRestrictionServicesRecord(int Reg_id, DateTime EndDateTime)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                bool isAddRestrict = true;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, Reg_id, true));
                DataSet dsRegRestrict = DataAccess.ExecuteStoredProcedure("usp_SelectREG_RESTRICTION", parameters, "regRestrict");
                foreach (DataRow dr in dsRegRestrict.Tables[0].Rows)
                {
                    //OHPNM-17738 - if the end date of the existing restriction is earlier than the current date, create a new restriction record
                    DateTime restrictionEndDate = ObjectControllerHelper.GetDateTime("END_DATE", dr);

                    if (restrictionEndDate > DateTime.Now)
                    {
                        isAddRestrict = false;
                        break;
                    }
                }

                if (isAddRestrict)
                {
                    DateTime EndDate = new DateTime(2299, 12, 31);
                    Guid userid = new Guid(Constants.appPDMSDataExchangeUserId);

                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, Reg_id, true));
                    parameters.Add(SqlParms.CreateParameter("EFFECTIVE_DATE", DbType.DateTime, EndDateTime.AddDays(1), false)); // SAM635  set the restricted services span effective date to the revalidation due date +1
                    parameters.Add(SqlParms.CreateParameter("END_DATE", DbType.DateTime, EndDate, false));
                    parameters.Add(SqlParms.CreateParameter("IS_EXCLUDE", DbType.String, Constants.RSIncludeExclude.Exclude, false));
                    parameters.Add(SqlParms.CreateParameter("REVIEW_TYPE_ID", DbType.Int32, Convert.ToInt32(Constants.RSFullReview1TypeId), false));
                    parameters.Add(SqlParms.CreateParameter("REVIEW_REASON_ID", DbType.Int32, Convert.ToInt32(Constants.RSReviewReasonId), false));
                    parameters.Add(SqlParms.CreateParameter("IS_RESTRICT", DbType.Boolean, true, false));
                    parameters.Add(SqlParms.CreateParameter("STATUS_CODE", DbType.Int32, Constants.RSStatusCode.Active, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, userid, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, userid, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("MODIFIED_STATUS_TYPE_ID", DbType.Int32, Constants.RegistrationModifiedStatusType.Inserted, false));
                    parameters.Add(SqlParms.CreateParameter("SENT_TO_SI", DbType.Boolean, false, false));
                    DataAccess.ExecuteStoredProcedure("insertREG_RESTRICTION", parameters);

                    //if (this.WorkflowPage.CurrentTaskName == "" && ObjectControllerHelper.IsUserInRestrictedServiceViewRole(userid.ToString()))


                    DataSet dsProcess = RegistrationController.SelectRegistrationByRegID(Reg_id);
                    DataRow drProcess = dsProcess.Tables[0].Rows[0];
                    if (Methods.GetStringValue(drProcess, "CurrentTaskName") == string.Empty)
                    {
                        int transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
                        int serviceLocationID = 0;
                        string txnResult = string.Empty;

                        serviceLocationID = ObjectControllerHelper.GetInt("REG_SERVICE_LOCATION_ID", drProcess);

                        int tqId = TransactionController.InsertTransactionQueue(
                                transactionType,
                                Reg_id,
                                serviceLocationID,
                                DateTime.Now,
                                null,
                                null,
                                DateTime.Now,
                                userid.ToString(),
                                true);

                        List<SqlParameter> paramWF = new List<SqlParameter>();
                        paramWF.Add(SqlParms.CreateParameter("ProcessID", DbType.Int32, Methods.GetIntValue(drProcess, "ProcessID"), true));
                        paramWF.Add(SqlParms.CreateParameter("ParameterName", DbType.String, CON.ProcessParameter.TransactionQueueID, true));
                        paramWF.Add(SqlParms.CreateParameter("ParameterValue", DbType.String, tqId.ToString(), true));
                        DataAccess.ExecuteStoredProcedure("usp_WF_SaveProcessParameter", paramWF);

                    }
                }
                else
                {
                    log.CreateLogEntry("Not adding Restrcited services as it already has- RegID:" + Reg_id.ToString());
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to Add Restriction services for with Reg Id: " + Reg_id + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }


        }
        public void SendOutTransactionPayloads()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            int total = 0;
            try
            {
                log.CreateLogEntry("Fetching termination transactions to send.");
                string appEnv = AppSettings.Get("MakeWSRequestCallToSI");

                if (appEnv.ToLower().Equals("true"))
                {
                    DataSet dsTrans = new DataSet();
                    dsTrans = DataAccess.ExecuteStoredProcedure("usp_SelectTerminationTransactions", "TerminationTransactions");
                    if (ObjectControllerHelper.HasRows(dsTrans))
                    {
                        foreach (DataRow row in dsTrans.Tables[0].Rows)
                        {
                            int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                            int transactionID = ObjectControllerHelper.GetInt("TRANSACTION_QUEUE_ID", row);
                            int transactionTypeID = ObjectControllerHelper.GetInt("TRANSACTION_TYPE_ID", row);
                            int sendHistory = ObjectControllerHelper.GetInt("SEND_HISTORY_BY_JOB", row);

                            try
                            {
                                SendTerminationTransactionPayload stp = new SendTerminationTransactionPayload();
                                stp.SendTransaction(transactionID, transactionTypeID, sendHistory > 0 ? true : false);
                                total += 1;
                            }
                            catch (Exception ex)
                            {
                                log.CreateLogEntry("Failed to send for transaction id :- " + transactionID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                            }

                        }
                    }

                    log.CreateLogEntry("Finish sending transactions. Total transactions successfully sent - " + total);
                }
                else
                {
                    log.CreateLogEntry("MakeWSRequestCallToSI setting is false. Dont send transactions now.");
                }

            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }
        private void SendDisenrollEmail(int regID, string Template_Name, int NoticeTypeID, int processID)
        {
            try
            {
                string subject = "Revalidation Termination";

                string bccList = "";
                string recipients = "";
                GetRecipients(regID, NoticeTypeID, out recipients, out bccList);

                //if Paper Email Template then send both Paper mail and Email
                string paperTemplates = DataAccess.GetAppSetting("PaperEmailTemplates");
                if (paperTemplates.Contains(Template_Name))
                {
                    PaperNotification notify = new PaperNotification(subject, this.ThreadId);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                    {
                        TermEmailsSent += 1;
                        InsertProviderFeedNotes(regID, subject, processID, "SendDisenrollEmail1");
                    }
                    else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                        Logging.LogPriority.Error);

                    if (!string.IsNullOrEmpty(recipients))
                    {
                        EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                        if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        {
                            TermEmailsSent += 1;
                            InsertProviderFeedNotes(regID, subject, processID, "SendDisenrollEmail2");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
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
                            InsertProviderFeedNotes(regID, subject, processID, "SendDisenrollEmail3");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems generating Revalidation paper notification",
                                Logging.LogPriority.Error);
                    }
                    else
                    {
                        EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                        if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        {
                            TermEmailsSent += 1;
                            InsertProviderFeedNotes(regID, subject, processID, "SendDisenrollEmail4");
                        }
                        else log.CreateLogEntry("Reg Id: " + regID + " - problems sending Revalidation email",
                                Logging.LogPriority.Error);
                    }
                }
            }
            catch (Exception ex)
            { // not rethrowing so we can send emails to other providers 
                log.CreateLogEntry("Failed to send notice type = " + NoticeTypeID + " Revalidation notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }
    }
}
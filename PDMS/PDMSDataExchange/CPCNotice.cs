using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using CON = MAXIMUS.Core.Libraries.Constants;
namespace MAXIMUS.DataExchange.PDMS
{
    public class CPCNotice : BaseJob, IJob
    {

        private const string CPCInvitationRemindersJobId = "47467DCF-D57D-4BD5-AD55-75BBF00002E8";
        private const string CPCFinalInvitationRemindersJobId = "8E5EEAE2-2CF5-43A6-8164-898FC4E1A426";
        private const string CPCInvitationsJobId = "8195E7DA-A130-4BAB-AF84-A8958086A086";
        private const string CPCNonRenewalNotificationJobId = "225F3605-9F9E-400A-82F5-A1FE042959DB";
        private const string CPCTerminationsJobId = "6C7A7DB3-2885-4407-8625-9F2231A0E16A";
        private String template = string.Empty;
        private string subject = string.Empty;
        private Logging log = null;


        private DataSet recipients = null;
        private Guid guid;
        public CPCNotice(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(CPCInvitationsJobId));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            SendCPCInvitation(jobGuid);            
            this.guid = jobId;
        }

        private void SendCPCInvitation(string jobGuid)
        {
            recipients = null;
            try
            {
                switch (jobGuid)
                {
                    case CPCInvitationsJobId:
                        this.template = CON.CPCLetterTemplateType.CPCInvitation;
                        this.subject = "Comprehensive Primary Care Invitation";
                        this.getCPCRecipients(CON.CPC_CommunicationLetterTypeID.CPC_INVITATION_LETTER);
                        break;
                    case CPCInvitationRemindersJobId:
                        this.template = CON.CPCLetterTemplateType.CPCRemainder;
                        this.subject = "Comprehensive Primary Care Invitation Reminder";
                        this.getCPCRecipients(CON.CPC_CommunicationLetterTypeID.CPC_FIRST_REMAINDER_LETTER);
                        break;
                    case CPCFinalInvitationRemindersJobId:
                        this.template = CON.CPCLetterTemplateType.CPCFinalRemainder;
                        this.subject = "Comprehensive Primary Care Invitation Final Reminder";
                        this.getCPCRecipients(CON.CPC_CommunicationLetterTypeID.CPC_FINAL_REMAINDER_LETTER);
                        break;
                    case CPCNonRenewalNotificationJobId:
                        this.template = CON.CPCLetterTemplateType.CPCNonRenewal;
                        this.subject = "Comprehensive Primary Care Non-Renewal Notification";
                        this.getCPCRecipients(CON.CPC_CommunicationLetterTypeID.CPC_NON_RENEWAL_NOTIFICATION_LETTER);
                        break;
                    case CPCTerminationsJobId:
                        EndDateCPCAfterOpenEnrollmentEnds();
                        break;
                }
            }
            catch(Exception ex)
            {
                log.CreateLogEntry("Failed to fetch the "+this. template +" Recipients. Exception Message: "
                                + ex.Message +" Exception Stack: "+ ex.StackTrace, Logging.LogPriority.Error);      
            }
        }

        private void SendNotice(string template)
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string emailSubject = this.subject;
            if (Methods.HasRows(recipients))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                int sendToTypeId = 17;
                DataSet ds;
                foreach (DataRow row in recipients.Tables[0].Rows)
                {
                    int regId = ObjectControllerHelper.GetInt("REG_ID", row);
                    parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regId, false));
                    parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeId, false));
                    ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "RecipientEmail");
                    if (Methods.HasRows(ds))
                    {
                        string recipientEmail = Methods.GetStringValue(ds.Tables[0].Rows[0], "EmailAddress");
                        if (!String.IsNullOrEmpty(recipientEmail) && !regId.Equals("-1"))
                        {
                            EMailNotification enotify = new EMailNotification(template, emailSubject, recipientEmail, this.guid);
                            enotify.SendWorkFlowEngineNotification(regId.ToString(), template);
                            log.CreateLogEntry(template + " - Email sent for provider with registration id " + regId);
                        }
                        else
                        {
                            log.CreateLogEntry("Email ID is missing for provider with registration id " + regId);
                        }
                    }
                    parameters.Clear();
                    ds.Clear();
                }

            }
            else
            {
                log.CreateLogEntry(template + "- No Providers to send email.");
            }
        }

        private void getCPCRecipients(int letterTypeId)
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Getting regid's to send the notices.");
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CPC_COMMUNICATION_LETTER_TYPE_ID", DbType.Int32, letterTypeId, false));
            recipients = DataAccess.ExecuteStoredProcedure("usp_SelectCPCEmailRecipients", parameters, "EmailRecipients");
            SendNotice(this.template);
        }
        private void EndDateCPCAfterOpenEnrollmentEnds()
        {
            try
            {
                // Create log object
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                log = new Logging(this.ThreadId, logMsg);
                log.CreateLogEntry("Start EndDateCPCAfterOpenEnrollmentEnds.");
                //DataSet dsKidsCPC = ProviderController.GetKidsSpecialtyRegistrations();  // OHPNM-13452 - included it GetNotReattestedCPCRegistrations()
                //DataSet dsPP = ProviderController.GetPPOldCPCMembers();   //OHPNM-13452 - Not required as year end job
                DataSet dsProv = ProviderController.GetNotReattestedCPCRegistrations();
                DateTime dtNow = DateTime.Now;
                
                //CPC 
                if (ObjectControllerHelper.HasRows(dsProv))
                {
                    log.CreateLogEntry("Processing Not Reattested CPC Registrations.");
                    DataTable dtCPC = dsProv.Tables[0];
                    if (dtCPC.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtCPC.Rows)
                        {
                            int regId = ObjectControllerHelper.GetInt("REG_ID", dr);
                            try
                            {
                                if (regId > 0)
                                {
                                    ProviderController.EndDateCPCProvider(regId, dtNow, CON.appAdminUserId, CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.Termination_Failure_to_Attest, true, true, false, false, 0);
                                    log.CreateLogEntry("dtCPC Terminating RegID - " + regId.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                log.CreateLogEntry("dtCPC -Error Terminating RegID - " + regId.ToString() + " - " + ex.Message, Logging.LogPriority.Error);
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("End Processing Not Reattested CPC Registrations RowCount = 0 ");
                    }

                    //End - date Kids specialty if no longer qualify.
                    log.CreateLogEntry("Start Processing CPC Enrollment Not Eligible For Kids Specialty.");
                    DataTable dtKidsCPC = dsProv.Tables[1];
                    if (dtKidsCPC.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtKidsCPC.Rows)
                        {
                            int regId = ObjectControllerHelper.GetInt("REG_ID", dr);
                            try
                            {
                                if (regId > 0)
                                {
                                    log.CreateLogEntry("End Date Kids Speciality on RegID - " + regId.ToString());
                                    ProviderController.EndDateCPCProvider(regId, dtNow, CON.appAdminUserId, CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.INACTIVE.ToString(), true, true, true);
                                    
                                }
                            }
                            catch (Exception ex)
                            {
                                log.CreateLogEntry("Error End Date Kids Speciality on RegID - " + regId.ToString() + " - " + ex.Message, Logging.LogPriority.Error);
                            }
                        }
                    }
                    else
                    {
                        log.CreateLogEntry("End End Date Kids Speciality RowCount = 0 ");
                    }
                }                
                
                log.CreateLogEntry("End EndDateCPCAfterOpenEnrollmentEnds.");
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                log.CreateLogEntry("Exception in EndDateCPCAfterOpenEnrollmentEnds process : " + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }
    }
}

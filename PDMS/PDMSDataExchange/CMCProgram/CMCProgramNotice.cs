using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.CMCProgram
{
    public class CMCProgramNotice : BaseJob, IJob
    {
        private const string CMCInvitationRemindersJobId = "B7A42525-8883-42AD-9C1F-8CF16ED03880";
        private const string CMCInvitationsJobId = "70245785-7B9E-4D6B-A2DF-DE152F69B495";
        private const string CMCNonRenewalNotificationJobId = "042B6870-439E-4AE5-B5C3-964F1099C06B";
        private const string CMCYearEndJobId = "C3BC2C3C-0BF1-45B0-ACDD-B6B7C94113CB";
        private String template = string.Empty;
        private string subject = string.Empty;
        private Logging log = null;
        private DataSet recipients = null;
        private Guid guid;

        public CMCProgramNotice(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(CMCInvitationsJobId));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            if (jobGuid == CMCYearEndJobId)
            {
                EndDateCMCAfterOpenEnrollmentEnds();
            }
            else if (jobGuid == CMCInvitationsJobId || jobGuid == CMCInvitationRemindersJobId || jobGuid == CMCNonRenewalNotificationJobId)
            {
                SendCMCInvitation(jobGuid);
            }
            
            this.guid = jobId;
        }

        private void SendCMCInvitation(string jobGuid)
        {
            recipients = null;
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                switch (jobGuid)
                {
                    case CMCInvitationsJobId:
                        this.template = CON.CMCLetterTemplateType.CMCInvitation;
                        this.subject = "Comprehensive Maternal Care Invitation";
                        this.GetCMCRecipients(CON.CMC_CommunicationLetterTypeID.CMC_INVITATION_LETTER);
                        break;
                    case CMCInvitationRemindersJobId:
                        this.template = CON.CMCLetterTemplateType.CMCRemainder;
                        this.subject = "Comprehensive Maternal Care Invitation Reminder";
                        this.GetCMCRecipients(CON.CMC_CommunicationLetterTypeID.CMC_FIRST_REMAINDER_LETTER);
                        break;
                    case CMCNonRenewalNotificationJobId:
                        this.template = CON.CMCLetterTemplateType.CMCNonRenewal;
                        this.subject = "Comprehensive Maternal Care Non-Renewal Notification";
                        this.GetCMCRecipients(CON.CMC_CommunicationLetterTypeID.CMC_NON_RENEWAL_NOTIFICATION_LETTER);
                        break;
                }

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to fetch the " + this.template + " Recipients. Exception Message: "
                                + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        private void GetCMCRecipients(int letterTypeId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("CMC_COMMUNICATION_LETTER_TYPE_ID", DbType.Int32, letterTypeId, false));
            recipients = DataAccess.ExecuteStoredProcedure("usp_SelectCMCEmailRecipients", parameters, "EmailRecipients");
            SendNotice(this.template);

            UpdateCMCNoticeStatus(letterTypeId, recipients);
        }

        private static void UpdateCMCNoticeStatus(int letterTypeId, DataSet recipients)
        {
            if (Methods.HasRows(recipients))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                foreach (DataRow row in recipients.Tables[0].Rows)
                {
                    string regId = Methods.GetStringValue(row, "REG_ID");
                    parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, Convert.ToInt32(regId), false));
                    parameters.Add(SqlParms.CreateParameter("CMC_COMMUNICATION_LETTER_TYPE_ID", DbType.Int32, letterTypeId, false));
                    DataAccess.ExecuteStoredProcedure("usp_UpdateCMCNoticeStatus", parameters);

                    parameters.Clear();
                }
            }            
        }

        private void SendNotice(string template)
        {
            string emailSubject = this.subject;
            if (Methods.HasRows(recipients))
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                int sendToTypeId = 17; //TODO
                foreach (DataRow row in recipients.Tables[0].Rows)
                {
                    string regId = Methods.GetStringValue(row, "REG_ID");
                    parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, Convert.ToInt32(regId), false));
                    parameters.Add(SqlParms.CreateParameter("SendToTypeID", DbType.Int32, sendToTypeId, false));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectEmailRecipients", parameters, "RecipientEmail");
                    string recipientEmail = Methods.GetStringValue(ds.Tables[0].Rows[0], "EmailAddress");
                    EMailNotification enotify = new EMailNotification(template, emailSubject, recipientEmail, this.guid);
                    enotify.SendWorkFlowEngineNotification(regId, template);
                    parameters.Clear();
                }
            }
        }

        private void EndDateCMCAfterOpenEnrollmentEnds()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            log.CreateLogEntry("Getting regid's for CMC year end job.");
            try
            {
                
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectProviders_EndDateCMCSpecialty");
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (Methods.HasRows(ds))
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        int regID = 0;
                        try
                        {
                            regID = Methods.GetIntValue(row, "REG_ID");
                            parameters.Add(SqlParms.CreateParameter("RegID", DbType.Int32, regID, false));
                            DataAccess.ExecuteStoredProcedure("usp_EndDateCMCSpecialty_AfterOpenEnrollment", parameters);
                            parameters.Clear();
                        }
                        catch(Exception ex) 
                        {
                            log.CreateLogEntry("Failed to end date MIS Specialty for RegID - " + regID.ToString() + " . Exception Message: "
                              + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to fetch the providers to year end job. Exception Message: "
                               + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }


            
            
            
        }

    }
}

using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class CPCCMCTermination : BaseJob, IJob
    {
        private Logging log = null;

        public CPCCMCTermination(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "5B397E38-7CBE-4A1D-A883-B490CC255C1F";

        override public void ExecuteJob()
        {
            // Default Job - Delegate Affiliation File Processing
            this.ExecuteJob(Guid.Parse(appID));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    ProcessCPCCMCTermination();
                    break;
            }
        }

        private void ProcessCPCCMCTermination()
        {
            // create log object
            string logMsg = string.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                // create log entry
                log.CreateLogEntry("Getting data related to CPC CMC Termination process.");

                // create local path if it does not exist
                int noticesSent = 0;
                bool noticeSent = false;
                string username = string.Empty;
                string email = string.Empty;
                string MedicaidId = string.Empty;
                DataTable dt = GetDataForProcess();
                DataTable dSpecial = GetSpecialistData();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        MedicaidId = ObjectControllerHelper.GetString("MEDICAID_ID", row);
                        foreach (DataRow dspecialistRow in dSpecial.Rows)
                        {
                            username = ObjectControllerHelper.GetString("UserName", dspecialistRow);
                            email = ObjectControllerHelper.GetString("Email", dspecialistRow);

                            noticeSent = SendOutEmails(username, email, MedicaidId);
                            noticesSent++;
                        }
                    }
                }
                log.CreateLogEntry(string.Format("Total Emails sent to Notification group {0}", noticesSent.ToString()));
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }
        private DataTable GetSpecialistData()
        {
            DataSet DsSpecialist = new DataSet();
            DsSpecialist = DataAccess.ExecuteStoredProcedure("usp_SelectAllAPMSpecialist");
            return DsSpecialist.Tables[0];
        }
        private DataTable GetDataForProcess()
        {
            DataSet ds = new DataSet();

            ds = DataAccess.ExecuteStoredProcedure("usp_SelectAllNotProcessedCPC");
            DataTable dt = ds.Tables[0];

            ds = new DataSet();
            ds = DataAccess.ExecuteStoredProcedure("usp_SelectAllNotProcessedCMC");

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Merge(ds.Tables[0]);
                dt.AcceptChanges();
            }
            else
                dt = ds.Tables[0];
            return dt;
        }

        private bool SendOutEmails(string username, string email, string MedicaidId)
        {
            bool noticeSent = false;
            string subject = Constants.CMCCPCTerminationType.subject;
            string templateFile = Constants.CMCCPCTerminationType.emailTemplateName;

            try
            {
                Notification n = new Notification();
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("USERNAME", username);
                fields.Add("MEDICAIDID", MedicaidId);
                string body = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                string pwdTemplateContent = null;
                using (StreamReader reader = new StreamReader(templateActualPath + templateFile))
                {
                    pwdTemplateContent = reader.ReadToEnd();
                }
                body = n.ParseEmailBody(pwdTemplateContent, fields);
                EMailNotification notify = new EMailNotification(body, subject, email);
                body = notify.SendActualNotification(templateActualPath + templateFile, fields, true);

                noticeSent = true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while sending out Emails {0}", ex.ToString()));
            }
            return noticeSent;
        }
    }
}